using Xunit;
using Xunit.Abstractions;
using Org.VeChain.Thor.Devkit.Abi;
using System.Linq;
using Org.VeChain.Thor.Devkit.Extension;
using System.Numerics;
using System.Collections.Generic;

namespace Org.VeChain.Thor.Devkit.UnitTest
{
    public class AbiTest
    {
        private readonly ITestOutputHelper _output;

        public AbiTest(ITestOutputHelper testOutputHelper)
        {
            _output = testOutputHelper;
        }

        [Fact]
        public void TestAbiParameBuilder()
        {
            string abiJson = "[{\"name\":\"to\",\"type\":\"address\"},{\"name\":\"numProposals\",\"type\":\"uint8\"}]";
            IAbiParameterDefinition[] parames = new AbiParameterBuilder().Builder(abiJson);
            Assert.True(parames.Length == 2);
            Assert.True(parames[0].Name.Equals("to"));
            Assert.True(parames[0].ABIType.Equals("address"));
            Assert.True(parames[1].Name.Equals("numProposals"));
            Assert.True(parames[1].ABIType.Equals("uint8"));
        }

        [Fact]
        public void TestAbiFunctionBuilder()
        {
            string abiJson = "{\"constant\": false,\"inputs\": [{\"name\": \"a1\",\"type\": \"uint256\"},{\"name\": \"a2\",\"type\": \"string\"}],\"name\": \"f1\",\"outputs\": [{\"name\": \"r1\",\"type\": \"address\"},{\"name\": \"r2\",\"type\": \"bytes\"}],\"payable\": false,\"stateMutability\": \"nonpayable\",\"type\": \"function\"}";
            IAbiFunctionDefinition definition = (new AbiFunctionBuiler()).Builder(abiJson);
            Assert.True(definition.Type.Equals("function"));
            Assert.True(definition.Name.Equals("f1"));
            Assert.True(definition.Constant.Equals(false));
            Assert.True(definition.Payable.Equals(false));
            Assert.True(definition.stateMutability.Equals(AbiStateMutability.Nonpayable));
            Assert.True(definition.Sha3Signature.SequenceEqual("0x27fcbb2f".ToBytes()));

            Assert.True(definition.inputs.Length == 2);
            Assert.True(definition.inputs[0].Name.Equals("a1"));
            Assert.True(definition.inputs[0].ABIType.Equals("uint256"));
            Assert.True(definition.inputs[1].Name.Equals("a2"));
            Assert.True(definition.inputs[1].ABIType.Equals("string"));

            Assert.True(definition.outputs.Length == 2);
            Assert.True(definition.outputs[0].Name.Equals("r1"));
            Assert.True(definition.outputs[0].ABIType.Equals("address"));
            Assert.True(definition.outputs[1].Name.Equals("r2"));
            Assert.True(definition.outputs[1].ABIType.Equals("bytes"));
        }

        [Fact]
        public void TestAbiFunctionEncode()
        {
            string abiJson = "{\"constant\": false,\"inputs\": [{\"name\": \"a1\",\"type\": \"uint256\"},{\"name\": \"a2\",\"type\": \"string\"}],\"name\": \"f1\",\"outputs\": [{\"name\": \"r1\",\"type\": \"address\"},{\"name\": \"r2\",\"type\": \"bytes\"}],\"payable\": false,\"stateMutability\": \"nonpayable\",\"type\": \"function\"}";
            AbiFuncationCoder coder = new AbiFuncationCoder(abiJson);
            var encodeData = coder.Encode(1,"foo");

            var data = "0x27fcbb2f000000000000000000000000000000000000000000000000000000000000000100000000000000000000000000000000000000000000000000000000000000400000000000000000000000000000000000000000000000000000000000000003666f6f0000000000000000000000000000000000000000000000000000000000";
            Assert.True(encodeData.SequenceEqual(data.ToBytes()));
        }

        [Fact]
        public void TestAbiFunctionDecode()
        {
            string outputData = "0x0000000000000000000000004c6f3ca686c053354a83c030e80d2ee0a000b0cf000000000000000000000000000000000000000000000000000000000000000500000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000005c073850000000000000000000000000000000000000000000000000000000005c073850000000000000000000000000000000000000000000000000000000005c073850";
            string abiJson = "{\"constant\": true,\"inputs\": [{\"name\": \"_tokenId\",\"type\": \"uint256\"}],\"name\": \"getMetadata\",\"outputs\": [{\"name\": \"\",\"type\": \"address\"},{\"name\": \"\",\"type\": \"uint8\"},{\"name\": \"\",\"type\": \"bool\"},{\"name\": \"\",\"type\": \"bool\"},{\"name\": \"\",\"type\": \"uint64\"},{\"name\": \"\",\"type\": \"uint64\"},{\"name\": \"\",\"type\": \"uint64\"}],\"payable\": false,\"stateMutability\": \"view\",\"type\": \"function\"}";
            AbiFuncationCoder coder = new AbiFuncationCoder(abiJson);
            var output = coder.Decode(outputData.ToBytes());

            Assert.True((output[0].Result as string).Equals("0x4c6f3ca686c053354a83c030e80d2ee0a000b0cf"));
            Assert.True(((BigInteger)output[1].Result).Equals(new BigInteger(5)));
            Assert.True(((bool)output[2].Result).Equals(false));
            Assert.True(((bool)output[3].Result).Equals(false));
            Assert.True(((BigInteger)output[4].Result).Equals(new BigInteger(1543977040)));
            Assert.True(((BigInteger)output[5].Result).Equals(new BigInteger(1543977040)));
            Assert.True(((BigInteger)output[6].Result).Equals(new BigInteger(1543977040)));
        }

        [Fact]
        public void TestEventEncode()
        {
            string abiJson = "{\"anonymous\": false,\"inputs\": [{\"indexed\": true,\"name\": \"_from\",\"type\": \"address\"},{\"indexed\": true,\"name\": \"_to\",\"type\": \"address\"},{\"indexed\": false,\"name\": \"_value\",\"type\": \"uint256\"}],\"name\": \"Transfer\",\"type\": \"event\"}";
            AbiEventCoder coder = new AbiEventCoder(abiJson);

            Dictionary<string,dynamic> indexed = new Dictionary<string,dynamic>();
            indexed.Add("_from","0xe4aea9f855d6960d56190fb26e32d0ec2ab40d82");
            indexed.Add("_to","0xf43a84be55e162034f4c13de65294a3875f15bc9");

            byte[][] filters = coder.EncodeFilter(indexed);

            Assert.True(filters[0].SequenceEqual("0xddf252ad1be2c89b69c2b068fc378daa952ba7f163c4a11628f55a4df523b3ef".ToBytes()));
            Assert.True(filters[1].SequenceEqual("0x000000000000000000000000e4aea9f855d6960d56190fb26e32d0ec2ab40d82".ToBytes()));
            Assert.True(filters[2].SequenceEqual("0x000000000000000000000000f43a84be55e162034f4c13de65294a3875f15bc9".ToBytes()));
        }

        [Fact]
        public void TestEventDecode()
        {
            string abiJson = "{\"anonymous\": false,\"inputs\": [{\"indexed\": true,\"name\": \"_from\",\"type\": \"address\"},{\"indexed\": true,\"name\": \"_to\",\"type\": \"address\"},{\"indexed\": false,\"name\": \"_value\",\"type\": \"uint256\"}],\"name\": \"Transfer\",\"type\": \"event\"}";
            AbiEventCoder coder = new AbiEventCoder(abiJson);

            List<byte[]> topics = new List<byte[]>();
            topics.Add("0xddf252ad1be2c89b69c2b068fc378daa952ba7f163c4a11628f55a4df523b3ef".ToBytes());
            topics.Add("0x000000000000000000000000e4aea9f855d6960d56190fb26e32d0ec2ab40d82".ToBytes());
            topics.Add("0x000000000000000000000000f43a84be55e162034f4c13de65294a3875f15bc9".ToBytes());
            byte[] data = "0x0000000000000000000000000000000000000000000000056bc75e2d63100000".ToBytes();

            AbiEventTopic[] decode = coder.DecodeTopics(topics.ToArray(),data);
            
            Assert.True(decode.Length == 3);
            Assert.True(decode[0].Definition.Name == "_from" && decode[0].Definition.Indexed && (decode[0].Result as string).Equals("0xe4aea9f855d6960d56190fb26e32d0ec2ab40d82"));
            Assert.True(decode[1].Definition.Name == "_to" && decode[1].Definition.Indexed && (decode[1].Result as string).Equals("0xf43a84be55e162034f4c13de65294a3875f15bc9"));
            Assert.True(decode[2].Definition.Name == "_value" && decode[2].Definition.Indexed == false && decode[2].Result.ToString().Equals("100000000000000000000"));
        }
    }
}