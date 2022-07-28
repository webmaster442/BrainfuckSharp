namespace BrainFuckSharp.Lib.Domain
{
    internal record struct Output : IInstruction
    {
        public string ToCsharp()
        {
            return "Console.Write((char)memory[pointer]);";
        }

        public override string ToString()
        {
            return nameof(Output);
        }
    }

}
