namespace BrainFuckSharp.Lib.Internals
{
    internal static class ProgramCodeReader
    {
        private static readonly HashSet<char> ValidTokens = new("+-.,><[]");

        public  static (string code, ulong hash) ReadCode(string fileName)
        {
            var code = File.ReadAllText(fileName);
            ulong hash = 2;
            for (int i=0; i < code.Length; i++)
            {
                if (ValidTokens.Contains(code[i]))
                {
                    hash = (hash * 5) + code[i];
                }
            }
            return(code, hash);
        }

    }
}
