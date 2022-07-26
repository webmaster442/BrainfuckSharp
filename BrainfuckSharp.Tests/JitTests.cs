namespace BrainFuckSharp.Tests
{
    [TestFixture]
    public class JitTests
    {
        private MemoryStream? _stream;
        private IInstruction[] _emmited;

        [SetUp]
        public void Setup()
        {
            _stream = new MemoryStream(4096);
            _emmited = Tokenizer.Tokenize(".+[.+]").ToArray();
            using (JitWriter writer = new JitWriter(_stream, 1))
            {
                writer.Emmit(_emmited);
            }
            _stream.Seek(0, SeekOrigin.Begin);
        }

        [TearDown]
        public void TearDown()
        {
            _stream?.Dispose();
            _stream = null;
        }


        [Test]
        public void TestRestore()
        {
            using (var reader = new IJitReader(_stream))
            {
                var restored = reader.ReadInstructions().ToArray();

                Assert.That(reader.OriginalProgramHash, Is.EqualTo(1));
                CollectionAssert.AreEqual(_emmited, restored);
            }
        }
    }
}
