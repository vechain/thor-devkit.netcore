using Xunit;
using Xunit.Abstractions;
using Org.VeChain.Thor.Devkit.Rlp;
using Org.VeChain.Thor.Devkit.Extension;
using System.Numerics;
using System.Globalization;
using System.Linq;
using System.Collections.Generic;
using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Org.VeChain.Thor.Devkit.UnitTest
{

    public class RlpEncodeTest{

        public struct EncodeTestCase
        {
            public EncodeTestCase(dynamic input,byte[] output)
            {
                this.Input = input;
                this.Output = output;
            }
            public dynamic Input;
            public byte[] Output;
        }

        [Fact]
        public void RlpBooleanTest()
        {
            var cases = new EncodeTestCase[]{
                new EncodeTestCase(true,new byte[1]{0x01}),
                new EncodeTestCase(false,new byte[1]{0x80}),
            };

            foreach(var item in cases)
            {
                byte[] encode = (new RLpBooleanKind()).EncodeToRlp(item.Input).RlpData;
                if(!encode.SequenceEqual(item.Output))
                {
                    Assert.True(false,string.Format("Rlp faild,input:{0}output:{1}",item.Input,item.Output));
                    break;
                }
            }
            Assert.True(true);
        }
    }


    public class RlpScalarTest
    {
        [Fact]
        public void RlpBigIntegerTest()
        {
            BigInteger data = BigInteger.Parse("3b102ea6c3a3566d8af",NumberStyles.AllowHexSpecifier);
            byte[] encode = (new RlpBigIntegerKind()).EncodeToRlp(data).Encode();
            RlpItem item = new RlpItem().Decode(encode) as RlpItem;
            BigInteger decode = (new RlpBigIntegerKind()).DecodeFromRlp(item);
            Assert.True(data.Equals(decode));

            byte[] zero = new byte[1]{0x80};
            BigInteger dataZero = BigInteger.Zero;
            byte[] encodeZero = (new RlpBigIntegerKind()).EncodeToRlp(dataZero).Encode();
            
            Assert.True(encodeZero.SequenceEqual(zero));
        }

        [Fact]
        public void RlpBooleanTest()
        {
            byte[] trueValue = new byte[1]{0x01};
            byte[] falseValue = new byte[1]{0x80};

            byte[] encode = (new RLpBooleanKind()).EncodeToRlp(true).Encode();
            Assert.True(encode.SequenceEqual(trueValue));

            RlpItem item = new RlpItem().Decode(falseValue) as RlpItem;
            bool decode = (new RLpBooleanKind()).DecodeFromRlp(item);
            Assert.True(!decode);
        }

        [Fact]
        public void RlpIntTest()
        {
            int data = 128;
            byte[] encode = (new RlpIntKind()).EncodeToRlp(data).Encode();
            Assert.True(encode.SequenceEqual(new byte[]{0x81,0x80}));
            RlpItem item = new RlpItem().Decode(encode) as RlpItem;
            int decode = (new RlpIntKind()).DecodeFromRlp(item);
            Assert.True(data.Equals(decode));
        }

        [Fact]
        public void RlpLongTest()
        {
            long data = 100000000000;
            byte[] encode = (new RlpLongKind()).EncodeToRlp(data).Encode();
            RlpItem item = new RlpItem().Decode(encode) as RlpItem;
            long decode = (new RlpLongKind()).DecodeFromRlp(item);
            Assert.True(data.Equals(decode));
        }

        [Fact]
        public void RlpStringTest()
        {
            string data = "hello";
            byte[] encode = (new RlpStringKind()).EncodeToRlp(data).Encode();
            RlpItem item = new RlpItem().Decode(encode) as RlpItem;
            string decode = (new RlpStringKind()).DecodeFromRlp(item);
            Assert.True(data.Equals(decode));
        }
    }

    public class RlpArrayTest
    {
        [Fact]
        public void RlpScalarArrayOfNumberTest(){
            int[] data = new int[]{0,1,2,3,4,5,6,7,8,9};
            RlpArrayKind rlpKind = new RlpArrayKind(new RlpIntKind());
            byte[] encode = rlpKind.EncodeToRlp(data).Encode();
            RlpArray rlpArray = new RlpArray().Decode(encode) as RlpArray;
            int[] decode = rlpKind.DecodeFromRlp<int>(rlpArray);
            Assert.True(data.SequenceEqual(decode));
        }

        [Fact]
        public void RlpScalarArrayOfStringTest()
        {
            string[] data = new string[]{"hello","vechain","connex",""};
            RlpArrayKind rlpKind = new RlpArrayKind(new RlpStringKind());
            byte[] encode = rlpKind.EncodeToRlp(data).Encode();
            RlpArray rlpArray = new RlpArray().Decode(encode) as RlpArray;
            string[] decode = rlpKind.DecodeFromRlp<string>(rlpArray);
            Assert.True(data.SequenceEqual(decode));
        }
    }

    public class RlpStructTest
    {
        [Fact]
        public void RlpStructKindTest()
        {
            var data = new TransactionBody(){
                ChainTag = 74,
                BlockRef = "0x005d64da8e7321bd",
                Expiration = 18,
                GasPriceCoef = 0,
                Gas = 21000,
                DependsOn = "",
                Nonce = "0xd6846cde87878603",
                Reserved = null,
                Clauses = new Clause[]{
                    new Clause(){To = "0xa4aDAfAef9Ec07BC4Dc6De146934C7119341eE25",Value = new BigInteger(100000), Data = "0x2398479812734981"},
                    new Clause(){To = "0xa4aDAfAef9Ec07BC4Dc6De146934C7119341eE25",Value = new BigInteger(100000), Data = "0x2398479812734981"},
                    new Clause(){To = "0xa4aDAfAef9Ec07BC4Dc6De146934C7119341eE25",Value = new BigInteger(100000), Data = "0x2398479812734981"}
                }
            };

            var rlpKind = new RlpStructKind();
            rlpKind.Properties.Add(new RlpIntKind("ChainTag"));
            rlpKind.Properties.Add(new RlpHexStringKind("BlockRef",false,8));
            rlpKind.Properties.Add(new RlpIntKind("Expiration"));

            var clause = new RlpStructKind();
            clause.Properties.Add(new RlpHexStringKind("To",true,20));
            clause.Properties.Add(new RlpBigIntegerKind("Value",false,32));
            clause.Properties.Add(new RlpHexStringKind("Data"));
            var clauses = new RlpArrayKind(clause,"Clauses");
            rlpKind.Properties.Add(clauses);

            rlpKind.Properties.Add(new RlpIntKind("GasPriceCoef"));
            rlpKind.Properties.Add(new RlpIntKind("Gas",8));
            rlpKind.Properties.Add(new RlpHexStringKind("DependsOn",true,32));
            rlpKind.Properties.Add(new RlpHexStringKind("Nonce",false,8));

            var reserved = new RlpStructKind("Reserved",true);
            reserved.Properties.Add(new RlpIntKind("Features"));
            reserved.Properties.Add(new RlpArrayKind(new RlpBytesKind(true),"Unused"));

            rlpKind.Properties.Add(reserved);

            byte[] encode = rlpKind.EncodeToRlp(data).Encode();
            var encode2 = "f8844a875d64da8e7321bd12f869e294a4adafaef9ec07bc4dc6de146934c7119341ee25830186a0882398479812734981e294a4adafaef9ec07bc4dc6de146934c7119341ee25830186a0882398479812734981e294a4adafaef9ec07bc4dc6de146934c7119341ee25830186a0882398479812734981808252088088d6846cde87878603c0".ToBytes();
            Assert.True(encode.SequenceEqual(encode2));

            
            
            RlpArray rlpArray = new RlpArray().Decode(encode2) as RlpArray;
            var decode = rlpKind.DecodeFromRlp(rlpArray,data.GetType());
            Assert.NotNull(decode);
        }

        public class TransactionBody
        {
            public int ChainTag {get;set;}
            public string BlockRef {get;set;}
            public int Expiration {get;set;}
            public int GasPriceCoef {get;set;}
            public int Gas {get;set;}
            public string DependsOn {get;set;}
            public string Nonce {get;set;}
            public Reserved Reserved {get;set;}
            public Clause[] Clauses {get;set;}
        }

        public class Reserved
        {
            public int Features {get;set;}
            public byte[][] Unused {get;set;}
        }

        public class Clause
        {
            public string To {get;set;}
            public BigInteger Value {get;set;}
            public string Data {get;set;}
        }
    
    }
}