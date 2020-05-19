using System.Linq;
using Xunit;
using Org.VeChain.Thor.Devkit.Extension;
using Org.VeChain.Thor.Devkit.Cry;

namespace Org.VeChain.Thor.Devkit.UnitTest
{
    public class HashTest
    {
        [Fact]
        public void TestBlake2b256()
        {
            byte[] result = "0x256c83b297114d201b30179f3f0ef0cace9783622da5974326b436178aeef610".HexToByteArray();
            byte[] hash = Blake.CalculateHash("hello world");
            Assert.True(result.SequenceEqual(hash));
        }

        [Fact]
        public void TestKeccack256()
        {
            byte[] result = "0x47173285a8d7341e5e972fc677286384f802f8ef42a5ec5f03bbfa254cb01fad".HexToByteArray();
            byte[] hash = Keccack256.CalculateHash("hello world");
            Assert.True(result.SequenceEqual(hash));
        }
    }
}