﻿namespace BrainFuckSharp.Lib.Domain
{
    internal record struct Input : IInstruction
    {
        public void Emmit(IJitWriter jitWriter)
        {
            jitWriter.WriteInstruction(OpCode.Input);
        }
    }
}
