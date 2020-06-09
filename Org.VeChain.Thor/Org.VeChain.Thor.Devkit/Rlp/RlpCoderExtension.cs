using Nethereum.RLP;
using System.Numerics;
using System.Collections.Generic;
using System;

namespace Org.VeChain.Thor.Devkit.Rlp
{
    public static class RlpEncodeExtension
    {
        public static IRlpItem EncodeToRlpItem(this BigInteger value)
        {
            return new RlpItem(value.ToBytesForRLPEncoding());
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
                return (item as RlpItem).RlpData.ToBigIntegerFromRLPDecoded();
            }
            else
            {
                return BigInteger.Zero;
            }
        }

        public static int DecodeToInt(this RlpItem item)
        {
            if(item.RlpData != null && item.RlpData.Length != 0)
            {
                return (item as RlpItem).RlpData.ToIntFromRLPDecoded();
            }
            else
            {
                return 0;
            }
        }
    
        public static long DecodeToLong(this RlpItem item)
        {
            if(item.RlpData != null && item.RlpData.Length != 0)
            {
                return (item as RlpItem).RlpData.ToLongFromRLPDecoded();
            }
            else
            {
                return 0;
            }
        }
    
        public static string DecodeToString(this RlpItem item)
        {
            if(item.RlpData != null && item.RlpData.Length != 0)
            {
                return (item as RlpItem).RlpData.ToStringFromRLPDecoded();
            }
            else
            {
                return "";
            }
        }
    
        public static byte[][] DecodeToList(this RlpArray array)
        {
            List<byte[]> datas = new List<byte[]>(array.Count);
            foreach(IRlpItem item in array)
            {
                datas.Add(item.RlpData);
            }
            return datas.ToArray();
        }
    }
}