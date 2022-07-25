using System.Text;

using BrainFuckSharp.Lib.Domain;
using BrainFuckSharp.Lib.Internals;

namespace BrainFuckSharp.Lib
{
    public class CsharpGenerator
    {
        private readonly StringBuilder _buffer;
        private const int _tabSize = 4;
        private const string NamespaceToken = "%namespace%";
        private const string MemoryLimitToken = "%memorylimit%";
        private const string CodeToken = "%code%";

        public CsharpGenerator()
        {
            _buffer = new StringBuilder(4096);
        }

        private void WriteLine(string code, int indentlevel)
        {
            string? padding = "".PadLeft(indentlevel * _tabSize, ' ');
            _buffer.Append(padding);
            _buffer.Append(code);
            _buffer.Append("\r\n");
        }

        public string GenerateCsharpCode(string brainfFuckCode, string @namespace, int memoryLimit = 4096)
        {
            IList<IInstruction>? instructions = TokenCompressor.Compress(Tokenizer.Tokenize(brainfFuckCode));

            StringBuilder finalBuffer = new(4096 * 2);
            Generate(instructions);
            finalBuffer.Append(Properties.Resources.Template);
            finalBuffer.Replace(NamespaceToken, @namespace);
            finalBuffer.Replace(MemoryLimitToken, memoryLimit.ToString());
            finalBuffer.Replace(CodeToken, _buffer.ToString());
            return finalBuffer.ToString();
        }

        private void Generate(IEnumerable<IInstruction> instructions, int level = 3)
        {
            foreach (IInstruction? instruction in instructions)
            {
                if (instruction is Increment increment)
                {
                    if (increment.Value == 1)
                        WriteLine("Memory[CellCounter]++;", level);
                    else if (increment.Value == -1)
                        WriteLine("Memory[CellCounter]--;", level);
                    else
                        WriteLine($"Memory[CellCounter] += {increment.Value};", level);
                }
                else if (instruction is PointerMove pointerMove)
                {
                    WriteLine($"CellCounter += {pointerMove.Value};", level);
                }
                else if (instruction is Loop loop)
                {
                    WriteLine("while (Memory[CellCounter] != 0)", level);
                    WriteLine("{", level);
                    Generate(loop.Instructions, level + 1);
                    WriteLine("}", level);
                }
                else if (instruction is Output)
                {
                    WriteLine("Console.Write((char)Memory[CellCounter]);", level);
                }
                else if (instruction is Input)
                {
                    WriteLine("Memory[CellCounter] = Console.Read();", level);
                }
            }
        }
    }
}
