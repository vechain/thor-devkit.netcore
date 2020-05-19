using System.Linq;
using System;
using Xunit;
using System.Text;
using Xunit.Abstractions;
using Org.VeChain.Thor.Devkit.Cry;
using Org.VeChain.Thor.Devkit.Extension;

namespace Org.VeChain.Thor.Devkit.UnitTest
{
    public class Secp256k1Test
    {
        private readonly ITestOutputHelper _output;

        public Secp256k1Test(ITestOutputHelper testOutputHelper)
        {
            _output = testOutputHelper;
        }

        [Fact]
        public void TestIsValidPrivateKey1()
        {
            var priKey1 = "0xdce1443bd2ef0c2631adc1c67e5c93f13dc23a41c18b536effbbdcbcdb96fb65";
            var priKey2 = "0x1c67e5c93f13dc23a41c18b536effbb1";

            Assert.True(Secp256k1.IsValidPrivateKey(priKey1.HexToByteArray()));
            Assert.False(Secp256k1.IsValidPrivateKey(priKey2.HexToByteArray()));
        }

        [Fact]
        public void TestSecp256k1()
        {
            var priKey = Secp256k1.GeneratePrivateKey();
            var pubKey = Secp256k1.DerivePublicKey(priKey);

            var msgHash = Keccack256.CalculateHash("hello world");

            var signature = Secp256k1.Sign(msgHash,priKey);

            var recoveredPubKey = Secp256k1.RecoverPublickey(Keccack256.CalculateHash("hello world"),signature);

            Assert.True(pubKey.SequenceEqual(recoveredPubKey));

        }
    }
}
