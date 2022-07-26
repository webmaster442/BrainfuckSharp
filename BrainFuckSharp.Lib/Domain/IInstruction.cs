namespace BrainFuckSharp.Lib.Domain
{
    internal interface IInstruction
    {
        void Emmit(IJitWriter jitWriter);
    }
}
