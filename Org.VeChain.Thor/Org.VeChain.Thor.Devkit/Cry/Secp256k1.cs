using System;
using System.Text;
using Nethereum.Signer.Crypto;
using System.Collections.Generic;
using Org.BouncyCastle.Math;
using System.Linq;
using Org.BouncyCastle.Asn1;

namespace Org.VeChain.Thor.Devkit.Cry
{
    public class Secp256k1
    {
        public static byte[] N = Encoding.UTF8.GetBytes("fffffffffffffffffffffffffffffffebaaedce6af48a03bbfd25e8cd0364141");

        public static byte[] ZERO = new byte[32];

        public static bool IsValidPrivateKey(byte[] privatekey)
        {
            return privatekey.Length == 32 && !privatekey.Equals(Secp256k1.ZERO) && (new BigInteger(privatekey)).CompareTo(new BigInteger(N)) < 0;
        }

        public delegate byte[] generatePrivateKeyHandler();

        public static byte[] GeneratePrivateKey(generatePrivateKeyHandler handler = null)
        {
            if (handler == null)
            {
                handler = delegate {
                    byte[] newKey = new byte[32];
                    Random rand = new Random();
                    rand.NextBytes(newKey);
                    return newKey;
                };
            }
            return handler();
        }

        public static byte[] DerivePublicKey(byte[] privatekey)
        {
            if (IsValidPrivateKey(privatekey))
            {
                ECKey key = new ECKey(privatekey,true);
                return key.GetPubKey(false);
            }
            else
            {
                throw new ArgumentException("invalid private key");
            }
        }

        public static byte[] Sign(byte[] msgHash, byte[] privatekey)
        {

            ECKey ecKey = new ECKey(privatekey,true);
            var sign = ecKey.Sign(msgHash);
            var recId = Secp256k1.CalculateRecId(ecKey,sign, msgHash);
            sign.V = new byte[1]{Convert.ToByte(recId)};
            List<byte> signature = new List<byte>();
            signature.AddRange(sign.R.ToByteArrayUnsigned());
            signature.AddRange(sign.S.ToByteArrayUnsigned());
            signature.AddRange(sign.V);
            return signature.ToArray(); 
        }

        public static byte[] RecoverPublickey(byte[] msgHash, byte[] signature)
        {
            if(signature.Length != 65)
            {
                throw new ArgumentException("signature invalid");
            }

            byte[] r = new byte[32];
            byte[] s = new byte[32];
            byte[] v = new byte[1];

            try
            {
                Array.Copy(signature, 0, r, 0, 32);
                Array.Copy(signature, 32, s, 0, 32);
                Array.Copy(signature, 64, v, 0, 1);

                int V = Convert.ToInt32(v[0]);

                if( V != 0 && V != 1)
                {
                    throw new ArgumentException("invalid signature recovery");
                }

                ECDSASignature sign = new ECDSASignature(new DerInteger(r).PositiveValue,new DerInteger(s).PositiveValue);
                ECKey ecKey = ECKey.RecoverFromSignature(V,sign,msgHash,false);
                return ecKey.GetPubKey(false);
            }
            catch
            {
                throw new ArgumentException("signature invalid");
            }
        }

        private static int CalculateRecId(ECKey eckey,ECDSASignature signature, byte[] hash)
        {
            var thisKey = eckey.GetPubKey(false);
            return CalculateRecId(signature,hash,thisKey);
        }

        private static int CalculateRecId(ECDSASignature signature, byte[] hash, byte[] uncompressedPublicKey)
        {
             var recId = -1;

            for (var i = 0; i < 4; i++)
            {
                var rec = ECKey.RecoverFromSignature(i, signature, hash, false);
                if (rec != null)
                {
                    var k = rec.GetPubKey(false);
                    if (k != null && k.SequenceEqual(uncompressedPublicKey))
                    {
                        recId = i;
                        break;
                    }
                }
            }
            if (recId == -1)
                throw new ArgumentException("Could not construct a recoverable key. This should never happen.");
            return recId;
        }
    }
}
