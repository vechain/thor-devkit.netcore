using System;
using System.Collections.Generic;
using System.Data.HashFunction.Blake2;
using System.Text;
using Org.VeChain.Thor.Devkit.Extension;

namespace Org.VeChain.Thor.Devkit.Cry
{
    public class Blake
    {
        public static byte[] CalculateHash(byte[] data)
        {
            Blake2BConfig config = new Blake2BConfig();
            config.HashSizeInBits = 256;
            IBlake2B blake2B =  Blake2BFactory.Instance.Create(config);
            return blake2B.ComputeHash(data).Hash;
        }

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
