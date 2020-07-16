namespace Org.VeChain.Thor.Devkit.Certificate
{
    public interface ICertificate
    {
        /// <summary>
        /// Only identification or agreement can be select now. 
        /// </summary>
        /// <value></value>
        string Purpose { get; set; }
        ICertificatePayload Payload { get; set; }

        /// <summary>
        /// The hostname of the application, it is a similar concept of Party A in a contract.
        /// </summary>
        /// <value></value>
        string Domain { get; set; }

        /// <summary>
        /// Timestamp,in second, consistent with UNIX timestamp and block timestamp. 
        /// </summary>
        /// <value></value>
        int Timestamp { get; set; }

        /// <summary>
        /// User account which signed the certificate in the hexadecimal string, starts with '0x'.
        /// </summary>
        /// <value></value>
        string Signer { get; set; }

        /// <summary>
        /// Signature in hexadecimal string, starts with '0x'.
        /// </summary>
        /// <value></value>
        byte[] Signature { get; set; }
    }

    public interface ICertificatePayload
    {
        /// <summary>
        /// Specify the type of content, currently it only support text format, the type may be extended in the future.
        /// </summary>
        /// <value></value>
        string Type { get; set; }

        /// <summary>
        /// A message for user which is determine by type. When the type is "text", it should be text format.
        /// </summary>
        /// <value></value>
        string Content { get; set; }
    }
}