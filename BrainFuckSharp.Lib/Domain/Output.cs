namespace BrainFuckSharp.Lib.Domain
{
    internal record struct Output : IInstruction
    {
        public void Emmit(IJitWriter jitWriter)
        {
            jitWriter.WriteInstruction(OpCode.Output);
        }

        public override string ToString()
        {
            return nameof(Output);
        }
    }

}
