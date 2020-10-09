using Org.VeChain.Thor.Devkit.Rlp;
using System.Numerics;
using System;
using Org.VeChain.Thor.Devkit.Cry;
using Org.VeChain.Thor.Devkit.Extension;
using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace Org.VeChain.Thor.Devkit.Transaction
{
    public class Body
    {
        public int ChainTag { get; set; }
        public string BlockRef;
        public int Expiration;
        public List<Clause> Clauses;
        public int GasPriceCoef;
        public long Gas;
        public string DependsOn;
        public string Nonce;
        public Reserved Reserved;

        public Body()
        {
            this.Clauses = new List<Clause>();
        }

        public static IRlpKind RlpDefinition()
        {
            var rlpKind = new RlpStructKind();
            rlpKind.Properties.Add(new RlpIntKind("ChainTag"));
            rlpKind.Properties.Add(new RlpHexStringKind("BlockRef",false,8));
            rlpKind.Properties.Add(new RlpIntKind("Expiration"));
            rlpKind.Properties.Add(new RlpArrayKind(Clause.RlpDefinition(),"Clauses"));
            rlpKind.Properties.Add(new RlpIntKind("GasPriceCoef"));
            rlpKind.Properties.Add(new RlpLongKind("Gas",8));
            rlpKind.Properties.Add(new RlpHexStringKind("DependsOn",true,32));
            rlpKind.Properties.Add(new RlpHexStringKind("Nonce",false,8));
            rlpKind.Properties.Add(Reserved.RlpDefinition());
            return rlpKind;
        }
    }

    public class Clause
    {
        public string To;
        public BigInteger Value;
        public string Data;

        public Clause(){}

        public Clause(string to,BigInteger value,string data)
        {
            this.To = to;
            this.Value = value;
            this.Data = data;
        }

        public static IRlpKind RlpDefinition()
        {
            var rlpKind = new RlpStructKind();
            rlpKind.Properties.Add(new RlpHexStringKind("To",true,20));
            rlpKind.Properties.Add(new RlpBigIntegerKind("Value",false,32));
            rlpKind.Properties.Add(new RlpHexStringKind("Data"));
            return rlpKind;
        }
    }

    public class Reserved
    {
        public Reserved()
        {
            this.Features = 0;
            this.Unused = null;
        }
        public int Features;
        public List<byte[]> Unused;

        public static IRlpKind RlpDefinition()
        {
            return new ReservedKind("Reserved",true);
        }

        public class ReservedKind:IRlpCustomKind
        {
            public string Name { get; set; }
            public bool Nullable { get; set; }

            public ReservedKind(bool nullable):this("",nullable){}
            public ReservedKind(string name,bool nullable)
            {
                this.Nullable = nullable;
                this.Name = name;
            }

            public IRlpItem EncodeToRlp(dynamic obj)
            {
                if(this.Nullable && obj == null)
                {
                    return null;
                }

                if(obj is Reserved reserved)
                {
                    RlpArray result = new RlpArray
                    {
                        new RlpIntKind("", 4, false).EncodeToRlp(reserved.Features)
                    };
                    if(reserved.Unused != null)
                    {
                        foreach(byte[] item in reserved.Unused)
                        {
                            result.Add((new RlpBytesKind(true).EncodeToRlp(item)));
                        }
                    }
                    return result;
                }

                throw new ArgumentException("value type invalid");
            }

            public dynamic DecodeFromRlp(IRlpItem rlp,Type type)
            {
                if (this.Nullable && (rlp == null || rlp.RlpData.Length == 0))
                {
                    return null;
                }

                RlpArray rlpArray = new RlpArray();

                if (rlp.RlpType == RlpType.Array)
                {
                    rlpArray = rlp as RlpArray;
                }
                else
                {
                    rlpArray = new RlpArray().Decode(rlp.RlpData) as RlpArray;
                }

                if(rlpArray != null && rlpArray.Count != 0)
                {
                    if(rlpArray[^1].RlpData.Length == 0)
                    {
                        throw new ArgumentException("invalid reserved fields: not trimmed");
                    }

                    Reserved reserved = new Reserved
                    {
                        Features = new RlpIntKind().DecodeFromRlp(rlpArray[0])
                    };
                    if(rlpArray.Count >1)
                    {
                        reserved.Unused = new List<byte[]>();
                        for(int index = 1; index < rlpArray.Count; index++)
                        {
                            reserved.Unused.Add(new RlpBytesKind(true).DecodeFromRlp(rlpArray[index]));                            
                        }
                    }
                    return reserved;
                }
                else
                {
                    return null;
                }
            }
        
            public IRlpItem EncodeWithJson(JToken item)
            {
                if(this.Nullable && (item.Type == JTokenType.Null))
                {
                    return new RlpArray();
                }

                Reserved reserved = new Reserved
                {
                    Features = (item["Features"] as JValue).ToObject<int>()
                };

                if(item["Unused"] != null && item["Unused"].Type != JTokenType.Null)
                {
                    if(item["Unused"] is JArray)
                    {
                        if((item["Unused"] as JArray).Count > 0)
                        {
                            reserved.Unused = new List<byte[]>();
                            foreach(var data in item["Unused"] as JArray)
                            {
                                string jValue = (data as JValue).Value as string;
                                if(jValue.Length != 0)
                                {
                                    if(jValue.IsHexString())
                                    {
                                        reserved.Unused.Add(jValue.ToBytes());
                                    }
                                    else
                                    {
                                        throw new ArgumentException("unused value is not hexstring");
                                    }
                                }
                                else
                                {
                                    throw new ArgumentException("invalid reserved fields: not trimmed");
                                }
                            }
                        }
                    }
                    else
                    {
                        throw new ArgumentException("invalid item type");
                    }
                }
                return this.EncodeToRlp(reserved);
            }
        
            public JToken DecodeToJson(IRlpItem rlp)
            {
                Reserved reserved = this.DecodeFromRlp(rlp,typeof(Reserved));
                if(reserved == null)
                {
                    return null;
                }
                string jsonstr = JsonConvert.SerializeObject(reserved,new JsonBytesConverter());
                return JObject.Parse(jsonstr);
            }
        }
    }

    public class Transaction
    {
        public static readonly int DELEGATED_MASK = 1;
        public byte[] Signature { get; set; }
        public string Id => this.CalculateID();
        public Body Body { get; protected internal set; }
        public string Origin => this.GetOrigin();

        public string Delegator => this.GetDelegator();

        public bool IsDelegated => this.GetDelegated();

        public long IntrinsicGas()
        {
            const long txGas = 5000;
            const long clauseGas = 16000;
            const long clauseGasContractCreation = 48000;

            if(this.Body.Clauses.Count == 0)
            {
                return txGas + clauseGas;
            }

            long sumGas = txGas;

            foreach(var clause in this.Body.Clauses)
            {
                if(clause.To.Length != 0){
                    sumGas += clauseGas;
                }else
                {
                    sumGas += clauseGasContractCreation;
                }
            }

            return sumGas;
        }

        public Transaction(Body body)
        {
            this.Body = body;
        }

        protected internal Transaction(){}

        public byte[] Encode()
        {   
            this.TrimReserved();
            if(this.Signature != null && this.Signature.Length != 0)
            {
                var transaction = new {
                    ChainTag = this.Body.ChainTag,
                    BlockRef = this.Body.BlockRef,
                    Expiration = this.Body.Expiration,
                    Clauses = this.Body.Clauses,
                    GasPriceCoef = this.Body.GasPriceCoef,
                    Gas = this.Body.Gas,
                    DependsOn = this.Body.DependsOn,
                    Nonce = this.Body.Nonce,
                    Reserved = this.Body.Reserved,
                    Signature = this.Signature
                };
                return RlpCode.Encode(Transaction.SignedRlpDefinition(),transaction);
            }
            else
            {
                return RlpCode.Encode(Transaction.UnsignedRlpDefinition(),this.Body);
            }
        }

        public static Transaction Decode(byte[] raw,bool unsigned = true)
        {
            if(unsigned)
            {
                return RlpCode.Decode(Transaction.UnsignedRlpDefinition(),raw,typeof(Transaction));
            }

            return RlpCode.Decode(Transaction.SignedRlpDefinition(),raw,typeof(Transaction));
        }
    
        public static IRlpKind UnsignedRlpDefinition()
        {
            return Body.RlpDefinition();
        }

        public static IRlpKind SignedRlpDefinition()
        {
            var rlpKind = new RlpStructKind();
            rlpKind.Properties.AddRange((Body.RlpDefinition() as RlpStructKind).Properties);
            rlpKind.Properties.Add(new RlpIntKind("Signature"));
            return rlpKind;
        }
    
        public byte[] SigningHash(string delegateFor = "")
        {
            byte[] encode = RlpCode.Encode(Transaction.UnsignedRlpDefinition(),this.Body);
            byte[] hash = Blake2b.CalculateHash(encode);

            if(delegateFor.Length != 0)
            {
                if(SimpleWallet.IsValidAddress(delegateFor))
                {
                    MemoryStream stream = new MemoryStream();
                    stream.Append(hash);
                    stream.Append(delegateFor.ToBytes());
                    return Blake2b.CalculateHash(stream.ToArray());
                }

                throw new ArgumentException("delegateFor expected address");
            }
            return hash;
        }

        public string CalculateID()
        {
            if(!this.GetSignatureValid()){return "";}
            try
            {
                string origin = GetOrigin();
                return this.CalculateIDWithUnsigned(origin);
            }
            catch
            {
                return "";
            }
        }

        public string CalculateIDWithUnsigned(string origin)
        {
            if(!SimpleWallet.IsValidAddress(origin))
            {
                throw new ArgumentException("origin expected address");
            }
            byte[] signingHash = this.SigningHash();
            MemoryStream stream = new MemoryStream();
            stream.Append(signingHash);
            stream.Append(origin.ToBytes());
            return Blake2b.CalculateHash(stream.ToArray()).ToHexString();
        }

        public void AddVIP191Signature(byte[] senderSignature,byte[] gasPayerSignature)
        {
            if(senderSignature == null || senderSignature.Length != 65)
            {
                throw new ArgumentException("SenderSignature invalid");
            }

            if(gasPayerSignature == null || gasPayerSignature.Length != 65)
            {
                throw new ArgumentException("GasPayerSignature invalid");
            }

            MemoryStream stream = new MemoryStream();
            stream.Append(senderSignature);
            stream.Append(gasPayerSignature);
            this.Signature = stream.ToArray();
        }

        private void TrimReserved()
        {
            if(Body.Reserved?.Unused != null)
            {
                while(this.Body.Reserved.Unused.Count > 0)
                {
                    if(this.Body.Reserved.Unused[^1].Length == 0)
                    {
                        this.Body.Reserved.Unused.RemoveAt(this.Body.Reserved.Unused.Count - 1);
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }
    
        private bool GetDelegated()
        {
            return this.Body.Reserved != null && (this.Body.Reserved.Features & DELEGATED_MASK) == DELEGATED_MASK;
        }
    
        private bool GetSignatureValid()
        {
            int expectedSigLen = this.GetDelegated() ? 65 * 2 : 65;
            return this.Signature != null && this.Signature.Length == expectedSigLen;
        }
    
        private string GetOrigin()
        {
            if(!this.GetSignatureValid()){return "";}
            try
            {
                byte[] signingHash = this.SigningHash();
                byte[] originSigh = new byte[65];
                Array.Copy(this.Signature,0,originSigh,0,65);
                byte[] pubKey = Secp256k1.RecoverPublickey(signingHash,originSigh);
                return SimpleWallet.PublicKeyToAddress(pubKey);
            }
            catch
            {
                return "";
            }
        }
    
        private string GetDelegator()
        {
            if(!this.GetDelegated())
            {
                return "";
            }

            if(!this.GetSignatureValid())
            {
                return "";
            }

            try
            {
                byte[] signingHash = this.SigningHash(this.Origin);
                byte[] delegatorSignature = new byte[65];
                Array.Copy(this.Signature,65,delegatorSignature,0,65);
                byte[] delegatorpubKey = Secp256k1.RecoverPublickey(signingHash,delegatorSignature);
                return SimpleWallet.PublicKeyToAddress(delegatorpubKey);
            }
            catch
            {
                return "";
            }
        }
    }
}