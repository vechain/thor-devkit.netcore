using System;
using Nethereum.ABI;

namespace Org.VeChain.Thor.Devkit.Abi
{

    public class AbiOutputParameter
    {
        
        public AbiOutputParameter(string abitype,string name = "")
        {
            this._nethABIType = ABIType.CreateABIType(abitype);
            this.Definition = new AbiOutputParameterDefinition(name,abitype);
        }

        public AbiOutputParameter(IAbiParameterDefinition definition)
        {
            this._nethABIType = ABIType.CreateABIType(definition.ABIType);
            this.Definition = definition;
        }

        public IAbiParameterDefinition Definition;

        public dynamic Result 
        {
            get{ return this._result; }
            protected internal set
            {
                this._result = value;
            }
        }

        private dynamic _result;
        private Nethereum.ABI.ABIType _nethABIType;

        public class AbiOutputParameterDefinition : IAbiParameterDefinition
        {
             public string Name { get; set;}
             public string ABIType { get; set;}
             public int Index { get; set; }

             public AbiOutputParameterDefinition(string name,string abitype,int index = 0)
             {
                 this.Name = name;
                 this.ABIType = abitype;
                 this.Index = index;
             }
        }
    }
}
