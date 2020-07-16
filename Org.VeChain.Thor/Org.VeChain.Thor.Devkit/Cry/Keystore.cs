using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Nethereum.KeyStore;

namespace Org.VeChain.Thor.Devkit.Cry
{
    public class Keystore
    {
        public string address { get; internal set; }
        public JObject crypto { get; internal set; }
        public string id { get; internal set; }
        public int version { get; internal set; }

        /// <summary>
        /// the keystore support version 3
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public static Keystore CreateFromJson(string json)
        {
            Keystore keystore = new Keystore();
            keystore = JsonConvert.DeserializeObject<Keystore>(json);
            return keystore;
        }

        public JObject keystoreConvertToJson()
        {
            JObject json = new JObject();
            json["address"] = this.address.ToLower().Replace("0x", "");
            json["crypto"] = this.crypto;
            json["id"] = this.id;
            json["version"] = this.version;
            return json;
        }

        public static string EncryptToJson(byte[] privateKey, string password)
        {
            byte[] publicKey = Secp256k1.DerivePublicKey(privateKey);
            string address = SimpleWallet.PublicKeyToAddress(publicKey);
            KeyStoreService keyStore = new KeyStoreService();
            string jsonString = keyStore.EncryptAndGenerateDefaultKeyStoreAsJson(password, privateKey, address);
            JObject json = JObject.Parse(jsonString);
            json["address"] = json["address"].ToString().Replace("0x","");
            return json.ToString();
        }

        public static Keystore EncryptToStruct(byte[] privateKey, string password)
        {
            Keystore keystore = null;
            string jsonString = Keystore.EncryptToJson(privateKey, password);
            JsonConvert.DeserializeObject<Keystore>(jsonString);
            return keystore;
        }

        public static byte[] DecryptFromJson(string json, string password)
        {
            byte[] privateKey = new byte[32];
            KeyStoreService keyStore = new KeyStoreService();
            privateKey = keyStore.DecryptKeyStoreFromJson(password, json);
            return privateKey;
        }

        public static byte[] DecryptFromJson(Keystore keystore, string password)
        {
            byte[] privateKey = new byte[32];
            KeyStoreService keyStore = new KeyStoreService();
            privateKey = keyStore.DecryptKeyStoreFromJson(password, keystore.keystoreConvertToJson().ToString());
            return privateKey;
        }
    }
}