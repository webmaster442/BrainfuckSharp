namespace BrainFuckSharp.Lib.Domain
{
    internal record struct Input : IInstruction
    {
        public string ToCsharp()
        {
            return "memory[pointer] = (byte)Console.Read();";
        }

        public override string ToString()
        {
            return nameof(Input);
        }
    }
}
