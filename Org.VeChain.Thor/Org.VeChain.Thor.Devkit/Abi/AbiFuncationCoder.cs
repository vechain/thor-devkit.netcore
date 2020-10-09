using System.IO;
using Org.VeChain.Thor.Devkit.Extension;
using System;

namespace Org.VeChain.Thor.Devkit.Abi
{
    public class AbiFuncationCoder
    {
        public AbiFuncationCoder(string abiJson)
        {
            this._definition = (new AbiFunctionBuiler()).Builder(abiJson);
        }

        public AbiFuncationCoder(IAbiFunctionDefinition definition)
        {
            this._definition = definition;
        }

        public byte[] Encode(params dynamic[] values)
        {
            MemoryStream stream = new MemoryStream();
            if(values.Length == this._definition.inputs.Length)
            {
                try
                {
                    AbiInputParameter[] parameters = new AbiInputParameter[values.Length];
                    for(int index = 0; index < values.Length; index++)
                    {
                        parameters[index] = new AbiInputParameter(this._definition.inputs[index],values[index]);
                    }
                    stream.Append(this._definition.Sha3Signature);
                    stream.Append(AbiParameterCoder.EncodeParames(parameters));
                }
                catch
                {
                    throw new ArgumentException("input values invalid");
                }
            }
            else
            {
                throw new ArgumentException("values number not match");
            }
            return stream.ToArray();
        }

        public AbiOutputParameter[] Decode(byte[] outputdata)
        {  
            return AbiParameterCoder.DecodeParames(this._definition.outputs,outputdata);
        }

        private IAbiFunctionDefinition _definition;
    }
}