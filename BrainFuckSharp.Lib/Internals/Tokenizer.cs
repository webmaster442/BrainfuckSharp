using BrainFuckSharp.Lib.Domain;

namespace BrainFuckSharp.Lib.Internals
{
    internal static class Tokenizer
    {
        public static IList<IInstruction> Tokenize(string input)
        {
            int i = 0;
            int depth = 0;
            var result = Tokenize(input, ref i, ref depth);

            if (depth < 0)
                throw new InvalidOperationException("Extra loop closing in program");
            else if (depth > 0)
                throw new InvalidOperationException("Unclosed loop in program");

            return result;
        }

        private static IList<IInstruction> Tokenize(string input, ref int i, ref int depth)
        {
            var tokens = new List<IInstruction>();
            while (i < input.Length)
            {
                char token = input[i];
                if (token == '[')
                {
                    i++;
                    ++depth;
                    var loop = new Loop { Instructions = Tokenize(input, ref i, ref depth) };
                    tokens.Add(loop);
                }
                else if (token == ']')
                {
                    i++;
                    --depth;
                    return tokens;
                }
                else
                {
                    IInstruction? parsed = Create(input[i]);
                    if (parsed != null)
                        tokens.Add(parsed);
                    i++;
                }
            }

            return tokens;
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
