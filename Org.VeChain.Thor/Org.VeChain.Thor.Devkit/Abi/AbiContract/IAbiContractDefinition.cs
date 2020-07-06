namespace Org.VeChain.Thor.Devkit.Abi
{
    public interface IAbiContractDefinition
    {
        IAbiConstructorDefinition Constructor { get; }
        IAbiFunctionDefinition[] Functions { get; }

        IAbiEventDefinition[] Events { get; }
    }

    public interface IAbiConstructorDefinition
    {
        string Type { get; }
        bool Payable { get; }
        AbiStateMutability stateMutability { get; }
        IAbiParameterDefinition[] inputs { get; }
    }
}