using Nethereum.KeyStore.Crypto;
using Org.VeChain.Thor.Devkit.Extension;
using System.Text.RegularExpressions;
using Nethereum.Util;


namespace Org.VeChain.Thor.Devkit.Cry
{
    public class SimpleWallet
    {
        public SimpleWallet(byte[] priKey)
        {
            this._privateKey = priKey;
            this._publicKey = Secp256k1.DerivePublicKey(this._privateKey);
            this.address = SimpleWallet.PublicKeyToAddress(this._publicKey);
        }

        public const string AddressEmptyAsHex = "0x0";
        public const string hexRegex = "^0x[0-9a-f]{40}$";

        public readonly string address;

        /// <summary>
        /// return address starts with '0x'.
        /// </summary>
        /// <param name="priKey"></param>
        /// <returns></returns>
        public static string PrivateKeyToAddress(byte[] priKey)
        {
            byte[] pubKey = Secp256k1.DerivePublicKey(priKey);
            return SimpleWallet.PublicKeyToAddress(pubKey);
        }

        /// <summary>
        /// return address starts with '0x'.
        /// </summary>
        /// <param name="publickey"></param>
        /// <returns></returns>
        public static string PublicKeyToAddress(byte[] publickey)
        {
            byte[] hash = (new KeyStoreCrypto()).CalculateKeccakHash(publickey.Slice(1, publickey.Length));
            byte[] address = hash.Slice(12, hash.Length);
            return address.ToHexString();
        }

        public static bool IsValidAddress(string address)
        {
            Regex regex = new Regex(hexRegex);
            return regex.IsMatch(address.ToLower());
        }

        public static string ToChecksumAddress(string address)
        {
            return new AddressUtil().ConvertToChecksumAddress(address);
        }

        public static bool IsChecksumAddress(string address)
        {
            return new AddressUtil().IsChecksumAddress(address); 
        }

        public byte[] Sign(byte[] msgHash)
        {
            return Secp256k1.Sign(msgHash,this._privateKey);
        }

        public static string RecoverAddress(byte[] msgHash,byte[] signature)
        {
            byte[] publickey = Secp256k1.RecoverPublickey(msgHash,signature);
            return SimpleWallet.PublicKeyToAddress(publickey);
        }

        private byte[] _privateKey;
        private byte[] _publicKey;

    }
}