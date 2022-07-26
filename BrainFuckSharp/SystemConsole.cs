using BrainFuckSharp.Lib;

namespace BrainFuckSharp
{
    internal class SystemConsole : IBrainFuckConsole
    {
        public char Read()
        {
            return (char)Console.Read();
        }

        public void Write(char c)
        {
            Console.Write(c);
        }
    }
}