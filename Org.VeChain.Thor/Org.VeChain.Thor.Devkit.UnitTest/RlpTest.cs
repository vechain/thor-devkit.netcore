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
        public void TestRlpBoolean()
        {
            var cases = new EncodeTestCase[]{
                new EncodeTestCase(true,new byte[1]{0x01}),
                new EncodeTestCase(false,new byte[1]{0x80}),
            };

            foreach(var item in cases)
            {
                byte[] encode = (new RLpBooleanKind()).EncodeToRlp(item.Input).Encode();
                if(!encode.SequenceEqual(item.Output))
                {
                    Assert.True(false,string.Format("Rlp faild,input:{0}output:{1}",item.Input,item.Output));
                    break;
                }
            }
            Assert.True(true);
        }

        [Fact]
        public void TestRlpInt()
        {
            var cases = new EncodeTestCase[]{
                new EncodeTestCase(0,"0x80".ToBytes()),
                new EncodeTestCase(127,"0x7F".ToBytes()),
                new EncodeTestCase(128,"0x8180".ToBytes()),
                new EncodeTestCase(256,"0x820100".ToBytes()),
                new EncodeTestCase(1024,"0x820400".ToBytes()),
                new EncodeTestCase(Convert.ToInt32("0xFFFFFF",16),"0x83FFFFFF".ToBytes()),
                new EncodeTestCase(Convert.ToInt32("0xFFFFFFFF",16),"0x84FFFFFFFF".ToBytes()),
            };

            foreach(var item in cases)
            {
                byte[] encode = (new RlpIntKind()).EncodeToRlp(item.Input).Encode();
                if(!encode.SequenceEqual(item.Output))
                {
                    Assert.True(false,string.Format("Rlp faild,input:{0}output:{1}",item.Input,item.Output));
                    break;
                }
            }
            Assert.True(true);
        }
    
        [Fact]
        public void TestRlpLong()
        {
            var cases = new EncodeTestCase[]{
                new EncodeTestCase(Convert.ToInt64("0xFFFFFFFF",16),"0x84FFFFFFFF".ToBytes()),
                new EncodeTestCase(Convert.ToInt64("0xFFFFFFFFFF",16),"0x85FFFFFFFFFF".ToBytes()),
                new EncodeTestCase(Convert.ToInt64("0xFFFFFFFFFFFF",16),"0x86FFFFFFFFFFFF".ToBytes()),
                new EncodeTestCase(Convert.ToInt64("0xFFFFFFFFFFFFFF",16),"0x87FFFFFFFFFFFFFF".ToBytes()),
                new EncodeTestCase(Convert.ToInt64("0xFFFFFFFFFFFFFFFF",16),"0x88FFFFFFFFFFFFFFFF".ToBytes()),
            };

            foreach(var item in cases)
            {
                byte[] encode = (new RlpLongKind()).EncodeToRlp(item.Input).Encode();
                if(!encode.SequenceEqual(item.Output))
                {
                    Assert.True(false,string.Format("Rlp faild,input:{0}output:{1}",item.Input,item.Output));
                    break;
                }
            }
            Assert.True(true);
        }

        [Fact]
        public void TestRlpBigInteger()
        {
            var cases = new EncodeTestCase[]{
                new EncodeTestCase(BigInteger.Zero,"0x80".ToBytes()),
                new EncodeTestCase(new BigInteger(1),"0x01".ToBytes()),
                new EncodeTestCase(new BigInteger(127),"0x7F".ToBytes()),
                new EncodeTestCase(new BigInteger(128),"0x8180".ToBytes()),
                new EncodeTestCase(new BigInteger(256),"0x820100".ToBytes()),
                new EncodeTestCase(new BigInteger(1024),"0x820400".ToBytes()),
                new EncodeTestCase(new BigInteger(0xFFFFFF),"0x83FFFFFF".ToBytes()),
                new EncodeTestCase(new BigInteger(0xFFFFFFFF),"0x84FFFFFFFF".ToBytes()),
                new EncodeTestCase(new BigInteger(0xFFFFFFFFFF),"0x85FFFFFFFFFF".ToBytes()),
                new EncodeTestCase(new BigInteger(0xFFFFFFFFFFFF),"0x86FFFFFFFFFFFF".ToBytes()),
                new EncodeTestCase(new BigInteger(0xFFFFFFFFFFFFFF),"0x87FFFFFFFFFFFFFF".ToBytes()),
                new EncodeTestCase(BigInteger.Parse("102030405060708090A0B0C0D0E0F2",NumberStyles.AllowHexSpecifier),"0x8F102030405060708090A0B0C0D0E0F2".ToBytes()),
                new EncodeTestCase(BigInteger.Parse("0100020003000400050006000700080009000A000B000C000D000E01",NumberStyles.AllowHexSpecifier),"0x9C0100020003000400050006000700080009000A000B000C000D000E01".ToBytes()),
                new EncodeTestCase(BigInteger.Parse("010000000000000000000000000000000000000000000000000000000000000000",NumberStyles.AllowHexSpecifier),"0xA1010000000000000000000000000000000000000000000000000000000000000000".ToBytes())
            };

            foreach(var item in cases)
            {
                byte[] encode = (new RlpBigIntegerKind()).EncodeToRlp(item.Input).Encode();
                if(!encode.SequenceEqual(item.Output))
                {
                    Assert.True(false,string.Format("Rlp faild,input:{0}output:{1}",item.Input,item.Output));
                    break;
                }
            }
            Assert.True(true);
        }
    
        [Fact]
        public void TestRlpBytes()
        {
            var cases = new EncodeTestCase[]{
                new EncodeTestCase(new byte[]{},"0x80".ToBytes()),
                new EncodeTestCase(new byte[]{0x7E},"0x7E".ToBytes()),
                new EncodeTestCase(new byte[]{0x7F},"0x7F".ToBytes()),
                new EncodeTestCase(new byte[]{0x80},"0x8180".ToBytes()),
                new EncodeTestCase(new byte[]{0x01,0x02,0x03},"0x83010203".ToBytes()),
            };

            foreach(var item in cases)
            {
                byte[] encode = (new RlpBytesKind()).EncodeToRlp(item.Input).Encode();
                if(!encode.SequenceEqual(item.Output))
                {
                    Assert.True(false,string.Format("Rlp faild,input:{0}output:{1}",item.Input,item.Output));
                    break;
                }
            }
            Assert.True(true);
        }
    
        [Fact]
        public void TestRlpHexString()
        {
            var cases = new EncodeTestCase[]{
                new EncodeTestCase("0x7E","0x7E".ToBytes()),
                new EncodeTestCase("0x7F","0x7F".ToBytes()),
                new EncodeTestCase("0x80","0x8180".ToBytes()),
            };

            foreach(var item in cases)
            {
                byte[] encode = (new RlpHexStringKind()).EncodeToRlp(item.Input).Encode();
                if(!encode.SequenceEqual(item.Output))
                {
                    Assert.True(false,string.Format("Rlp faild,input:{0}output:{1}",item.Input,item.Output));
                    break;
                }
            }
            Assert.True(true);
        }
    
        [Fact]
        public void TestRlpString()
        {
            var cases = new EncodeTestCase[]{
                new EncodeTestCase("","0x80".ToBytes()),
                new EncodeTestCase("dog","0x83646F67".ToBytes()),
                new EncodeTestCase("Lorem ipsum dolor sit amet, consectetur adipisicing eli","0xB74C6F72656D20697073756D20646F6C6F722073697420616D65742C20636F6E7365637465747572206164697069736963696E6720656C69".ToBytes()),
                new EncodeTestCase("Lorem ipsum dolor sit amet, consectetur adipisicing elit","0xB8384C6F72656D20697073756D20646F6C6F722073697420616D65742C20636F6E7365637465747572206164697069736963696E6720656C6974".ToBytes()),
                new EncodeTestCase("Lorem ipsum dolor sit amet, consectetur adipiscing elit. Curabitur mauris magna, suscipit sed vehicula non, iaculis faucibus tortor. Proin suscipit ultricies malesuada. Duis tortor elit, dictum quis tristique eu, ultrices at risus. Morbi a est imperdiet mi ullamcorper aliquet suscipit nec lorem. Aenean quis leo mollis, vulputate elit varius, consequat enim. Nulla ultrices turpis justo, et posuere urna consectetur nec. Proin non convallis metus. Donec tempor ipsum in mauris congue sollicitudin. Vestibulum ante ipsum primis in faucibus orci luctus et ultrices posuere cubilia Curae; Suspendisse convallis sem vel massa faucibus, eget lacinia lacus tempor. Nulla quis ultricies purus. Proin auctor rhoncus nibh condimentum mollis. Aliquam consequat enim at metus luctus, a eleifend purus egestas. Curabitur at nibh metus. Nam bibendum, neque at auctor tristique, lorem libero aliquet arcu, non interdum tellus lectus sit amet eros. Cras rhoncus, metus ac ornare cursus, dolor justo ultrices metus, at ullamcorper volutpat","0xB904004C6F72656D20697073756D20646F6C6F722073697420616D65742C20636F6E73656374657475722061646970697363696E6720656C69742E20437572616269747572206D6175726973206D61676E612C20737573636970697420736564207665686963756C61206E6F6E2C20696163756C697320666175636962757320746F72746F722E2050726F696E20737573636970697420756C74726963696573206D616C6573756164612E204475697320746F72746F7220656C69742C2064696374756D2071756973207472697374697175652065752C20756C7472696365732061742072697375732E204D6F72626920612065737420696D70657264696574206D6920756C6C616D636F7270657220616C6971756574207375736369706974206E6563206C6F72656D2E2041656E65616E2071756973206C656F206D6F6C6C69732C2076756C70757461746520656C6974207661726975732C20636F6E73657175617420656E696D2E204E756C6C6120756C74726963657320747572706973206A7573746F2C20657420706F73756572652075726E6120636F6E7365637465747572206E65632E2050726F696E206E6F6E20636F6E76616C6C6973206D657475732E20446F6E65632074656D706F7220697073756D20696E206D617572697320636F6E67756520736F6C6C696369747564696E2E20566573746962756C756D20616E746520697073756D207072696D697320696E206661756369627573206F726369206C756374757320657420756C74726963657320706F737565726520637562696C69612043757261653B2053757370656E646973736520636F6E76616C6C69732073656D2076656C206D617373612066617563696275732C2065676574206C6163696E6961206C616375732074656D706F722E204E756C6C61207175697320756C747269636965732070757275732E2050726F696E20617563746F722072686F6E637573206E69626820636F6E64696D656E74756D206D6F6C6C69732E20416C697175616D20636F6E73657175617420656E696D206174206D65747573206C75637475732C206120656C656966656E6420707572757320656765737461732E20437572616269747572206174206E696268206D657475732E204E616D20626962656E64756D2C206E6571756520617420617563746F72207472697374697175652C206C6F72656D206C696265726F20616C697175657420617263752C206E6F6E20696E74657264756D2074656C6C7573206C65637475732073697420616D65742065726F732E20437261732072686F6E6375732C206D65747573206163206F726E617265206375727375732C20646F6C6F72206A7573746F20756C747269636573206D657475732C20617420756C6C616D636F7270657220766F6C7574706174".ToBytes()),
            };

            foreach(var item in cases)
            {
                byte[] encode = (new RlpStringKind()).EncodeToRlp(item.Input).Encode();
                if(!encode.SequenceEqual(item.Output))
                {
                    Assert.True(false,string.Format("Rlp faild,input:{0}output:{1}",item.Input,item.Output));
                    break;
                }
            }
            Assert.True(true);
        }
    
        [Fact]
        public void TestRlpArray()
        {
            var cases1 = new EncodeTestCase[]{
                new EncodeTestCase(new int[0],"0xC0".ToBytes()),
                new EncodeTestCase(new int[3]{1,2,3},"0xC3010203".ToBytes())
            };

            foreach(var item in cases1)
            {
                byte[] encode = (new RlpArrayKind(new RlpIntKind(true)).EncodeToRlp(item.Input).Encode());
                if(!encode.SequenceEqual(item.Output))
                {
                    Assert.True(false,string.Format("Rlp faild,input:{0}output:{1}",item.Input,item.Output));
                    break;
                }
            }

            var cases2 = new EncodeTestCase[]{
                new EncodeTestCase(new string[]{"aaa", "bbb", "ccc", "ddd", "eee", "fff", "ggg", "hhh", "iii", "jjj", "kkk", "lll", "mmm", "nnn", "ooo"},"0xF83C836161618362626283636363836464648365656583666666836767678368686883696969836A6A6A836B6B6B836C6C6C836D6D6D836E6E6E836F6F6F".ToBytes()),
            };

            foreach(var item in cases2)
            {
                byte[] encode = (new RlpArrayKind(new RlpStringKind(true)).EncodeToRlp(item.Input).Encode());
                if(!encode.SequenceEqual(item.Output))
                {
                    Assert.True(false,string.Format("Rlp faild,input:{0}output:{1}",item.Input,item.Output));
                    break;
                }
            }


            Assert.True(true);
        }
    }
}