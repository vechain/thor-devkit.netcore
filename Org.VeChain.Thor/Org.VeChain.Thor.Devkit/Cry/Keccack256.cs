using Nethereum.Util;
using Org.VeChain.Thor.Devkit.Extension;

namespace Org.VeChain.Thor.Devkit.Cry
{
    public class Keccack256
    {
        public static byte[] CalculateHash(byte[] data)
        {
            return (new Sha3Keccack()).CalculateHash(data);
        }

        public static byte[] CalculateHash(string data)
        {
            string hashString = (new Sha3Keccack()).CalculateHash(data);
            return hashString.ToBytes();
        }
    }
}