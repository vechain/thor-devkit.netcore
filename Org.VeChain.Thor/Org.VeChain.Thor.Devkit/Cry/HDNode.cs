using System.Collections.Generic;
using Nethereum.HdWallet;
using Org.VeChain.Thor.Devkit.Extension;

namespace Org.VeChain.Thor.Devkit.Cry
{
    public interface IHDNode
    {
        byte[] privateKey { get; }
        byte[] publicKey { get; }
        byte[] chainCode { get; }
        string path { get; }
        IHDNode Derive(int index);
    }

    public class HDNode:IHDNode
    {
        public HDNode(string[] words,string path =  VeChainConstant.VET_DERIVATION_PATH):this(new Wallet(Mnemonic.WordsJoin(words),"",path))
        {
        }

        public byte[] privateKey { get; protected set;}
        public byte[] publicKey { get; protected set;}
        public byte[] chainCode { get; protected set;}

        public string path {get; protected set;}

        public IHDNode Derive(int index)
        {
            string derivePath = string.Format("{0}/{1}",this._wallet.Path,index.ToString());
            return new HDNode(new Wallet(Mnemonic.WordsJoin(this._wallet.Words),"",derivePath));
        }

        private HDNode(Wallet wallet)
        {
            this._wallet = wallet;
            this.privateKey = wallet.GetPrivateKey(0);
            this.publicKey = wallet.GetPublicKey(0);
            this.chainCode = wallet.GetKey(0).ChainCode;
            this.path = this._wallet.Path;
        }

        private Wallet _wallet;

    }
}