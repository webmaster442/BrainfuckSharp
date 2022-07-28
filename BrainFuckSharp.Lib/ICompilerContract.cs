namespace BrainFuckSharp.Lib
{
    internal interface ICompilerContract
    {
        void Run(ref byte[] memory, ref int pointer);
    }
}
