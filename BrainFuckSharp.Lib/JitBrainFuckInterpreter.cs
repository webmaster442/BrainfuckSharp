
using BrainFuckSharp.Lib.Domain;
using BrainFuckSharp.Lib.Internals;

namespace BrainFuckSharp.Lib
{
    public class JitBrainFuckInterpreter : BrainFuckInterpreter
    {
        public JitBrainFuckInterpreter(IBrainFuckConsole console, int memoryLimit = 30000) : base(console, memoryLimit)
        {
        }

        public override void Execute(string programFile)
        {
            Reset();

            if (!File.Exists(programFile))
                throw new InvalidOperationException("File doesn't exist");

            string program = ReadProgram(programFile, out ulong hash);

            string jitFile = Path.ChangeExtension(programFile, "bfjit");

            if (File.Exists(jitFile))
            {
                using (var jitStream = File.OpenRead(jitFile))
                {
                    using (IJitReader reader = new IJitReader(jitStream))
                    {
                        if (reader.OriginalProgramHash == hash)
                        {
                            RunInstructions(reader.ReadInstructions().ToList(), false);
                            return;
                        }
                    }
                }
            }

            CreateJitAndRun(program, jitFile, hash);
        }

        private void CreateJitAndRun(string program, string jitFile, ulong hash)
        {
            IList<IInstruction>? instructions = TokenCompressor.Compress(Tokenizer.Tokenize(program));

            using (var jitStream = File.Create(jitFile))
            {
                using (var jitWtiter = new JitWriter(jitStream, hash))
                {
                    jitWtiter.Emmit(instructions);
                }
            }
            RunInstructions(instructions);
        }

        private static string ReadProgram(string programFile, out ulong hash)
        {
            ulong hashValue = 14695981039346656037UL;
            using (var reader = File.OpenText(programFile))
            {
                var content = reader.ReadToEnd();
                foreach (var c in content)
                {
                    hashValue = (hashValue ^ c) * 1099511628211UL;
                }
                hash = hashValue;
                return content;
            }
        }
    }
}
