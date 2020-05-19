using Xunit;
using Org.VeChain.Thor.Devkit.Cry;
using Org.VeChain.Thor.Devkit.Extension;
using Xunit.Abstractions;

namespace Org.VeChain.Thor.Devkit.UnitTest
{
    public class HDNodeTest
    {
        private readonly ITestOutputHelper _output;

        public HDNodeTest(ITestOutputHelper testOutputHelper)
        {
            _output = testOutputHelper;
        }

        [Fact]
        public void TestHDNode()
        {
            IHDNode node;
            IHDNode sonNode;
            IHDNode grandNode;

            var works = new string[12]{"mouse","brave","fun","viable","utility","veteran","luggage","area","bike","myself","target","thunder"};
            node = new HDNode(works);
            sonNode = node.Derive(0);
            grandNode = sonNode.Derive(1);

            Assert.True(node.privateKey.ConvertToHexString() == "0x546498ba394789f18b5b4b62227572fe033f8f7f2597a747ada3d8ec8f20f650");
            Assert.True(sonNode.privateKey.ConvertToHexString() == "0x12ddf96bb7f2c031ee9b776e2b236f2fc46de6f90fb9cd48a82e0c849327d251");
            Assert.True(grandNode.privateKey.ConvertToHexString() == "0xa7af80364059211616e47394cf49240b40f7a87cea54ec99a6aa1815b586d74e");
        }
    }
}