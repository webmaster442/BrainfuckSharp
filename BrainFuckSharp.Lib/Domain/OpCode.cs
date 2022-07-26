namespace BrainFuckSharp.Lib.Domain
{
    internal enum OpCode
    {
        Increment = 0,
        PointerMove = 1,
        Input = 3,
        Output = 2,
        LoopStart = 6,
        LoopEnd = 7,
        MultiplyAdd = 5,
    }
}
