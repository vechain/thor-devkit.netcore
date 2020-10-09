using Nethereum.RLP;
using System.Numerics;
using System.Collections.Generic;
using System.Linq;

namespace Org.VeChain.Thor.Devkit.Rlp
{
    public static class RlpEncodeExtension
    {
        public static IRlpItem EncodeToRlpItem(this BigInteger value)
        {
            return new RlpItem(value.ToBytesForRLPEncoding());
        }

        public static IRlpItem EncodeToRlpItem(this bool value)
        {
            return new RlpItem(value?new byte[1]{0x01}:new byte[0]);
        }

        public static IRlpItem EncodeToRlpItem(this int value)
        {
            return new RlpItem(value.ToBytesForRLPEncoding());
        }

        public static IRlpItem EncodeToRlpItem(this long value)
        {
            return new RlpItem(value.ToBytesForRLPEncoding());
        }

        public static IRlpItem EncodeToRlpItem(this string value)
        {
            return new RlpItem(value.ToBytesForRLPEncoding());
        }

        public static IRlpItem EncodeToRlpItem(this byte[][] values)
        {
            return new RlpArray(new byte[0]);
        }

        public static IRlpItem EncodeToRlpItem(this string[] values)
        {
            return new RlpArray(new byte[0]);
        }
    }

    public static class RlpDecodeExtension
    {
        public static BigInteger DecodeToBigInteger(this RlpItem item)
        {
            if(item.RlpData != null && item.RlpData.Length != 0)
            {
                return item.RlpData.ToBigIntegerFromRLPDecoded();
            }

            return BigInteger.Zero;
        }

        public static bool DecodeToBoolean(this RlpItem item)
        {
            if(item.RlpData != null && item.RlpData.Length != 0)
            {
                return item.RlpData.SequenceEqual(new byte[]{01});
            }

            return false;
        }

        public static int DecodeToInt(this RlpItem item)
        {
            if(item.RlpData != null && item.RlpData.Length != 0)
            {
                return item.RlpData.ToIntFromRLPDecoded();
            }

            return 0;
        }
    
        public static long DecodeToLong(this RlpItem item)
        {
            if(item.RlpData != null && item.RlpData.Length != 0)
            {
                return item.RlpData.ToLongFromRLPDecoded();
            }

            return 0;
        }
    
        public static string DecodeToString(this RlpItem item)
        {
            if(item.RlpData != null && item.RlpData.Length != 0)
            {
                return item.RlpData.ToStringFromRLPDecoded();
            }

            return "";
        }
    
        public static byte[][] DecodeToList(this RlpArray array)
        {
            List<byte[]> datas = new List<byte[]>(array.Count);
            datas.AddRange(array.Select(item => item.RlpData));
            return datas.ToArray();
        }
    }
}