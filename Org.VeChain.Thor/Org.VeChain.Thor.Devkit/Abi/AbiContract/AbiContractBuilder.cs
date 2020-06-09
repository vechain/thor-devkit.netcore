using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Linq;
using System;
using System.Collections.Generic;

namespace Org.VeChain.Thor.Devkit.Abi
{
    public class AbiContractBuilder
    {
        public IAbiContractDefinition ContractBuilder(string abiString)
        {
            var abiJson = JsonConvert.DeserializeObject<JArray>(abiString);
            return this.ContractBuilder(abiJson);
        }

        public IAbiContractDefinition ContractBuilder(JArray abiJson)
        {
            AbiContractDefinition definition = new AbiContractDefinition();

            JToken constructorJson = abiJson.Where(item => item["type"].ToString() == "constructor").First();
            if(constructorJson != null)
            {
                definition.Constructor = this.ConstructorBuilder(constructorJson);
            }

            IEnumerable<JToken> funcitons = abiJson.Where(item => item["type"].ToString() == "function");
            List<IAbiFunctionDefinition> funcitonList = new List<IAbiFunctionDefinition>();
            foreach(JToken item in funcitons)
            {
                funcitonList.Add(new AbiFunctionBuiler().Builder(item));
            }
            definition.Functions = funcitonList.ToArray();

            IEnumerable<JToken> events = abiJson.Where(item => item["type"].ToString() == "event");
            List<IAbiEventDefinition> eventList = new List<IAbiEventDefinition>();
            foreach (JToken item in events)
            {
                eventList.Add(new AbiEventBuilder().Builder(item));
            }
            definition.Events = eventList.ToArray();

            return definition;
        }

        protected internal class AbiContractDefinition:IAbiContractDefinition
        {
            public IAbiConstructorDefinition Constructor { get; protected internal set; }
            public IAbiFunctionDefinition[] Functions { get; protected internal set; }
            public IAbiEventDefinition[] Events { get; protected internal set; }
        }

        protected internal class AbiConstructorDefinition:IAbiConstructorDefinition
        {
            public string Type { get; protected internal set; }
            public bool Payable { get; protected internal set; }
            public AbiStateMutability stateMutability { get; protected internal set; }
            public IAbiParameterDefinition[] inputs { get; protected internal set; }
        }

        private IAbiConstructorDefinition ConstructorBuilder(JToken abiJson)
        {
            AbiConstructorDefinition definition = new AbiConstructorDefinition();
            definition.Payable = (bool)abiJson["payable"];
            
            AbiStateMutability stateMutability;
            Enum.TryParse<AbiStateMutability>(abiJson["stateMutability"].ToString(),true,out stateMutability);
            definition.stateMutability = stateMutability;
            
            definition.Type = "constructor";
            definition.inputs = (new AbiParameterBuilder()).Builder(abiJson["inputs"].ToString());
            
            return definition;
        }
    }
}