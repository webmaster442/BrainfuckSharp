using BrainFuckSharp.Lib.Domain;

namespace BrainFuckSharp.Lib.Internals
{
    internal static class Tokenizer
    {
        public static IList<IInstruction> Tokenize(string input)
        {
            return Tokenize(input, 0, true, out _);
        }

        private static IList<IInstruction> Tokenize(string input, int startindex, bool maincall, out int parsed)
        {
            List<IInstruction>? tokens = new List<IInstruction>();
            int p = 0;
            for (int i = startindex; i < input.Length; i++)
            {
                char chr = input[i];
                if (chr == '[')
                {
                    Loop? l = new Loop
                    {
                        Instructions = Tokenize(input, i + 1, false, out int increment)
                    };
                    tokens.Add(l);
                    i += increment + 1;
                }
                else if (chr == ']')
                {
                    /*if (maincall)
                    {
                        throw new InvalidOperationException("Extra loop closing");
                    }*/
                    /*else
                    {*/
                        parsed = p;
                        return tokens;
                    ///}
                }
                else
                {
                    IInstruction? t = Create(chr);
                    ++p;
                    if (t != null)
                    {
                        tokens.Add(t);
                    }
                }
            }
            if (maincall)
            {
                parsed = p;
                return tokens;
            }
            else
            {
                throw new InvalidOperationException("Unclosed loop");
            }
        }

        private static IInstruction? Create(char chr)
        {
            return chr switch
            {
                '+' => new Increment { Value = 1 },
                '-' => new Increment { Value = -1 },
                '>' => new PointerMove { Value = 1 },
                '<' => new PointerMove { Value = -1 },
                '.' => new Output(),
                ',' => new Input(),
                _ => null,
            };
        }
    }
}
