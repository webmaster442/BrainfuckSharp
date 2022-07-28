using System.Reflection;
using System.Runtime.Loader;
using System.Text;

using BrainFuckSharp.Lib.Domain;
using BrainFuckSharp.Lib.Internals;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Emit;

namespace BrainFuckSharp.Lib
{
    internal class CompilngInterpreter
    {
        private readonly HashSet<PortableExecutableReference> _references;
        private readonly CSharpCompilationOptions _compilerOptions;
        private readonly string _appDir;

        private byte[] _memory;
        private int _programCounter;

        public CompilngInterpreter(int memoryLimit = 30_000)
        {
            _references = new HashSet<PortableExecutableReference>();
            ReferenceNetStandard();
            AddTypeReference<object>();
            AddTypeReference<ICompilerContract>();
            _compilerOptions = new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary)
                .WithPlatform(Platform.AnyCpu)
                .WithNullableContextOptions(NullableContextOptions.Enable)
                .WithOverflowChecks(true)
                .WithOptimizationLevel(OptimizationLevel.Release);


            _appDir = Path.Combine(AppContext.BaseDirectory, "compiled");
            if (!Directory.Exists(_appDir))
                Directory.CreateDirectory(_appDir);

            _memory = new byte[memoryLimit];
            _programCounter = 0;
        }

        private void ReferenceNetStandard()
        {
            if (!(AppContext.GetData("TRUSTED_PLATFORM_ASSEMBLIES") is string trusted))
                throw new InvalidOperationException("Can't locate Trusted platform assemblies");

            string[]? trustedAssembliesPaths = trusted.Split(Path.PathSeparator);

            string[]? neededAssemblies = new[]
            {
                "System.Runtime",
                "netstandard",
            };
            IEnumerable<PortableExecutableReference>? references = trustedAssembliesPaths
                .Where(p => neededAssemblies.Contains(Path.GetFileNameWithoutExtension(p)))
                .Select(p => MetadataReference.CreateFromFile(p));

            foreach (PortableExecutableReference? reference in references)
            {
                _references.Add(reference);
            }
        }

        private void AddTypeReference<TType>()
        {
            string location = typeof(TType).GetTypeInfo().Assembly.Location;
            _references.Add(MetadataReference.CreateFromFile(location));
        }

        private IEnumerable<SyntaxTree> GenerateCode(IEnumerable<IInstruction> instructions, ulong hash)
        {
            StringBuilder sb = new(4096 * 2);
            foreach (var instruction in instructions)
            {
                sb.AppendLine(instruction.ToCsharp());
            }
            StringBuilder finalProgram = new(Properties.Resources.Template);
            finalProgram.Replace("%ProgramHash%", hash.ToString());
            finalProgram.Replace("%code%", sb.ToString());

            SyntaxTree tree = SyntaxFactory.ParseSyntaxTree(finalProgram.ToString(), CSharpParseOptions.Default.WithLanguageVersion(LanguageVersion.Latest));
            yield return tree;
        }

        public void Run(string file)
        {
            var result =  ProgramCodeReader.ReadCode(file);
            var compressed = TokenCompressor.Compress(Tokenizer.Tokenize(result.code));

            var compiledFile = Path.Combine(_appDir, $"{result.hash}.dll");

            if (!File.Exists(compiledFile))
            {
                Assembly compiled = Compile(compiledFile, compressed, result.hash);
                Run(compiled);
            }
            else
            {
                Assembly loaded = LoadCompiled(compiledFile);
                Run(loaded);
            }

        }

        private void Reset()
        {
            Array.Clear(_memory, 0, _memory.Length);
            _programCounter = 0;
        }

        private void Run(Assembly assembly)
        {
            Type? contract = typeof(ICompilerContract);
            var t = assembly.GetTypes().Where(x => x.IsClass && contract.IsAssignableFrom(x)).FirstOrDefault();
            if (Activator.CreateInstance(t) is ICompilerContract loaded)
            {
                Reset();
                loaded.Run(ref _memory, ref _programCounter);
            }
        }

        private Assembly LoadCompiled(string compiledFile)
        {
            using (var file = File.OpenRead(compiledFile))
            {
                return AssemblyLoadContext.Default.LoadFromStream(file);
            }
        }

        private Assembly Compile(string compiledFile, IList<IInstruction> compressed, ulong hash)
        {
            CSharpCompilation compiler = CSharpCompilation.Create($"{hash}.dll")
                .WithOptions(_compilerOptions)
                .AddReferences(_references.ToArray())
                .AddSyntaxTrees(GenerateCode(compressed, hash));

            using (var file = File.Create(compiledFile))
            {
                EmitResult emitResult = compiler.Emit(file);
                if (!emitResult.Success)
                {
                    string? details = string.Join('\n', emitResult.Diagnostics);
                    throw new InvalidOperationException($"Compile error\n{details}");
                }
                file.Seek(0, SeekOrigin.Begin);
                return AssemblyLoadContext.Default.LoadFromStream(file);
            }
        }
    }
}
