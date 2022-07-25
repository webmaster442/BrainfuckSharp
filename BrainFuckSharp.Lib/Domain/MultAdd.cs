namespace BrainFuckSharp.Lib.Domain
{
    internal sealed record class MultAdd : IInstruction, IValue
    {
        public int Offset { get; set; }
        public int Value { get; set; }
    }
}
