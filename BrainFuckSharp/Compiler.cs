
using BrainFuckSharp.Lib;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.Extensions.DependencyModel;

namespace BrainFuckSharp
{
    internal sealed class Compiler
    {
        private readonly CSharpCompilationOptions _options;

        public Compiler()
        {
            _options
                = new CSharpCompilationOptions(OutputKind.ConsoleApplication)
                .WithAllowUnsafe(false)
                .WithOverflowChecks(true)
                .WithPlatform(Platform.AnyCpu)
                .WithDeterministic(true)
                .WithOptimizationLevel(OptimizationLevel.Release);
        }

        public void Compile(CompilerOptions options)
        {
            CsharpGenerator generator = new();
            var csharpSource = generator.GenerateCsharpCode(options.BrainFuckSource, options.Namespace, options.MemoryLimit);

            var tree = CSharpSyntaxTree.ParseText(csharpSource);

            var compilation = CSharpCompilation.Create(options.OutputFileName, new[] { tree }, GetMetadataReference(), _options);

            compilation.Emit(options.OutputFileName);
        }

        private static IEnumerable<MetadataReference>? GetMetadataReference()
        {
            return DependencyContext.Default.CompileLibraries
                  .First(cl => cl.Name == "Microsoft.NETCore.App")
                  .ResolveReferencePaths()
                  .Select(asm => MetadataReference.CreateFromFile(asm))
                  .ToArray();
        }
    }
}
