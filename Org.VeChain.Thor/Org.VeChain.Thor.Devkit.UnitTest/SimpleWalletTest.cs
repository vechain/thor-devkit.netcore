using Xunit;
using Org.VeChain.Thor.Devkit.Cry;
using Org.VeChain.Thor.Devkit.Extension;

namespace Org.VeChain.Thor.Devkit.UnitTest
{
    public class SimpleWalletTest
    {
        [Fact]
        public void TestGenerateVeChainKey()
        {
            var priKey = "0xdce1443bd2ef0c2631adc1c67e5c93f13dc23a41c18b536effbbdcbcdb96fb65".ToBytes();
            var vechainKey = new SimpleWallet(priKey);
            Assert.True(vechainKey.address == "0x7567d83b7b8d80addcb281a71d54fc7b3364ffed");
        }

        [Fact]
        public void TestPrivateKeyToAddress()
        {
            var priKey = "0xdce1443bd2ef0c2631adc1c67e5c93f13dc23a41c18b536effbbdcbcdb96fb65".ToBytes();
            Assert.True(SimpleWallet.PrivateKeyToAddress(priKey) == "0x7567d83b7b8d80addcb281a71d54fc7b3364ffed");
        }

        [Fact]
        public void TestPublicKeyToAddress()
        {
            var pubKey = "0x04cfd9860ac8f054e1cf77801cebe587d4f8dc35d96f22f7e140ec058ac9c6ab730c1f4682f0c1b24302aba2a8d6e8e0f5046c1a13bcb8356ab48ccb73f552e143".ToBytes();
            var address = "0x08cbc6738798954761cf84fd5da9f48141fc520b";
            Assert.True(SimpleWallet.PublicKeyToAddress(pubKey) == address);
        }

        [Fact]
        public void TestIsValidAddress()
        {
            var address1 = "0x7567d83b7b8d80addcb281a71d54fc7b3364ffed";
            var address2 = "0x7567d83b7b8d80addcb2";
            var address3 = "hello world";

            Assert.True(SimpleWallet.IsValidAddress(address1));
            Assert.False(SimpleWallet.IsValidAddress(address2));
            Assert.False(SimpleWallet.IsValidAddress(address3));
        }

        [Fact]
        public void TestToChecksumAddress()
        {
            var address = "0x7567d83b7b8d80addcb281a71d54fc7b3364ffed";
            var checksumAddress = SimpleWallet.ToChecksumAddress(address);
            Assert.True(checksumAddress == "0x7567D83b7b8d80ADdCb281A71d54Fc7B3364ffed");
        }

        [Fact]
        public void TestIsChecksumAddress()
        {
            var checksumAddress1 = "0x7567D83b7b8d80ADdCb281A71d54Fc7B3364ffed";
            var checksumAddress2 = "0x7567d83b7b8d80ADdCb281a71d54FC7B3364ffeD";
            var checksumAddress3 = "hello world";

            Assert.True(SimpleWallet.IsChecksumAddress(checksumAddress1));
            Assert.False(SimpleWallet.IsChecksumAddress(checksumAddress2));
            Assert.False(SimpleWallet.IsChecksumAddress(checksumAddress3));
        }

        [Fact]
        public void TestRecoverAddressTest()
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