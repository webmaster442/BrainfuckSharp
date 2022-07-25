namespace BrainFuckSharp
{
    internal sealed record class CompilerOptions
    {
        public string Namespace { get; init; }
        public int MemoryLimit { get; init; }
        public string BrainFuckSource { get; init; }
        public string OutputFileName { get; init; }

        public CompilerOptions()
        {
            Namespace = "BrainFuck";
            MemoryLimit = 4096;
            BrainFuckSource = string.Empty;
            OutputFileName = string.Empty;
        }
    }
}
