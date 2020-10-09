using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Nethereum.ABI.Model;
using System;
using Org.VeChain.Thor.Devkit.Extension;

namespace Org.VeChain.Thor.Devkit.Abi
{
    public class AbiFunctionBuiler
    {
        /// <summary>
        /// instantiation a function builder with abi json string
        /// </summary>
        /// <param name="abiString"></param>
        /// <returns></returns>
        public IAbiFunctionDefinition Builder(string abiString)
        {
            var abiJson = JsonConvert.DeserializeObject<JToken>(abiString);
            return this.Builder(abiJson);
        }

        /// <summary>
        /// instantiation a function builder with abi json object
        /// </summary>
        /// <param name="abiJson"></param>
        /// <returns></returns>
        public IAbiFunctionDefinition Builder(JToken abiJson)
        {
            AbiFunctionDefinition definition = new AbiFunctionDefinition();

            definition.Name = abiJson["name"].ToString();
            definition.Constant = (bool)abiJson["constant"];
            definition.Payable = (bool)abiJson["payable"];
            
            AbiStateMutability stateMutability;
            Enum.TryParse(abiJson["stateMutability"].ToString(),true,out stateMutability);
            definition.stateMutability = stateMutability;

            definition.inputs = (new AbiParameterBuilder()).Builder(abiJson["inputs"].ToString());
            definition.outputs = (new AbiParameterBuilder()).Builder(abiJson["outputs"].ToString());
            definition.Sha3Signature = GetNethFunctionABI(definition).Sha3Signature.ToBytes();

            return definition;
        }
    
        protected internal class AbiFunctionDefinition : IAbiFunctionDefinition
        {
            public string Type => "function";
            public string Name { get; protected internal set; }

            public bool Constant { get; protected internal set; }

            public bool Payable { get; protected internal set; }

            public AbiStateMutability stateMutability { get; protected internal set; }

            public IAbiParameterDefinition[] inputs { get; protected internal set; }

            public IAbiParameterDefinition[] outputs { get; protected internal set; }

            public byte[] Sha3Signature { get; protected internal set; }
        }
     
        private FunctionABI GetNethFunctionABI(IAbiFunctionDefinition definition)
        {
            var functionABI = new FunctionABI(definition.Name,definition.Constant);
            functionABI.InputParameters = this.GetNethParameters(definition.inputs);
            functionABI.OutputParameters = this.GetNethParameters(definition.outputs);
            return functionABI;
        }

        private Parameter[] GetNethParameters(IAbiParameterDefinition[] parames)
        {
            Parameter[] result = new Parameter[parames.Length];
            for(int index = 0;index < parames.Length;index++)
            {
                result[index] = new Parameter(parames[index].ABIType,parames[index].Name,index+1);
            }
            return result;
        }
    }
}