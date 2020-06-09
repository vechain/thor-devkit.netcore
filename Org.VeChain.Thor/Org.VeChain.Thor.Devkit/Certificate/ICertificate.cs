namespace Org.VeChain.Thor.Devkit.Certificate
{
    public interface ICertificate
    {
        string Purpose { get; }
        ICertificatePayload Payload { get; }
        string Domain { get; }
        int Timestamp { get; }
        string Signer { get; }
        byte[] Signature { get;set; }
    }

    public interface ICertificatePayload
    {
        string Type { get; }
        string Content { get; }
    }
}