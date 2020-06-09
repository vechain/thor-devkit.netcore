using Xunit;
using Xunit.Abstractions;
using Org.VeChain.Thor.Devkit.Rlp;
using Org.VeChain.Thor.Devkit.Extension;
using System.Numerics;
using System.Globalization;
using System.Linq;
using System.Collections.Generic;

namespace Org.VeChain.Thor.Devkit.UnitTest
{
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

            BigInteger dataZero = BigInteger.Zero;
            byte[] encodeZero = (new RlpBigIntegerKind()).EncodeToRlp(dataZero).Encode();
            RlpItem itemZero = new RlpItem().Decode(encodeZero) as RlpItem;
            BigInteger decodeZero = (new RlpBigIntegerKind()).DecodeFromRlp(itemZero);
            Assert.True(dataZero.Equals(decodeZero));
        }

        [Fact]
        public void RlpIntTest()
        {
            int data = 100;
            byte[] encode = (new RlpIntKind()).EncodeToRlp(data).Encode();
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


    public class RlpArrayTestNew
    {
        [Fact]
        public void RlpScalarArrayOfNumberTest(){
            int[] data = new int[]{0,1,2,3,4,5,6,7,8,9};
            RlpArrayKind<RlpIntKind,int> rlpDefinition = new RlpArrayKind<RlpIntKind,int>();
            byte[] encode = rlpDefinition.EncodeToRlp(data).Encode();
            RlpArray rlpArray = new RlpArray().Decode(encode) as RlpArray;
            int[] decode = rlpDefinition.DecodeFromRlp(rlpArray);
            Assert.True(data.SequenceEqual(decode));
        }

        [Fact]
        public void RlpScalarArrayOfStringTest()
        {
            string[] data = new string[]{"hello","vechain","connex",""};
            RlpArrayKind<RlpStringKind,string> rlpDefinition = new RlpArrayKind<RlpStringKind,string>();
            byte[] encode = rlpDefinition.EncodeToRlp(data).Encode();
            RlpArray rlpArray = new RlpArray().Decode(encode) as RlpArray;
            string[] decode = rlpDefinition.DecodeFromRlp(rlpArray);
            Assert.True(data.SequenceEqual(decode));
        }
    }
}