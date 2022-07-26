namespace BrainFuckSharp.Lib.Domain
{
    internal record struct MultAdd : IInstruction, IValue
    {
        public int Offset { get; init; }
        public int Value { get; init; }
    }
}
