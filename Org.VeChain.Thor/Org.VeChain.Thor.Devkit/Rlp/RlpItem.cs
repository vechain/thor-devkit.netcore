using Nethereum.RLP;
using System.Collections.Generic;

namespace Org.VeChain.Thor.Devkit.Rlp
{
    public enum RlpType
    {
        Item,
        Array
    }

    public interface IRlpItem
    {
        byte[] RlpData { get; set; }
        RlpType RlpType { get; }
        byte[] Encode();
        IRlpItem Decode(byte[] bytes);
    }

    public class RlpHelper
    {
        public static RlpType GetRlpType(byte prefix)
        {
            if(prefix < 0xc0)
            {
                return RlpType.Item;
            }

            return RlpType.Array;
        }
    }

    public class RlpItem:IRlpItem
    {
        public RlpType RlpType => RlpType.Item;

        public byte[] RlpData { get; set; }

        public RlpItem()
        {
            this.RlpData = new byte[0];
        }
        public RlpItem(byte[] rlpData)
        {
            this.RlpData = rlpData;
        }

        public byte[] Encode()
        {
            return RLP.EncodeElement(this.RlpData);
        }

        public IRlpItem Decode(byte[] bytes)
        {
            IRLPElement rLPElement = RLP.Decode(bytes);
            return new RlpItem(rLPElement.RLPData);
        }
    }

    public class RlpArray:List<IRlpItem>,IRlpItem
    {
        public RlpType RlpType => RlpType.Array;

        public RlpArray(){}

        public RlpArray(byte[] rlpData)
        {
            this.RlpData = rlpData;
        }

        public byte[] RlpData { get; set; }

        public RlpArray(IRlpItem[] rlpItems)
        {
            this.AddRange(rlpItems);
        }

        public byte[] Encode()
        {
            List<byte[]> datas = new List<byte[]>();
            foreach(IRlpItem item in this)
            {
                if(item != null)
                {
                    datas.Add(item.Encode());
                }
            }
            return RLP.EncodeList(datas.ToArray());
        }

        public IRlpItem Decode(byte[] bytes)
        {
            RlpArray rlpArray = new RlpArray();
            RLPCollection rlpCollection = new RLPCollection();
            rlpCollection = RLP.Decode(bytes) as RLPCollection;
            this.RlpData = rlpCollection.RLPData;
            foreach(IRLPElement item in rlpCollection)
            {
                if(item.RLPData == null || item.RLPData[0] == 0x0)
                {
                    rlpArray.Add(new RlpItem());
                    continue;
                }

                var rlpItem = new RlpItem(item.RLPData);
                rlpArray.Add(rlpItem);
            }
            return rlpArray;
        }

        private List<IRlpItem> rlpItems = new List<IRlpItem>();
    }
}