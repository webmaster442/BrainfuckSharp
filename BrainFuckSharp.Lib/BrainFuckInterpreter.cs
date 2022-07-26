using BrainFuckSharp.Lib.Domain;
using BrainFuckSharp.Lib.Internals;

namespace BrainFuckSharp.Lib
{
    public class BrainFuckInterpreter
    {
        private readonly IBrainFuckConsole _console;
        private readonly byte[] _memory;
        private int _programCounter;

        public BrainFuckInterpreter(IBrainFuckConsole console, int memoryLimit = 30_000)
        {
            _console = console;
            _memory = new byte[memoryLimit];
        }

        public void Execute(string program)
        {
            Array.Clear(_memory, 0, _memory.Length);
            _programCounter = 0;
            IList<IInstruction>? instructions = TokenCompressor.Compress(Tokenizer.Tokenize(program));
            RunInstructions(instructions);
        }

        private void RunInstructions(IList<IInstruction> instructions, bool insideLoop = false)
        {
            foreach (IInstruction? instruction in instructions)
            {
                if (instruction is Increment increment)
                {
                    int value = _memory[_programCounter] + increment.Value;
                    _memory[_programCounter] = (byte)value;
                }
                else if (instruction is MultAdd mAdd)
                {
                    int value = _memory[_programCounter] * mAdd.Value;
                    _memory[_programCounter + mAdd.Offset] = (byte)value;
                }
                else if (instruction is PointerMove pointerMove)
                {
                    _programCounter += pointerMove.Value;
                }
                else if (instruction is Output)
                {
                    DoOutput();
                }
                else if (instruction is Input)
                {
                    DoInput();
                }
                else if (instruction is Loop loop)
                {
                    while (_memory[_programCounter] != 0)
                    {
                        RunInstructions(loop.Instructions, true);
                    }
                }
                else
                {
                    throw new InvalidOperationException("Unknown instruction");
                }
            }
        }

        private void DoInput()
        {
            _memory[_programCounter] = (byte)_console.Read();
        }

        private void DoOutput()
        {
            char c = (char)_memory[_programCounter];
            _console.Write(c);
        }
    }
}
