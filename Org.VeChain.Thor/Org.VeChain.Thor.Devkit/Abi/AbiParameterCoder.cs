using System;
using System.Collections.Generic;
using Nethereum.ABI.FunctionEncoding;
using Nethereum.ABI.Model;

namespace Org.VeChain.Thor.Devkit.Abi
{
    public class AbiParameterCoder
    {
        public static byte[] EncodeParame(AbiInputParameter parameter)
        {
            return AbiParameterCoder.Encode(new[] {parameter});
        }

        public static byte[] EncodeParames(AbiInputParameter[] parameters)
        {
            return AbiParameterCoder.Encode(parameters);
        }

        public static AbiOutputParameter DecodeParame(IAbiParameterDefinition definition,byte[] data)
        {
            AbiOutputParameter[] outputs = AbiParameterCoder.Decode(new[]{definition},data);
            return outputs.Length > 0 ? outputs[0] : null;
        }

        public static AbiOutputParameter[] DecodeParames(IAbiParameterDefinition[] parameters,byte[] data)
        {
            return AbiParameterCoder.Decode(parameters,data);
        }

        private static byte[] Encode(AbiInputParameter[] parameters)
        {
            List<Parameter> netherParames = new List<Parameter>();
            List<dynamic> values = new List<dynamic>();

            foreach(AbiInputParameter parame in parameters)
            {
                netherParames.Add(new Parameter(parame.Definition.ABIType,parame.Definition.Name));
                values.Add(parame.Value);
            }

            try
            {
                ParametersEncoder encoder = new ParametersEncoder();
                return encoder.EncodeParameters(netherParames.ToArray(),values.ToArray());
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        private static AbiOutputParameter[] Decode(IAbiParameterDefinition[] parameters,byte[] data)
        {
            List<AbiOutputParameter> result = new List<AbiOutputParameter>();
            List<Parameter> netherParames = new List<Parameter>();
            foreach(IAbiParameterDefinition parame in parameters)
            {
                netherParames.Add(new Parameter(parame.ABIType,parame.Name));
            }

            List<ParameterOutput> outputs = (new ParameterDecoder()).DecodeDefaultData(data,netherParames.ToArray());
            
            foreach(ParameterOutput output in outputs){
                try
                {
                    AbiOutputParameter abiOutput = new AbiOutputParameter(output.Parameter.Type,output.Parameter.Name);
                    abiOutput.Result = output.Result;
                    result.Add(abiOutput);
                }
                catch(Exception ex)
                {
                    throw ex;
                }
            }
            return result.ToArray();
        }
    }
}