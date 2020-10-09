namespace Org.VeChain.Thor.Devkit.Abi
{
    public interface IAbiFunctionDefinition
    {
        /// <summary>
        /// return abi type, IAbiFunctionDefinition allways return 'function'
        /// </summary>
        /// <value></value>
        string Type { get; }

        /// <summary>
        /// return abi function name
        /// </summary>
        /// <value></value>
        string Name { get; }

        /// <summary>
        /// return this function is constant
        /// </summary>
        /// <value></value>
        bool Constant { get; }

        /// <summary>
        /// return this function need to pay gas
        /// </summary>
        /// <value></value>
        bool Payable { get; }

        /// <summary>
        /// return stateMutability type
        /// </summary>
        /// <value></value>
        AbiStateMutability stateMutability { get; }

        /// <summary>
        /// return this function input parame array
        /// </summary>
        /// <value></value>
        IAbiParameterDefinition[] inputs { get; }

        /// <summary>
        /// return this function output parame array, the name of there parames are not required
        /// </summary>
        /// <value></value>
        IAbiParameterDefinition[] outputs { get; }

        byte[] Sha3Signature { get; }
    }
}