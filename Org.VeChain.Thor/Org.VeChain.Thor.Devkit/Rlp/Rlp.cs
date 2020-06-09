namespace Org.VeChain.Thor.Devkit.Rlp
{
    public class Rlp
    {
        public static byte[] Encode(IRlpKind kind,dynamic obj)
        {
            return new byte[0];
        }

        public static T Decode<T>(IRlpKind kind,byte[] data)
        {
            return default(T);
        }
    }
}