using System.Linq;
using Xunit;
using Xunit.Abstractions;
using Org.VeChain.Thor.Devkit.Extension;

namespace Org.VeChain.Thor.Devkit.UnitTest
{
    public class HashTest
    {
        private readonly ITestOutputHelper _output;

        public HashTest(ITestOutputHelper testOutputHelper)
        {
            _output = testOutputHelper;
        }

        [Fact]
        public void TestBlake2b256()
        {
            byte[] result = "0x256c83b297114d201b30179f3f0ef0cace9783622da5974326b436178aeef610".ToBytes();
            byte[] hash = Cry.Blake2b.CalculateHash("hello world");
            Assert.True(result.SequenceEqual(hash));
        }

        [Fact]
        public void TestKeccack256()
        {
            byte[] result = "0x47173285a8d7341e5e972fc677286384f802f8ef42a5ec5f03bbfa254cb01fad".ToBytes();
            byte[] hash = Cry.Keccack256.CalculateHash("hello world");
            Assert.True(result.SequenceEqual(hash));
        }
    }
}