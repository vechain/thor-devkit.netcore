using System.Data.HashFunction.Blake2;
using System.Text;

namespace Org.VeChain.Thor.Devkit.Cry
{
    public class Blake2b
    {
        /// <summary>
        /// return hash
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static byte[] CalculateHash(byte[] data)
        {
            Blake2BConfig config = new Blake2BConfig();
            config.HashSizeInBits = 256;
            IBlake2B blake2B =  Blake2BFactory.Instance.Create(config);
            return blake2B.ComputeHash(data).Hash;
        }

        /// <summary>
        /// use UTF8 encode data and calculate hash
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static byte[] CalculateHash(string data)
        {
            Blake2BConfig config = new Blake2BConfig();
            config.HashSizeInBits = 256;
            IBlake2B blake2B = Blake2BFactory.Instance.Create(config);

            byte[] bytes = Encoding.UTF8.GetBytes(data);
            return blake2B.ComputeHash(bytes).Hash;
        }
    }
}
