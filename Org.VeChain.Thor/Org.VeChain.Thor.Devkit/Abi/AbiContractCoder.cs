using System;
using System.Collections.Generic;
using System.Linq;

namespace Org.VeChain.Thor.Devkit.Abi
{
    public class AbiContractCoder
    {
        public AbiContractCoder(string abiJson)
        {
            this._definition = (new AbiContractBuilder()).ContractBuilder(abiJson);
        }

        public AbiContractCoder(IAbiContractDefinition definition)
        {
            this._definition = definition;
        }

        public byte[] EncodeConstructorWithParameter(params dynamic[] values)
        {
            if(this._definition.Constructor == null)
            {
                throw new ArgumentException("Constructor havn’t parameter");
            }

            List<AbiInputParameter> parameters = new List<AbiInputParameter>();
            foreach (dynamic t in values)
            {
                IAbiParameterDefinition parameDefinition = this._definition.Constructor.inputs[0];
                parameters.Add(new AbiInputParameter(parameDefinition,t));
            }

            return AbiParameterCoder.EncodeParames(parameters.ToArray());
        }

        public AbiFuncationCoder GetFuncationCoder(string funcName){
            IAbiFunctionDefinition funcDefinition = this._definition.Functions.First(item => item.Name == funcName);
            if(funcDefinition == null){ throw new Exception("function not exists"); }
            return new AbiFuncationCoder(funcDefinition);
        }

        public AbiEventCoder GetEventCoder(string eventName)
        {
            IAbiEventDefinition eventDefinition = this._definition.Events.First(item => item.Name == eventName);
            if(eventDefinition == null){ throw new Exception("event not exists"); }
            return new AbiEventCoder(eventDefinition);
        }

        private readonly IAbiContractDefinition _definition;
    }
}