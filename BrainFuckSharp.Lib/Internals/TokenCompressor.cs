using BrainFuckSharp.Lib.Domain;

namespace BrainFuckSharp.Lib.Internals
{
    internal static class TokenCompressor
    {
        private enum TokenType
        {
            Other,
            Increment,
            Pointer
        }

        public static IList<IInstruction> Compress(IList<IInstruction> instructions)
        {
            List<IInstruction> result = new();

            if (instructions.Count == 0)
                return result;

            TokenType type = TokenType.Other;
            IInstruction current = instructions[0];
            int sum = SetSumFromCurrent(current);
            for (int i = 1; i < instructions.Count; i++)
            {
                if (current.GetType() == instructions[i].GetType())
                {
                    if (instructions[i] is Increment inc)
                    {
                        type = TokenType.Increment;
                        sum += inc.Value;
                    }
                    else if (instructions[i] is PointerMove pm)
                    {
                        type = TokenType.Pointer;
                        sum += pm.Value;
                    }
                }
                else
                {
                    result.Add(Create(current, type, sum));
                    current = instructions[i];
                    sum = SetSumFromCurrent(current);
                    type = TokenType.Other;
                }
            }

            result.Add(Create(current, type, sum));


            return result;
        }

        private static int SetSumFromCurrent(IInstruction current)
        {
            if (current is IValue value)
                return value.Value;
            return 1;
        }

        private static IInstruction Create(IInstruction instruction, TokenType type, int sum)
        {
            return type switch
            {
                TokenType.Other => OptimizeIfLoop(instruction),
                TokenType.Pointer => new PointerMove { Value = sum },
                TokenType.Increment => new Increment { Value = sum },
                _ => throw new InvalidOperationException("Unknown token type"),
            };
        }

        private static IInstruction OptimizeIfLoop(IInstruction instruction)
        {
            if (instruction is Loop loop)
                return new Loop { Instructions = Compress(loop.Instructions) };
            else
                return instruction;
        }
    }
}
