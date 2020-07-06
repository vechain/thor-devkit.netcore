using System.Collections.Generic;

namespace Org.VeChain.Thor.Devkit.Abi
{
    public interface IAbiFunctionDefinition
    {
        string Type { get; }
        string Name { get; }
        bool Constant { get; }
        bool Payable { get; }
        AbiStateMutability stateMutability { get; }
        IAbiParameterDefinition[] inputs { get; }
        IAbiParameterDefinition[] outputs { get; }

        byte[] Sha3Signature { get; }
    }
}