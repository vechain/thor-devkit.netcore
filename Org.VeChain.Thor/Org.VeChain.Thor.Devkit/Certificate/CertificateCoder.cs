using System;
using Newtonsoft.Json.Linq;
using Org.VeChain.Thor.Devkit.Extension;
using Org.VeChain.Thor.Devkit.Cry;
using Newtonsoft.Json;

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

        public byte[] SigningHash()
        {
            JToken json = this.ConvertToJsonObjectWithNoSignature();
            return Blake2b.CalculateHash(json.ToString(Formatting.None));
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
                byte[] msgHash = this.SigningHash();
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

        private JToken ConvertToJsonObjectWithNoSignature()
        {
            JToken payload = new JObject();
            payload["content"] = this._certificate.Payload.Content;
            payload["type"] = this._certificate.Payload.Type;

            JToken certificateJson = new JObject();
            certificateJson["domain"] = this._certificate.Domain;
            certificateJson["payload"] = payload;
            certificateJson["purpose"] = this._certificate.Purpose;
            certificateJson["signer"] = this._certificate.Signer.ToLower();
            certificateJson["timestamp"] = this._certificate.Timestamp;
            return certificateJson;
        }
    
    }
}