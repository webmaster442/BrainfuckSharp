namespace BrainFuckSharp.Lib.Domain
{
    internal record struct MultAdd : IInstruction, IValue
    {
        public int Offset { get; init; }
        public int Value { get; init; }

        public void Emmit(IJitWriter jitWriter)
        {
            jitWriter.WriteInstruction(OpCode.MultiplyAdd, Offset, Value);
        }
    }
}
