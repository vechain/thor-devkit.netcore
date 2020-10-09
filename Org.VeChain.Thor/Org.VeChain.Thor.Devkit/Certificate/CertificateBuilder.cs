using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Org.VeChain.Thor.Devkit.Extension;

namespace Org.VeChain.Thor.Devkit.Certificate
{
    public class CertificateBuilder
    {
        /// <summary>
        /// instantiation a certificate builder with abi json string
        /// </summary>
        /// <param name="certificateJson"></param>
        /// <returns></returns>
        public ICertificate Builder(string certificateJson)
        {
            var abiJson = JsonConvert.DeserializeObject<JToken>(certificateJson);
            return this.Builder(abiJson);
        }

        /// <summary>
        /// instantiation a certificate builder with abi json object
        /// </summary>
        /// <param name="certificateJson"></param>
        /// <returns></returns>
        public ICertificate Builder(JToken certificateJson)
        {
            Certificate certificate = new Certificate
            {
                Purpose = certificateJson["purpose"].ToString(),
                Domain = certificateJson["domain"].ToString(),
                Timestamp = (int) certificateJson["timestamp"],
                Signer = certificateJson["signer"].ToString().ToLower()
            };


            CertificatePayload payload = new CertificatePayload
            {
                Type = certificateJson["payload"]["type"].ToString(),
                Content = certificateJson["payload"]["content"].ToString()
            };

            certificate.Signature = certificateJson["signer"] != null ? certificateJson["signer"].ToString().ToBytes() : new byte[0];

            return null;
        }
    }

    public class Certificate:ICertificate
    {
        public string Purpose { get;set; }
        public ICertificatePayload Payload { get; set; }
        public string Domain { get;set; }
        public int Timestamp { get;set; }
        public string Signer { get;set; }
        public byte[] Signature { get;set; }
    }

    public class CertificatePayload:ICertificatePayload
    {
        public string Type { get; set; }
        public string Content { get; set; }
    }
}