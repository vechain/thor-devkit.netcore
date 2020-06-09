using System.Linq;

namespace Org.VeChain.Thor.Devkit.Abi
{
    public interface IAbiEventDefinition
    {
        bool Anonymous { get; }
        string Name { get; }

        string Type { get; }

        IAbiEventInputDefinition[] inputs { get; }

        byte[] Sha3Signature { get; }
    }

    public interface IAbiEventInputDefinition:IAbiParameterDefinition
    {
        bool Indexed { get; }
    }

    public static class AbiEventInputDefinitionExtension
    {
        public static int IndexedCount(this IAbiEventInputDefinition[] inputs)
        {
            return inputs.Where(item => item.Indexed).Count();
        }
    }
}