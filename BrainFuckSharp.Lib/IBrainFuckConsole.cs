namespace BrainFuckSharp.Lib
{
    public interface IBrainFuckConsole
    {
        void Write(char c);
        char Read();
    }
}
