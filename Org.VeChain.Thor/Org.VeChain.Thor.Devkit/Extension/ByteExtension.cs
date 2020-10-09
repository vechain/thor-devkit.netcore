using System;

namespace Org.VeChain.Thor.Devkit.Extension
{
    public static class ByteExtension
    {
        public static string ToHexString(this byte[] data)
        {
            return "0x" + BitConverter.ToString(data).Replace("-","").ToLower();
        } 
    }
}