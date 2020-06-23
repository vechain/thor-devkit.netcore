using System;
using Newtonsoft.Json.Linq;
using Org.VeChain.Thor.Devkit.Extension;
using Org.VeChain.Thor.Devkit.Cry;

namespace Org.VeChain.Thor.Devkit.Certificate
{
    public class CertificateCoder
    {
        public CertificateCoder(string certificateJson)
        {
            this._certificate = (new CertificateBuilder()).Builder(certificateJson);
        }

        public CertificateCoder(ICertificate certificate)
        {
            this._certificate = certificate;
        }

        public bool Verify()
        {
            bool result = false;
            if(this._certificate.Signer == null || this._certificate.Signature.Length != 65)
            {
                throw new ArgumentException("invalid signature");
            }

            try
            {
                JToken json = this.ConvertToJsonObjectWithNoSigner();
                byte[] msgHash = Blake2b.CalculateHash(json.ToString());

                byte[] publicKey = Secp256k1.RecoverPublickey(msgHash,this._certificate.Signature);
                return this._certificate.Signer.Equals(SimpleWallet.PublicKeyToAddress(publicKey));
            }
            catch
            {
                result = false;
            }

            return result;
        }

        private ICertificate _certificate;

        private JToken ConvertToJsonObjectWithNoSigner()
        {
            JToken certificateJson = new JObject();
            certificateJson["purpose"] = this._certificate.Purpose;
            certificateJson["payload"]["type"] = this._certificate.Payload.Type;
            certificateJson["payload"]["content"] = this._certificate.Payload.Content;
            certificateJson["domain"] = this._certificate.Domain;
            certificateJson["timestamp"] = this._certificate.Timestamp;
            certificateJson["signer"] = this._certificate.Signer;
            return certificateJson;
        }
    

    }
}