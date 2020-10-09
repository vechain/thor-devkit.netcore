using Nethereum.HdWallet;
using NBitcoin;

namespace Org.VeChain.Thor.Devkit.Extension
{
    public static class WalletExtension
    {
        public static ExtKey GetKey(this Wallet wallet,int index)
        {
            var masterKey = new ExtKey(wallet.Seed);
            var keyPath = new KeyPath(wallet.Path.Replace("x",index.ToString()));
            return masterKey.Derive(keyPath);
        }
    }
}
