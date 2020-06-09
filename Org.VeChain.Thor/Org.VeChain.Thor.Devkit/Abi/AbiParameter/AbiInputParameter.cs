using System.Numerics;
using System.Collections.Generic;
using System;
using Nethereum.ABI;

namespace Org.VeChain.Thor.Devkit.Abi
{

    public class AbiInputParameter
    {
        public AbiInputParameter(string abitype,dynamic value,string name = "")
        {
            this._nethABIType = ABIType.CreateABIType(abitype);
            this.Definition = new AbiInputParameterDefinition(this._nethABIType.Name,name);
            this.Value = value;
        }

        public AbiInputParameter(IAbiParameterDefinition definition,dynamic value)
        {
            this._nethABIType = ABIType.CreateABIType(definition.ABIType);
            this.Definition = definition;
            this.Value = value;
        }

        public IAbiParameterDefinition Definition;

        public dynamic Value 
        {
            get{ return this._value; }
            set
            {
                this._value = value;
            }
        }

        private dynamic _value;
        private Nethereum.ABI.ABIType _nethABIType;

        public class AbiInputParameterDefinition:IAbiParameterDefinition
        {
            public string Name { get; set;}
            public string ABIType { get; set;}

            public AbiInputParameterDefinition(string name,string abitype)
            {
                this.Name = name;
                this.ABIType = abitype;
            }
        }
    }
}