using Xunit;
using Org.VeChain.Thor.Devkit.Extension;
using Org.VeChain.Thor.Devkit.Certificate;

namespace Org.VeChain.Thor.Devkit.UnitTest
{
    public class CertificateTest
    {
        [Fact]
        public void TestCertificateVerify()
        {
            Certificate.Certificate info = new Certificate.Certificate();
            info.Purpose = "identification";
            info.Payload = new CertificatePayload();
            info.Payload.Type = "text";
            info.Payload.Content = "fyi";
            info.Domain = "localhost";
            info.Timestamp = 1545035330;
            info.Signer = "0x7567d83b7b8d80addcb281a71d54fc7b3364ffed";

            byte[] msgHash = CertificateCoder.SigningHash(info);

            var signature = Cry.Secp256k1.Sign(msgHash,"0xdce1443bd2ef0c2631adc1c67e5c93f13dc23a41c18b536effbbdcbcdb96fb65".ToBytes());
            info.Signature = signature;

            Assert.True(CertificateCoder.Verify(info));
        }
    }
}