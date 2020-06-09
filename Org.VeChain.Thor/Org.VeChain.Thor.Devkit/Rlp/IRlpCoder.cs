namespace Org.VeChain.Thor.Devkit.Rlp
{
    public interface IRlpCoder<T>
    {
        byte[] Encode(T value);

        T Decode(byte[] rlpData);
    }
}