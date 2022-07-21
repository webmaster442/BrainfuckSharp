namespace BrainFuckSharp.Tests
{
    [TestFixture]
    public class CsharpGeneratorTests
    {
        [Test]
        public void EnsureThat_CsharpGenerator_ProducesExpected_SimpleProgram()
        {
            var generator = new CsharpGenerator();
            string result =  generator.GenerateCsharpCode(".+[.+]", "test");

            Assert.That(result, Is.EqualTo(Resources.SimpleProgram));
        }

        [Test]
        public void EnsureThat_CsharpGenerator_ProducesExpected_HelloWorld()
        {
            var generator = new CsharpGenerator();
            string result = generator.GenerateCsharpCode("++++++++++[>+++++++>++++++++++>+++>+<<<<-]>++.>+.+++++++..+++.>++.<<+++++++++++++++.>.+++.------.--------.>+.>.", "test");

            Assert.That(result, Is.EqualTo(Resources.HelloWorld));
        }

    }
}
