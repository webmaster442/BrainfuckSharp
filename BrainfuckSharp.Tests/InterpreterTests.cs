namespace BrainFuckSharp.Tests
{
    [TestFixture]
    public class InterpreterTests
    {
        [Timeout(1000)]
        [TestCase("", "")]
        [TestCase("----[---->+<]>++.", "A")]
        [TestCase("----[---->+<]>++.-[-->+++<]>+.", "Aa")]
        [TestCase("+[--->++<]>++++.", "Z")]
        [TestCase("+[--->++<]>+.++[->++++<]>+.", "We")]
        [TestCase("++++++++++[>+>+++>+++++++>++++++++++<<<<-]>>>-----.", "A")]
        [TestCase("++++++++++[>+++++++>++++++++++>+++>+<<<<-]>++.>+.+++++++..+++.>++.<<+++++++++++++++.>.+++.------.--------.>+.>.", "Helo World!\n")]
        [TestCase("++++++++++[>+>+++>+++++++>++++++++++<<<<-]>>>-----.+.+.+.+.+.+.+.+.+.+.+.+.+.+.+.+.+.+.+.+.+.+.+.+.+.", "ABCDEFGHIJKLMNOPQRSTUVWXYZ")]
        [TestCase("++++++++++[>+>+++>+++++++>++++++++++<<<<-]>>>>-------------.++++++++++++++.+++++++.---------.++++++++++++.--.--------.<<++.>>+++++++++++++++.-----.<<.>>+++++.------------.---.<<.>>++.------.+++++++++++++.-------.<<+.", "Welcome to the gang!")]
        [TestCase("+[--->++<]>+.++[->++++<]>+.+++++++.---------.++++++++++++.--.--------.--[--->+<]>-.---[->++++<]>.-----.[--->+<]>-----.---[->++++<]>.------------.---.--[--->+<]>-.+++[->++<]>+.-[------>+<]>.+++++++++++++.-------.-[--->+<]>-.", "Welcome to the Gang!")]
        public void EnsureThat_InterpreterWorks(string bf, string expected)
        {
            TestConsole? testConsole = new TestConsole("");
            BrainFuckInterpreter brainFuckInterpreter = new(testConsole);
            brainFuckInterpreter.Execute(bf);
            Assert.That(testConsole.ToString(), Is.EqualTo(expected));
        }

        [Timeout(1000)]
        [Test]
        public void EnsureThat_Interpreter_Loop_Works()
        {
            TestConsole? testConsole = new TestConsole("");
            BrainFuckInterpreter brainFuckInterpreter = new(testConsole);
            brainFuckInterpreter.Execute(".+[.+]");
            string? str = testConsole.ToString();

            char[]? expected = Enumerable.Range(0, 256).Select(i => (char)i).ToArray();


            Assert.That(str.ToArray(), Is.EqualTo(expected));

        }
    }
}
