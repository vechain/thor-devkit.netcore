using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using Nethereum.ABI.JsonDeserialisation;
using Nethereum.ABI.Model;
using System;

namespace Org.VeChain.Thor.Devkit.Abi
{
    public class AbiParameterBuilder
    {
        public IAbiParameterDefinition[] Builder(string abiString)
        {
            var abiJson = JsonConvert.DeserializeObject<JArray>(abiString);
            return this.Builder(abiJson);
        }

        public IAbiParameterDefinition[] Builder(JArray abiJson)
        {
            List<IAbiParameterDefinition> parames = new List<IAbiParameterDefinition>();

            foreach(var item in abiJson)
            {
                parames.Add(new AbiParameterDefinition(item["name"].ToString(),item["type"].ToString()));
            }

            return parames.ToArray();
        }

        private class AbiParameterDefinition:IAbiParameterDefinition
        {
            public string Name { get; set; }
            public string ABIType { get; set; }

            public AbiParameterDefinition(string name,string type)
            {
                this.Name = name;
                this.ABIType = type;
            }
        }
    }
}
