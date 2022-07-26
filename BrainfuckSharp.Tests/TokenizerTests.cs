namespace BrainFuckSharp.Tests
{
    [TestFixture]
    public class TokenizerTests
    {
        private readonly IInstruction[] SimpleTestCase = new IInstruction[]
        {
            new Increment { Value = 1},
            new Increment { Value = -1 },
            new Input(),
            new Output(),
            new PointerMove {Value = 1},
            new PointerMove {Value = -1},
        };

        private readonly IInstruction[] LoopTestCase = new IInstruction[]
        {
            new Loop
            {
                new Output(),
                new Increment { Value = 1},
            }
        };

        private readonly IInstruction[] MultiLoopTestCase = new IInstruction[]
        {
            new Loop
            {
                new Loop
                {
                    new Loop
                    {
                        new Output()
                    }
                }
            }
        };

        [Test]
        public void EnsureThat_Tokenizer_TokenizeSimple()
        {
            IList<IInstruction>? tokens = Tokenizer.Tokenize("+-,.><");
            CollectionAssert.AreEqual(SimpleTestCase, tokens);
        }

        [Test]
        public void EnsureThat_Tokenizer_TokenizeLoops()
        {
            IList<IInstruction>? tokens = Tokenizer.Tokenize("[.+]");
            CollectionAssert.AreEqual(LoopTestCase, tokens);
        }

        [Test]
        public void EnsureThat_Tokenizer_Tokenize_MultiLoop()
        {
            IList<IInstruction>? tokens = Tokenizer.Tokenize("[[[.]]]");
            CollectionAssert.AreEqual(MultiLoopTestCase, tokens);
        }

        [TestCase("#[.+]")]
        [TestCase("#[ő.+]")]
        [TestCase("#[ő.á+]")]
        [TestCase("#[ő.+4 ] ")]
        public void EnsureThat_Tokenizer_TokenizeLoops_WithExtraChars(string input)
        {
            IList<IInstruction>? tokens = Tokenizer.Tokenize(input);
            CollectionAssert.AreEqual(LoopTestCase, tokens);
        }

        [TestCase("[]]")]
        [TestCase("[[]")]
        [TestCase("[[[]]")]
        [TestCase("[[]]]")]
        public void EnsureThat_Tokenizer_Throws_UnballancedLoops(string input)
        {
            Assert.Throws<InvalidOperationException>(() =>
            {
                Tokenizer.Tokenize(input);
            });
        }
    }
}
