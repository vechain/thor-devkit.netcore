using System;
using Newtonsoft.Json.Linq;
using Org.VeChain.Thor.Devkit.Cry;
using Newtonsoft.Json;

namespace Org.VeChain.Thor.Devkit.Certificate
{
    public class CertificateCoder
    {
        /// <summary>
        /// use black2b to calculate the certificate info hash
        /// </summary>
        /// <param name="certificate"></param>
        /// <returns></returns>
        public static byte[] SigningHash(ICertificate certificate)
        {
            JToken json = CertificateCoder.ConvertToJsonObjectWithNoSignature(certificate);
            return Blake2b.CalculateHash(json.ToString(Formatting.None));
        }

        /// <summary>
        /// verify certificate signture
        /// </summary>
        /// <param name="certificate"></param>
        /// <returns></returns>
        public static bool Verify(ICertificate certificate)
        {
            bool result = false;
            if(certificate.Signer == null || certificate.Signature.Length != 65)
            {
                throw new ArgumentException("invalid signature");
            }

            try
            {
                byte[] msgHash = SigningHash(certificate);
                byte[] publicKey = Secp256k1.RecoverPublickey(msgHash,certificate.Signature);
                return certificate.Signer.Equals(SimpleWallet.PublicKeyToAddress(publicKey));
            }
            catch
            {
                result = false;
            }

            return result;
        }

        private static JToken ConvertToJsonObjectWithNoSignature(ICertificate certificate)
        {
            JToken payload = new JObject();
            payload["content"] = certificate.Payload.Content;
            payload["type"] = certificate.Payload.Type;

            JToken certificateJson = new JObject();
            certificateJson["domain"] = certificate.Domain;
            certificateJson["payload"] = payload;
            certificateJson["purpose"] = certificate.Purpose;
            certificateJson["signer"] = certificate.Signer.ToLower();
            certificateJson["timestamp"] = certificate.Timestamp;
            return certificateJson;
        }
    
    }
}