﻿using BrainFuckSharp.Lib.Domain;

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
                    else
                    {
                        type = TokenType.Other;
                        result.AddRange(CreateInstructions(current, type, sum));
                    }
                }
                else
                {
                    result.AddRange(CreateInstructions(current, type, sum));
                    current = instructions[i];
                    sum = SetSumFromCurrent(current);
                    type = TokenType.Other;
                }
            }

            result.AddRange(CreateInstructions(current, type, sum));


            return result;
        }

        private static int SetSumFromCurrent(IInstruction current)
        {
            return current is IValue value ? value.Value : 1;
        }

        private static IEnumerable<IInstruction> CreateInstructions(IInstruction instruction, TokenType type, int sum)
        {
            switch(type)
            {
                case TokenType.Pointer:
                    yield return new PointerMove { Value = sum };
                    break;
                case TokenType.Increment:
                    yield return new Increment { Value = sum };
                    break;
                case TokenType.Other:
                    if (instruction is Loop loop)
                    {
                        yield return new Loop { Instructions = Compress(loop.Instructions) };
                    }
                    else
                    {
                        yield return instruction;
                    }
                    break;
            }
        }

        private static bool CanUnroll(Loop loop)
        {
            return loop.All(x => x is Increment or PointerMove) && loop.Instructions.Count > 1;
        }
    }
}
