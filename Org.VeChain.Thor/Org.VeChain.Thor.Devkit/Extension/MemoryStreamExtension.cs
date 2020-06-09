using System.IO;

namespace Org.VeChain.Thor.Devkit.Extension
{
    public static class MemoryStreamExtension
    {
        public static void Append(this MemoryStream stream, byte value)
        {
            stream.Append(new[] { value });
        }

        public static void Append(this MemoryStream stream, byte[] values)
        {
            stream.Write(values, 0, values.Length);
        }
    }
}