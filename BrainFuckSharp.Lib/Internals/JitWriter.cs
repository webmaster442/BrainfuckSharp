using System.Text;

using BrainFuckSharp.Lib.Domain;

namespace BrainFuckSharp.Lib.Internals
{
    internal sealed class JitWriter : IDisposable, IJitWriter
    {
        private readonly BinaryWriter _writer;

        public JitWriter(Stream stream, ulong originalProgramHash)
        {
            _writer = new BinaryWriter(stream, Encoding.ASCII, true);
            _writer.Write(originalProgramHash);
        }

        public void Emmit(IEnumerable<IInstruction> instructions)
        {
            foreach (var instruction in instructions)
            {
                instruction.Emmit(this);
            }
        }

        public void Dispose()
        {
            _writer.Dispose();
        }

        void IJitWriter.WriteInstruction(OpCode opCode)
        {
            _writer.Write((int)opCode);
        }

        void IJitWriter.WriteInstruction(OpCode opCode, int param)
        {
            _writer.Write((int)opCode);
            _writer.Write(param);
        }

        void IJitWriter.WriteInstruction(OpCode opCode, int param1, int param2)
        {
            _writer.Write((int)opCode);
            _writer.Write(param1);
            _writer.Write(param2);
        }
    }
}
