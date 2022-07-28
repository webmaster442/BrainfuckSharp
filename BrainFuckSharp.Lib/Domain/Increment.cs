namespace BrainFuckSharp.Lib.Domain
{
    internal record struct Increment : IInstruction, IValue
    {
        public int Value { get; set; }

        public string ToCsharp()
        {
            if (Value == -1)
                return "memory[pointer]--;";
            else if (Value == 1)
                return "memory[pointer]++;" ;
            else
                return $"memory[pointer] += {Value};";
        }

        public override string ToString()
        {
            return $"{nameof(Increment)} => {Value}";
        }
    }
}
