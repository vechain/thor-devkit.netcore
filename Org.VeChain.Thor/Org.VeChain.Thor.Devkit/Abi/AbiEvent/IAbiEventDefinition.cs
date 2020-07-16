using System.Linq;

namespace Org.VeChain.Thor.Devkit.Abi
{
    public interface IAbiEventDefinition
    {
        /// <summary>
        /// retrun abi anonymous
        /// </summary>
        /// <value></value>
        bool Anonymous { get; }

        /// <summary>
        /// return abi event name
        /// </summary>
        /// <value></value>
        string Name { get; }

        /// <summary>
        /// return abi type, IAbiEventDefinition allways return 'event'
        /// </summary>
        /// <value></value>
        string Type { get; }

        /// <summary>
        /// return abi inputs parameter array
        /// </summary>
        /// <value></value>
        IAbiEventInputDefinition[] inputs { get; }

        /// <summary>
        /// return this event sha3 signature value
        /// </summary>
        /// <value></value>
        byte[] Sha3Signature { get; }
    }

    public interface IAbiEventInputDefinition:IAbiParameterDefinition
    {
        /// <summary>
        /// return event parame is indexed
        /// </summary>
        /// <value></value>
        bool Indexed { get; }
    }

    public static class AbiEventInputDefinitionExtension
    {
        /// <summary>
        /// return indexed parame count
        /// </summary>
        /// <param name="inputs"></param>
        /// <returns></returns>
        public static int IndexedCount(this IAbiEventInputDefinition[] inputs)
        {
            return inputs.Where(item => item.Indexed).Count();
        }
    }
}