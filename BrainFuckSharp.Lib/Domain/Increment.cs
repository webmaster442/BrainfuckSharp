namespace BrainFuckSharp.Lib.Domain
{
    internal record struct Increment : IInstruction, IValue
    {
        public int Value { get; set; }
    }
}
