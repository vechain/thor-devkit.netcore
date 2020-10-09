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
        public void TestHDNodeByMnemonic()
        {
            var works = new[]{"mouse","brave","fun","viable","utility","veteran","luggage","area","bike","myself","target","thunder"};
            IHDNode node = new HDNode(works);
            IHDNode sonNode = node.Derive(0);
            IHDNode grandNode = sonNode.Derive(1);

            Assert.True(node.PrivateKey.ToHexString() == "0x546498ba394789f18b5b4b62227572fe033f8f7f2597a747ada3d8ec8f20f650");
            Assert.True(sonNode.PrivateKey.ToHexString() == "0x12ddf96bb7f2c031ee9b776e2b236f2fc46de6f90fb9cd48a82e0c849327d251");
            Assert.True(grandNode.PrivateKey.ToHexString() == "0xa7af80364059211616e47394cf49240b40f7a87cea54ec99a6aa1815b586d74e");
        }

        [Fact]
        public void TestHDNodeBySeed()
        {
            byte[] seed = "0x403d3e5f3646f517d3d6f51bd98f90493d502d259506abbfe46d5af2196960531edf6030aef3876fe3432f457c9cb9d6f312c538f19d3f30e731022d309683c2".ToBytes();
            IHDNode node = new HDNode(seed);
            IHDNode sonNode = node.Derive(0);
            IHDNode grandNode = sonNode.Derive(1);

            Assert.True(node.PrivateKey.ToHexString() == "0x546498ba394789f18b5b4b62227572fe033f8f7f2597a747ada3d8ec8f20f650");
            Assert.True(sonNode.PrivateKey.ToHexString() == "0x12ddf96bb7f2c031ee9b776e2b236f2fc46de6f90fb9cd48a82e0c849327d251");
            Assert.True(grandNode.PrivateKey.ToHexString() == "0xa7af80364059211616e47394cf49240b40f7a87cea54ec99a6aa1815b586d74e");

        }

        [Fact]
        public void TestHDNodeByPrivateKey()
        {
            byte[] privateKey = "0x546498ba394789f18b5b4b62227572fe033f8f7f2597a747ada3d8ec8f20f650".ToBytes();
            byte[] chainCode = "0xc86826253a925f5958a838756e20ba33e71566da60b9e274f97342492c578c10".ToBytes();
            IHDNode node = new HDNode(privateKey,chainCode);
            IHDNode sonNode = node.Derive(0);
            IHDNode grandNode = sonNode.Derive(1);

            Assert.True(node.PrivateKey.ToHexString() == "0x546498ba394789f18b5b4b62227572fe033f8f7f2597a747ada3d8ec8f20f650");
            Assert.True(sonNode.PrivateKey.ToHexString() == "0x12ddf96bb7f2c031ee9b776e2b236f2fc46de6f90fb9cd48a82e0c849327d251");
            Assert.True(grandNode.PrivateKey.ToHexString() == "0xa7af80364059211616e47394cf49240b40f7a87cea54ec99a6aa1815b586d74e");
        }
    }
}