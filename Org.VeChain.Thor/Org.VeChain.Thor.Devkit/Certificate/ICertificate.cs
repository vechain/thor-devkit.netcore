namespace Org.VeChain.Thor.Devkit.Certificate
{
    public interface ICertificate
    {
        string Purpose { get; set; }
        ICertificatePayload Payload { get; set; }
        string Domain { get; set; }
        int Timestamp { get; set; }
        string Signer { get; set; }
        byte[] Signature { get; set; }
    }

    public interface ICertificatePayload
    {
        string Type { get; set; }
        string Content { get; set; }
    }
}