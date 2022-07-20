using BrainFuckSharp.Lib.Domain;
using BrainFuckSharp.Lib.Internals;

namespace BrainFuckSharp.Lib
{
    public class BrainFuckInterpreter
    {
        private readonly IBrainFuckConsole _console;
        private readonly int[] _memory;
        private int _programCounter;

        public BrainFuckInterpreter(IBrainFuckConsole console, int memoryLimit = 4096)
        {
            _console = console;
            _memory = new int[memoryLimit];
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
                    _memory[_programCounter] += increment.Value;
                    ClampToByteArithmetics();
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
                        if (_memory[_programCounter] < 0 || _memory[_programCounter] > 255)
                            ClampToByteArithmetics();
                    }
                }
            }
        }

        private void ClampToByteArithmetics()
        {
            //standard uint8_t behavior replication
            if (_memory[_programCounter] > 255)
            {
                _memory[_programCounter] = _memory[_programCounter] - 255 - 1;
            }
            else if (_memory[_programCounter] < 0)
            {
                //in this case _memory[_programCounter] is minus! so we add!
                _memory[_programCounter] = 255 + _memory[_programCounter] + 1;
            }
        }

        private void DoInput()
        {
            _memory[_programCounter] = _console.Read();
        }

        private void DoOutput()
        {
            char c = (char)_memory[_programCounter];
            _console.Write(c);
        }
    }
}
