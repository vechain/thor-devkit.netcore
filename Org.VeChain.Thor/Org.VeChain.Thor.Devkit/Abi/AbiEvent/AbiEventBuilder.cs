using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Nethereum.ABI.Model;
using Org.VeChain.Thor.Devkit.Extension;

namespace Org.VeChain.Thor.Devkit.Abi
{
    public class AbiEventBuilder
    {
        /// <summary>
        /// instantiation a event builder with abi json string
        /// </summary>
        /// <param name="abiString"></param>
        /// <returns></returns>
        public IAbiEventDefinition Builder(string abiString)
        {
            var abiJson = JsonConvert.DeserializeObject<JToken>(abiString);
            return this.Builder(abiJson);
        }

        /// <summary>
        /// instantiation a contract builder with abi json object
        /// </summary>
        /// <param name="abiJson"></param>
        /// <returns></returns>
        public IAbiEventDefinition Builder(JToken abiJson)
        {
            AbiEventDefinition definition = new AbiEventDefinition();

            definition.Anonymous = (bool)abiJson["anonymous"];
            definition.Name = abiJson["name"].ToString();

            JArray inputs = JsonConvert.DeserializeObject<JArray>(abiJson["inputs"].ToString());
            if(inputs != null && inputs.Count > 0){
                definition.inputs = new IAbiEventInputDefinition[inputs.Count];
                for(int index = 0; index < inputs.Count; index++)
                {
                    definition.inputs[index] = new AbiEventInputDefinition(inputs[index]["name"].ToString(),inputs[index]["type"].ToString(),(bool)inputs[index]["indexed"]);
                }
            }
            definition.Sha3Signature = GetNethEventABI(definition).Sha3Signature.ToBytes();

            return definition;
        }

        private EventABI GetNethEventABI(IAbiEventDefinition definition)
        {
            EventABI eventABI = new EventABI(definition.Name,definition.Anonymous);
            eventABI.InputParameters = AbiEventBuilder.GetNethParameters(definition.inputs);
            return eventABI;
        }

        protected internal static Parameter[] GetNethParameters(IAbiEventInputDefinition[] parames)
        {
            Parameter[] result = new Parameter[parames.Length];
            for(int index = 0;index < parames.Length;index++)
            {
                Parameter parame = new Parameter(parames[index].ABIType,parames[index].Name,index+1);
                parame.Indexed = parames[index].Indexed;
                result[index] = parame;
            }
            return result;
        }

        protected internal class AbiEventDefinition:IAbiEventDefinition
        {
            public bool Anonymous { get; protected internal set; }
            public string Name { get; protected internal set; }

            public string Type 
            {
                get { return "event"; }
            }

            public IAbiEventInputDefinition[] inputs { get; protected internal set; }

            public byte[] Sha3Signature { get; protected internal set; }
        }

        protected internal class AbiEventInputDefinition:IAbiEventInputDefinition
        {
            public string Name { get; protected internal set; }
            public string ABIType { get; protected internal set; }
            public bool Indexed { get; protected internal set; }

            protected internal AbiEventInputDefinition(){}
            protected internal AbiEventInputDefinition(string name,string abiType,bool indexed)
            {
                this.Name = name;
                this.ABIType = abiType;
                this.Indexed = indexed;
            }
        }
    }
}