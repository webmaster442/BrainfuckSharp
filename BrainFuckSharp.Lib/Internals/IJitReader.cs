using System.Text;

using BrainFuckSharp.Lib.Domain;

namespace BrainFuckSharp.Lib.Internals
{
    internal sealed class IJitReader: IDisposable
    {
        private readonly BinaryReader _reader;

        public IJitReader(Stream stream)
        {
            _reader = new BinaryReader(stream, Encoding.ASCII, true);
            OriginalProgramHash = _reader.ReadUInt64();
        }

        public ulong OriginalProgramHash { get; }

        public IEnumerable<IInstruction> ReadInstructions()
        {
            while (CanRead(1))
            {
                OpCode code = (OpCode)_reader.ReadInt32();

                IInstruction? instruction = Create(code);

                if (instruction != null)
                    yield return instruction;
                else
                    break;
            }
        }

        private IInstruction? Create(OpCode code)
        {
            if (code == OpCode.Output)
                return new Output();
            else if (code == OpCode.Input)
                return new Input();
            else if (code == OpCode.Increment && CanRead(1))
                return new Increment { Value = _reader.ReadInt32() };
            else if (code == OpCode.PointerMove && CanRead(1))
                return new PointerMove { Value = _reader.ReadInt32() };
            else if (code == OpCode.MultiplyAdd && CanRead(2))
                return new MultAdd { Offset = _reader.ReadInt32(), Value = _reader.ReadInt32() };
            else if (code == OpCode.LoopStart)
                return CreateLoop();
            else
                return null;
        }

        private IInstruction? CreateLoop()
        {
            List<IInstruction> instructions = new();
            while (CanRead(1))
            {
                OpCode code = (OpCode)_reader.ReadInt32();
                if (code != OpCode.LoopEnd)
                {
                    var created = Create(code);
                    if (created != null)
                        instructions.Add(created);
                }
                else
                {
                    return new Loop { Instructions = instructions };
                }
            }
            return null;
        }

        public bool CanRead(int paramCount)
        {
            return _reader.BaseStream.Position + (paramCount * sizeof(int)) <= _reader.BaseStream.Length;
        }

        public void Dispose()
        {
            _reader.Dispose();
        }
    }
}
