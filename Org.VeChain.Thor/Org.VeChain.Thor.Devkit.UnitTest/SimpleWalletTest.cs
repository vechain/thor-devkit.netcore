using Xunit;
using Org.VeChain.Thor.Devkit.Cry;
using Org.VeChain.Thor.Devkit.Extension;

namespace Org.VeChain.Thor.Devkit.UnitTest
{
    public class SimpleWalletTest
    {
        [Fact]
        public void GenerateVeChainKeyTest()
        {
            var priKey = "0xdce1443bd2ef0c2631adc1c67e5c93f13dc23a41c18b536effbbdcbcdb96fb65".ToBytes();
            var vechainKey = new SimpleWallet(priKey);
            Assert.True(vechainKey.address == "0x7567d83b7b8d80addcb281a71d54fc7b3364ffed");
        }

        [Fact]
        public void PrivateKeyToAddress()
        {
            var priKey = "0xdce1443bd2ef0c2631adc1c67e5c93f13dc23a41c18b536effbbdcbcdb96fb65".ToBytes();
            Assert.True(SimpleWallet.PrivateKeyToAddress(priKey) == "0x7567d83b7b8d80addcb281a71d54fc7b3364ffed");
        }

        [Fact]
        public void PublicKeyToAddressTest()
        {
            var pubKey = "0x0465e790f6065164e2f610297b5358b6c474f999fb5b4d2574fcaffccb59342c1f6f28f0b684ec97946da65cd08a1b9fc276f79d90caed80e56456cebbc165938e".ToBytes();
            var address = "0x7567d83b7b8d80addcb281a71d54fc7b3364ffed";
            Assert.True(SimpleWallet.PublicKeyToAddress(pubKey) == address);
        }

        [Fact]
        public void IsValidAddressTest()
        {
            var address1 = "0x7567d83b7b8d80addcb281a71d54fc7b3364ffed";
            var address2 = "0x7567d83b7b8d80addcb2";
            var address3 = "hello world";

            Assert.True(SimpleWallet.IsValidAddress(address1));
            Assert.False(SimpleWallet.IsValidAddress(address2));
            Assert.False(SimpleWallet.IsValidAddress(address3));
        }

        [Fact]
        public void ToChecksumAddress()
        {
            var address = "0x7567d83b7b8d80addcb281a71d54fc7b3364ffed";
            var checksumAddress = SimpleWallet.ToChecksumAddress(address);
            Assert.True(checksumAddress == "0x7567D83b7b8d80ADdCb281A71d54Fc7B3364ffed");
        }

        [Fact]
        public void IsChecksumAddress()
        {
            var checksumAddress1 = "0x7567D83b7b8d80ADdCb281A71d54Fc7B3364ffed";
            var checksumAddress2 = "0x7567d83b7b8d80ADdCb281a71d54FC7B3364ffeD";
            var checksumAddress3 = "hello world";

            Assert.True(SimpleWallet.IsChecksumAddress(checksumAddress1));
            Assert.False(SimpleWallet.IsChecksumAddress(checksumAddress2));
            Assert.False(SimpleWallet.IsChecksumAddress(checksumAddress3));
        }

        [Fact]
        public void RecoverAddressTest()
        {
            var priKey = "0xdce1443bd2ef0c2631adc1c67e5c93f13dc23a41c18b536effbbdcbcdb96fb65".ToBytes();
            var vechainKey = new SimpleWallet(priKey);

            var msgHash = Keccack256.CalculateHash("hello world");

            var signature = Secp256k1.Sign(msgHash,priKey);

            var recoverAddress = SimpleWallet.RecoverAddress(msgHash,signature);

            Assert.True(recoverAddress == "0x7567d83b7b8d80addcb281a71d54fc7b3364ffed");
        }

    }
}