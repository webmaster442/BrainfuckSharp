namespace BrainFuckSharp.Lib.Domain
{
    internal sealed record class Increment : IInstruction, IValue
    {
        public int Value { get; set; }
    }
}
