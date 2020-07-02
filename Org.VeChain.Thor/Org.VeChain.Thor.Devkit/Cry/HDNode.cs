using System.Collections.Generic;
using Nethereum.HdWallet;
using Org.VeChain.Thor.Devkit.Extension;
using NBitcoin;

namespace Org.VeChain.Thor.Devkit.Cry
{
    public enum HDNodeType
    {
        Mnemonic,
        Seed,
        PrivateKey
    }
    public interface IHDNode
    {
        byte[] PrivateKey { get; }
        byte[] PublicKey { get; }
        byte[] ChainCode { get; }
        HDNodeType NodeType { get; }
        IHDNode Derive(uint index);
    }

    public class HDNode:IHDNode
    {
        public HDNode(string[] words,string path =  VeChainConstant.VET_DERIVATION_PATH):this(new Wallet(Mnemonic.WordsJoin(words),"",path),HDNodeType.Mnemonic){}

        public HDNode(byte[] seed,string path =  VeChainConstant.VET_DERIVATION_PATH):this(new Wallet(seed,path),HDNodeType.Seed){}

        public HDNode(byte[] privateKey,byte[] chainCode):this(new ExtKey(new Key(privateKey),chainCode)){}

        public byte[] PrivateKey { get; protected set;}
        public byte[] PublicKey { get; protected set;}
        public byte[] ChainCode { get; protected set;}
        public HDNodeType NodeType { get; protected set; }

        public IHDNode Derive(uint index)
        {
            switch(this.NodeType)
            {
                case HDNodeType.Mnemonic:
                {
                    string derivePath = string.Format("{0}/{1}",this._wallet.Path,index.ToString());
                    return new HDNode(new Wallet(Mnemonic.WordsJoin(this._wallet.Words),derivePath),this.NodeType);
                }
                case HDNodeType.Seed:
                {
                    string derivePath = string.Format("{0}/{1}",this._wallet.Path,index.ToString());
                    return new HDNode(new Wallet(this._wallet.Seed.ToBytes(),derivePath),this.NodeType);
                }
                case HDNodeType.PrivateKey:
                {
                    ExtKey childKey = this._masterKey.Derive(index);
                    return new HDNode(childKey);
                }
                default:
                {
                    return null;
                }
            }
        }

        private HDNode(Wallet wallet,HDNodeType type)
        {
            this._wallet = wallet;
            this.PrivateKey = wallet.GetPrivateKey(0);
            this.PublicKey = wallet.GetPublicKey(0);
            this.ChainCode = wallet.GetKey(0).ChainCode;
            this.NodeType = HDNodeType.Seed;
        }

        public HDNode(ExtKey masterKey)
        {
            this._masterKey = masterKey;
            this.PrivateKey = masterKey.PrivateKey.ToBytes();
            this.PublicKey = Secp256k1.DerivePublicKey(this.PrivateKey);
            this.ChainCode = masterKey.ChainCode;
            this.NodeType = HDNodeType.PrivateKey;
        }

        private Wallet _wallet;
        private ExtKey _masterKey;
    }
}