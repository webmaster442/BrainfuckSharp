namespace BrainFuckSharp.Lib.Domain
{
    internal record struct Increment : IInstruction, IValue
    {
        public int Value { get; set; }

        public void Emmit(IJitWriter jitWriter)
        {
            jitWriter.WriteInstruction(OpCode.Increment, Value);
        }

        public override string ToString()
        {
            return $"{nameof(Increment)} => {Value}";
        }
    }
}
