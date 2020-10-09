using System;

namespace Org.VeChain.Thor.Devkit.Rlp
{
    public class RlpCode
    {
        public static byte[] Encode(IRlpKind kind,dynamic obj)
        {
            IRlpItem rlpItem = null;
            if(kind is IRlpScalarKind scalarKind)
            {
                rlpItem = scalarKind.EncodeToRlp(obj);
                return rlpItem.Encode();
            }

            if(kind is IRlpArrayKind arrayKind)
            {
                if(obj is Array)
                {
                    rlpItem = arrayKind.EncodeToRlp(obj);
                    return rlpItem.Encode();
                }

                throw new ArgumentException("invalid item type");
            }

            if(kind is IRplStructKind structKind)
            {
                rlpItem = structKind.EncodeToRlp(obj);
                return rlpItem.Encode();
            }

            if(kind is IRlpCustomKind customKind)
            {
                rlpItem = customKind.EncodeToRlp(obj);
                return rlpItem.Encode();
            }
            return rlpItem.Encode();
        }

        public static dynamic Decode(IRlpKind kind,byte[] data,Type type)
        {
            dynamic result = null;
            IRlpItem rlpItem = new RlpItem(data);
            if(kind is IRlpScalarKind scalarKind)
            {
                result = scalarKind.DecodeFromRlp(rlpItem);
            }
            else if(kind is IRlpArrayKind arrayKind)
            {
                result = arrayKind.DecodeFromRlp(rlpItem,type);
            }
            else if(kind is IRplStructKind structKind)
            {
                result = structKind.DecodeFromRlp(rlpItem,type);
            }
            else if(kind is IRlpCustomKind customKind)
            {
                result = customKind.DecodeFromRlp(rlpItem,type);
            }
            return result;
        }
        
    }
}