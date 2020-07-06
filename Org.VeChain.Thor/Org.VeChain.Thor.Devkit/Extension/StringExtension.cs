using System.Text.RegularExpressions;
using System;
using System.Linq;

namespace Org.VeChain.Thor.Devkit.Extension
{
    public static class StringExtension
    {
        public static byte[] ToBytes(this string hex)
        {
            if(hex.Substring(0,2).ToLower() == "0x"){
                hex = hex.ToLower().Replace("0x",String.Empty);
            }

            if(hex.IsHexString()){
                return Enumerable.Range(0, hex.Length)
                     .Where(x => x % 2 == 0)
                     .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                     .ToArray();
            }
            else
            {
                throw new Exception("invalid hex string");
            }
        }

        public static bool IsHexString(this string str)
        {
            if(str.Length == 0)
            {
                return false;
            }

            if(str.Substring(0,2).ToLower() == "0x"){
                str = str.ToLower().Replace("0x",String.Empty);
            }

            Regex regex = new Regex(@"^[0-9a-f]+$");
            return regex.IsMatch(str) && str.Length % 2 == 0;
        }
    }
}