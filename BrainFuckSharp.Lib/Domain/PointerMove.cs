namespace BrainFuckSharp.Lib.Domain
{
    internal sealed record class PointerMove : IInstruction, IValue
    {
        public int Value { get; set; }
    }
}
