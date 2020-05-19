using Org.VeChain.Thor.Devkit.Cry;
using Org.VeChain.Thor.Devkit.Extension;
using Xunit;
using System.Linq;

namespace Org.VeChain.Thor.Devkit.UnitTest
{
    public class KeystoreTest
    {
        [Fact]
        public void TestKeystore()
        {
            var priKey = "0xdce1443bd2ef0c2631adc1c67e5c93f13dc23a41c18b536effbbdcbcdb96fb65";
            var keystore = Keystore.EncryptToJson(priKey.HexToByteArray(),"123456789");
            var recoveredPriKey = Keystore.DecryptFromJson(keystore,"123456789");
            Assert.True(recoveredPriKey.SequenceEqual(priKey.HexToByteArray()));
        }
    }
}