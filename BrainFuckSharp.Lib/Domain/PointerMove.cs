namespace BrainFuckSharp.Lib.Domain
{
    internal record struct PointerMove : IInstruction, IValue
    {
        public int Value { get; set; }

        public void Emmit(IJitWriter jitWriter)
        {
            jitWriter.WriteInstruction(OpCode.PointerMove, Value);
        }

        public override string ToString()
        {
            return $"{nameof(PointerMove)} => {Value}";
        }
    }
}
