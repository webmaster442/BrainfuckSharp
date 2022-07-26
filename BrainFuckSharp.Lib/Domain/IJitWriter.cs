namespace BrainFuckSharp.Lib.Domain
{
    internal interface IJitWriter
    {
        void WriteInstruction(OpCode opCode);
        void WriteInstruction(OpCode opCode, int param);
        void WriteInstruction(OpCode opCode, int param1, int param2);
    }
}
