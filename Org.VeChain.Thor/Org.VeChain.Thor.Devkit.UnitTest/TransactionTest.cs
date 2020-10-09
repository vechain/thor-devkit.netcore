using Xunit;
using Org.VeChain.Thor.Devkit.Transaction;
using System.Numerics;
using Org.VeChain.Thor.Devkit.Extension;
using System.Linq;
using Org.VeChain.Thor.Devkit.Rlp;

namespace Org.VeChain.Thor.Devkit.UnitTest
{
    public class TransactionTest
    {
        public Body TestTxBody()
        {
            var txbody = new Body
            {
                ChainTag = 74,
                BlockRef = "0x005d64da8e7321bd",
                Expiration = 18,
                GasPriceCoef = 0,
                Gas = 21000,
                DependsOn = "",
                Nonce = "0xd6846cde87878603",
                Reserved = null
            };
            txbody.Clauses.Add(new Clause("0xa4aDAfAef9Ec07BC4Dc6De146934C7119341eE25",new BigInteger(100000),"0x2398479812734981"));
            txbody.Clauses.Add(new Clause("0xa4aDAfAef9Ec07BC4Dc6De146934C7119341eE25",new BigInteger(100000),"0x2398479812734981"));
            txbody.Clauses.Add(new Clause("0xa4aDAfAef9Ec07BC4Dc6De146934C7119341eE25",new BigInteger(100000),"0x2398479812734981"));
            return txbody;
        }

        [Fact]
        public void TestRlpEncode()
        {
            var txbody = this.TestTxBody();
            byte[] encode = new Transaction.Transaction(txbody).Encode();
            var encode2 = "f8844a875d64da8e7321bd12f869e294a4adafaef9ec07bc4dc6de146934c7119341ee25830186a0882398479812734981e294a4adafaef9ec07bc4dc6de146934c7119341ee25830186a0882398479812734981e294a4adafaef9ec07bc4dc6de146934c7119341ee25830186a0882398479812734981808252088088d6846cde87878603c0".ToBytes();
            Assert.True(encode.SequenceEqual(encode2));
        }

        [Fact]
        public void TestNoClauses()
        {
            var txbody = new Body
            {
                ChainTag = 74,
                BlockRef = "0x005d64da8e7321bd",
                Expiration = 18,
                GasPriceCoef = 0,
                Gas = 21000,
                DependsOn = "",
                Nonce = "0xd6846cde87878603",
                Reserved = null
            };

            byte[] encode = new Transaction.Transaction(txbody).Encode();
            var encode2 = "0xda4a875d64da8e7321bd12c0808252088088d6846cde87878603c0".ToBytes();
            Assert.True(encode.SequenceEqual(encode2));
        }

        [Fact]
        public void TestHaveUnused()
        {
            var txbody = new Body
            {
                ChainTag = 74,
                BlockRef = "0x005d64da8e7321bd",
                Expiration = 18,
                GasPriceCoef = 0,
                Gas = 21000,
                DependsOn = "",
                Nonce = "0xd6846cde87878603",
                Reserved = new Reserved
                {
                    Features = 1,
                    Unused = new System.Collections.Generic.List<byte[]>
                    {
                        "0xa4aDAfAef9Ec07BC4Dc6De146934C7119341eE25".ToBytes()
                    }
                }
            };

            byte[] encode = new Transaction.Transaction(txbody).Encode();
            var encode2 = "0xf04a875d64da8e7321bd12c0808252088088d6846cde87878603d60194a4adafaef9ec07bc4dc6de146934c7119341ee25".ToBytes();
            Assert.True(encode.SequenceEqual(encode2));
        }

        [Fact]
        public void TestRlpDecode()
        {
            var rlpcode = "f8844a875d64da8e7321bd12f869e294a4adafaef9ec07bc4dc6de146934c7119341ee25830186a0882398479812734981e294a4adafaef9ec07bc4dc6de146934c7119341ee25830186a0882398479812734981e294a4adafaef9ec07bc4dc6de146934c7119341ee25830186a0882398479812734981808252088088d6846cde87878603c0".ToBytes();
            var transaction = Transaction.Transaction.Decode(rlpcode);
            Assert.True(transaction != null);
        }

        [Fact]
        public void TestSign()
        {
            var txbody = this.TestTxBody();
            var transaction = new Transaction.Transaction(txbody);
            transaction.Signature = Cry.Secp256k1.Sign(transaction.SigningHash(),"0xdce1443bd2ef0c2631adc1c67e5c93f13dc23a41c18b536effbbdcbcdb96fb65".ToBytes());
            
            (Transaction.Transaction.UnsignedRlpDefinition() as RlpStructKind)?.EncodeToRlp(transaction.Body);

            Assert.True(transaction.Signature.SequenceEqual("0x5ca73236f37c9d7b29f32af81e4812a33cfbfac573544684e374e2fb90c637740d406198252371fa3e64341e4bd0226268a774e4112913429750b3fb3fad56bf00".ToBytes()));
            Assert.True(transaction.Origin == "0x7567d83b7b8d80addcb281a71d54fc7b3364ffed");
            Assert.True(transaction.Id == "0xd3ad13b5db7be0a3ecc64ff537442047baa5d1df9593237e173fa097be2e7182");
        }

        [Fact]
        public void TestDelegate()
        {
            var delegatedBody = new Body
            {
                ChainTag = 74,
                BlockRef = "0x005d64da8e7321bd",
                Expiration = 18,
                GasPriceCoef = 0,
                Gas = 21000,
                DependsOn = "",
                Nonce = "0xd6846cde87878603",
                Reserved = new Reserved {Features = 1}
            };
            delegatedBody.Clauses.Add(new Clause("0xa4aDAfAef9Ec07BC4Dc6De146934C7119341eE25",new BigInteger(100000),"0x2398479812734981"));
            delegatedBody.Clauses.Add(new Clause("0xa4aDAfAef9Ec07BC4Dc6De146934C7119341eE25",new BigInteger(100000),"0x2398479812734981"));
            delegatedBody.Clauses.Add(new Clause("0xa4aDAfAef9Ec07BC4Dc6De146934C7119341eE25",new BigInteger(100000),"0x2398479812734981"));

            var transaction = new Transaction.Transaction(delegatedBody);
            Assert.True(transaction.IsDelegated);

            var senderPrikey = "0xdce1443bd2ef0c2631adc1c67e5c93f13dc23a41c18b536effbbdcbcdb96fb65";
            var senderAddr = "0x7567d83b7b8d80addcb281a71d54fc7b3364ffed";

            var gasPayerPrikey = "0x321d6443bc6177273b5abf54210fe806d451d6b7973bccc2384ef78bbcd0bf51";
            var geaPayerAddr = "0xd3ae78222beadb038203be21ed5ce7c9b1bff602";

            Assert.True(transaction.SigningHash().SequenceEqual("0xb07661afea87bf07d3a76742950b1607174090bc2241a32e40f8d35e3747e0ad".ToBytes()));

            var senderSigningHash = transaction.SigningHash();
            var gasPayerSigningHash = transaction.SigningHash(senderAddr);

            var senderSignature = Cry.Secp256k1.Sign(senderSigningHash,senderPrikey.ToBytes());
            var gasPayerSignature = Cry.Secp256k1.Sign(gasPayerSigningHash,gasPayerPrikey.ToBytes());

            transaction.AddVIP191Signature(senderSignature,gasPayerSignature);

            Assert.True(transaction.Id == "0xf6a2eab557187af50e4a3abb8a65894eb3be4d58a557b31e8f878ca91c0d9f4e");
            Assert.True(transaction.Origin == senderAddr);
            Assert.True(transaction.Delegator == geaPayerAddr);
        }
    
    }
}