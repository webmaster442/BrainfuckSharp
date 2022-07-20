namespace BrainFuckSharp.Tests
{
    [TestFixture]
    public class TokenCompressorTests
    {
        [TestCase("++++++--", 4)]
        [TestCase("++++++--.", 4)]
        [TestCase("+-++--", 0)]
        [TestCase("--++", 0)]
        public void EnsureThat_TokenCompressor_Compresses_Increments(string input, int expected)
        {
            IList<IInstruction>? raw = Tokenizer.Tokenize(input);
            IList<IInstruction>? result = TokenCompressor.Compress(raw);

            Assert.Multiple(() =>
            {
                Assert.That(result[0] is Increment, Is.True);
                Assert.That((result[0] as Increment)?.Value, Is.EqualTo(expected));
            });
        }

        [TestCase(">>>>>><<", 4)]
        [TestCase(">>>>>><<.", 4)]
        [TestCase("><>><<", 0)]
        [TestCase("<<>>", 0)]

        public void EnsureThat_TokenCompressor_Compresses_PointerMoves(string input, int expected)
        {
            IList<IInstruction>? raw = Tokenizer.Tokenize(input);
            IList<IInstruction>? result = TokenCompressor.Compress(raw);

            Assert.Multiple(() =>
            {
                Assert.That(result[0] is PointerMove, Is.True);
                Assert.That((result[0] as PointerMove)?.Value, Is.EqualTo(expected));
            });
        }

        [Test]
        public void EnsureThat_TokenCompressor_Compresses_Complex1()
        {
            IList<IInstruction>? raw = Tokenizer.Tokenize("++.>>[.+]");
            IList<IInstruction>? result = TokenCompressor.Compress(raw);

            IInstruction[]? expected = new IInstruction[]
            {
                new Increment { Value = 2 },
                new Output(),
                new PointerMove { Value = 2 },
                new Loop
                {
                    new Output(),
                    new Increment { Value = 1 }
                }
            };

            CollectionAssert.AreEqual(expected, result);
        }

        [Test]
        public void EnsureThat_TokenCompressor_Compresses_Complex2()
        {
            IList<IInstruction>? raw = Tokenizer.Tokenize(">>.++[.+]");
            IList<IInstruction>? result = TokenCompressor.Compress(raw);

            IInstruction[]? expected = new IInstruction[]
            {
                new PointerMove { Value = 2 },
                new Output(),
                new Increment { Value = 2 },
                new Loop
                {
                    new Output(),
                    new Increment { Value = 1 }
                }
            };

            CollectionAssert.AreEqual(expected, result);
        }
    }
}
