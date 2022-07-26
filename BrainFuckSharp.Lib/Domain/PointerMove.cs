namespace BrainFuckSharp.Lib.Domain
{
    internal record struct PointerMove : IInstruction, IValue
    {
        public int Value { get; set; }
    }
}
