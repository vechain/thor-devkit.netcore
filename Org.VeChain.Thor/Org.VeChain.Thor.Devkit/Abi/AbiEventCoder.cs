using System;
using System.Collections.Generic;
using Org.VeChain.Thor.Devkit.Cry;
using Org.VeChain.Thor.Devkit.Extension;
using Nethereum.ABI.Model;
using Nethereum.ABI.FunctionEncoding;
using System.Linq;

namespace Org.VeChain.Thor.Devkit.Abi
{
    public class AbiEventCoder
    {
        public AbiEventCoder(string abiJson)
        {
            this._definition = (new AbiEventBuilder()).Builder(abiJson);
        }

        public AbiEventCoder(IAbiEventDefinition definition)
        {
            this._definition = definition;
        }

        public byte[][] EncodeFilter(Dictionary<string,dynamic> indexed)
        {
            if(this._definition.inputs.IndexedCount() >= indexed.Count)
            {
                if(this._definition.Anonymous && indexed.Count <= 4 || !this._definition.Anonymous && indexed.Count <= 3)
                {
                    return this.Encode(indexed);
                }

                throw new ArgumentException("invalid topics count");
            }

            throw new ArgumentException("invalid topics count");
        }

        public AbiEventTopic[] DecodeTopics(byte[][] topics,byte[] data)
        {
            List<AbiEventTopic> result = new List<AbiEventTopic>();
            EventABI nethEventAbi = new EventABI(this._definition.Name, this._definition.Anonymous)
            {
                InputParameters = AbiEventBuilder.GetNethParameters(this._definition.inputs)
            };
            List<string> topicsStr = new List<string>();
            foreach(byte[] topic in topics)
            {
                topicsStr.Add(topic.ToHexString());
            }
            List<ParameterOutput> nethTopics = (new EventTopicDecoder(this._definition.Anonymous)).DecodeDefaultTopics(nethEventAbi,topicsStr.ToArray(),data.ToHexString());

            foreach(ParameterOutput output in nethTopics.OrderBy(item => item.Parameter.Order))
            {
                IAbiEventInputDefinition definition = this._definition.inputs.First(item => item.Name.Equals(output.Parameter.Name));
                result.Add(new AbiEventTopic(definition,output.Result));
            }

            return result.ToArray();
        }

        private byte[][] Encode(Dictionary<string,dynamic> args)
        {
            List<byte[]> topics = new List<byte[]>();
            if(!this._definition.Anonymous){
                topics.Add(this._definition.Sha3Signature);
            }

            foreach(var input in this._definition.inputs)
            {
                if(!input.Indexed){ continue; }
                if(args.ContainsKey(input.Name) && args[input.Name] == null)
                {
                    topics.Add(null);
                }
                else
                {
                    var arg = args[input.Name];
                    if(IsDynamicType(input.ABIType))
                    {
                        if(input.ABIType == "string")
                        {
                            byte[] topic = Keccack256.CalculateHash(arg.ToString());
                            topics.Add(topic);
                        }
                        else
                        {
                            if(arg is string stringArgs && stringArgs.IsHexString())
                            {
                                byte[] topic = Keccack256.CalculateHash(arg);
                                topics.Add(topic);
                            }
                            else
                            {
                                throw new ArgumentException($"invalid {input.ABIType} value");
                            }
                        }
                    }
                    else
                    {
                         byte[] topic = AbiParameterCoder.EncodeParame(new AbiInputParameter(input,arg));
                         topics.Add(topic);
                    }
                }
            }
            return topics.ToArray();
        }

        private static bool IsDynamicType(string abiType)
        {
            return abiType == "bytes" || abiType == "string" || abiType.EndsWith("[]");
        }

        private readonly IAbiEventDefinition _definition;

        private Parameter[] GetNethParameters(IAbiEventInputDefinition[] definitions)
        {
            Parameter[] result = new Parameter[definitions.Length];
            for(int index = 0;index < definitions.Length;index++)
            {
                Parameter nethParame = new Parameter(definitions[index].ABIType, definitions[index].Name, index + 1)
                {
                    Indexed = definitions[index].Indexed
                };
                result[index] = nethParame;
            }
            return result;
        }
    }
}