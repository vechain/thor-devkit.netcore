namespace Org.VeChain.Thor.Devkit.Abi
{
    public interface IAbiContractDefinition
    {
        /// <summary>
        /// return abi constructor definition interface
        /// </summary>
        /// <value></value>
        IAbiConstructorDefinition Constructor { get; }

        /// <summary>
        /// return abi function definitions interface array
        /// </summary>
        /// <value></value>
        IAbiFunctionDefinition[] Functions { get; }

        /// <summary>
        /// return abi event definitions interface array
        /// </summary>
        /// <value></value>
        IAbiEventDefinition[] Events { get; }
    }

    public interface IAbiConstructorDefinition
    {
        /// <summary>
        /// return abi type, IAbiConstructorDefinition allways return 'constructor'
        /// </summary>
        /// <value></value>
        string Type { get; }

        /// <summary>
        /// return this constructor need to pay gas
        /// </summary>
        /// <value></value>
        bool Payable { get; }

        /// <summary>
        /// return abi stateMutability 
        /// </summary>
        /// <value></value>
        AbiStateMutability stateMutability { get; }

        /// <summary>
        /// return abi inputs parameter array
        /// </summary>
        /// <value></value>
        IAbiParameterDefinition[] inputs { get; }
    }
}