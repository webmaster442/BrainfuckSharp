namespace BrainFuckSharp.Tests
{
    [TestFixture]
    public class InterpreterTests
    {
        private TestConsole _testConsole;
        private BrainFuckInterpreter _brainFuckInterpreter;

        [SetUp]
        public void Setup()
        {
            _testConsole = new TestConsole("");
            _brainFuckInterpreter = new(_testConsole);
        }

        [Timeout(1000)]
        [TestCase("", "")]
        [TestCase("----[---->+<]>++.", "A")]
        [TestCase("----[---->+<]>++.-[-->+++<]>+.", "Aa")]
        [TestCase("+[--->++<]>++++.", "Z")]
        [TestCase("+[--->++<]>+.++[->++++<]>+.", "We")]
        [TestCase("++++++++++[>+>+++>+++++++>++++++++++<<<<-]>>>-----.", "A")]
        [TestCase("++++++++[>++++[>++>+++>+++>+<<<<-]>+>+>->>+[<]<-]>>.>---.+++++++..+++.>>.<-.<.+++.------.--------.>>+.>++.", "Hello World!\n")]
        [TestCase("++++++++++[>+++++++>++++++++++>+++>+<<<<-]>++.>+.+++++++..+++.>++.<<+++++++++++++++.>.+++.------.--------.>+.>.", "Hello World!\n")]
        [TestCase("++++++++++[>+>+++>+++++++>++++++++++<<<<-]>>>-----.+.+.+.+.+.+.+.+.+.+.+.+.+.+.+.+.+.+.+.+.+.+.+.+.+.", "ABCDEFGHIJKLMNOPQRSTUVWXYZ")]
        [TestCase("++++++++++[>+>+++>+++++++>++++++++++<<<<-]>>>>-------------.++++++++++++++.+++++++.---------.++++++++++++.--.--------.<<++.>>+++++++++++++++.-----.<<.>>+++++.------------.---.<<.>>++.------.+++++++++++++.-------.<<+.", "Welcome to the gang!")]
        [TestCase("+[--->++<]>+.++[->++++<]>+.+++++++.---------.++++++++++++.--.--------.--[--->+<]>-.---[->++++<]>.-----.[--->+<]>-----.---[->++++<]>.------------.---.--[--->+<]>-.+++[->++<]>+.-[------>+<]>.+++++++++++++.-------.-[--->+<]>-.", "Welcome to the Gang!")]
        [TestCase("++++++++++>+++++<[>[->+>+<<]>[-<+>]<<-]>[-]>>[-<<<+>>>]<<<.", "2")]
        public void EnsureThat_InterpreterWorks(string bf, string expected)
        {
            _brainFuckInterpreter.Execute(bf);
            Assert.That(_testConsole.ToString(), Is.EqualTo(expected));
        }

        [Timeout(1000)]
        [Test]
        public void EnsureThat_Interpreter_Loop_Works()
        {
            _brainFuckInterpreter.Execute(".+[.+]");
            string? str = _testConsole.ToString();

            char[]? expected = Enumerable.Range(0, 256).Select(i => (char)i).ToArray();

            Assert.That(str.ToArray(), Is.EqualTo(expected));

        }
    }
}
