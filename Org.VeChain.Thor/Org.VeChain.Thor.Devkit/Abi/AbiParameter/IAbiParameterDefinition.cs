namespace Org.VeChain.Thor.Devkit.Abi
{
    public interface IAbiParameterDefinition
    {
        /// <summary>
        /// abi parameter name
        /// </summary>
        /// <value></value>
        string Name { get; }

        /// <summary>
        /// abi parame type
        /// </summary>
        /// <value></value>
        string ABIType { get; }
    }
}