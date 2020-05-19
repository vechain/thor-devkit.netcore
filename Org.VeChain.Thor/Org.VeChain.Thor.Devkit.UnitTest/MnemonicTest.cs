using Org.VeChain.Thor.Devkit.Cry;
using Xunit;
using System.Linq;
using Org.VeChain.Thor.Devkit.Extension;

namespace Org.VeChain.Thor.Devkit.UnitTest
{
    public class MnemonicTest
    {
        [Fact]
        public void TestWords()
        {
            var works = new string[12]{"mouse","brave","fun","viable","utility","veteran","luggage","area","bike","myself","target","thunder"};
            var priKey = Mnemonic.DerivePrivateKey(works);
            Assert.True(Mnemonic.Validate(works));
            Assert.True(Secp256k1.IsValidPrivateKey(priKey));
            Assert.True(priKey.SequenceEqual("0x12ddf96bb7f2c031ee9b776e2b236f2fc46de6f90fb9cd48a82e0c849327d251".HexToByteArray()));
        }
    }
}