using System.Text;

namespace BrainFuckSharp.Tests
{
    internal class TestConsole : IBrainFuckConsole
    {
        private readonly StringBuilder _buffer;
        private readonly string _inputBuffer;
        private int _index = 0;

        public TestConsole(string inputbuffer)
        {
            _buffer = new StringBuilder(4096);
            _inputBuffer = inputbuffer;
            _index = 0;
        }

        public char Read()
        {
            char c = _inputBuffer[_index];
            _index++;
            return c;
        }

        public void Write(char c)
        {
            _buffer.Append(c);
        }

        public override string ToString()
        {
            return _buffer.ToString();
        }
    }
}
