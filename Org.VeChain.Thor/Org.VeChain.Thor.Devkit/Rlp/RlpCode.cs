using System.Reflection;
using System;

namespace Org.VeChain.Thor.Devkit.Rlp
{
    public class RlpCode
    {
        public static byte[] Encode(IRlpKind kind,dynamic obj)
        {
            IRlpItem rlpItem = null;
            if(kind is IRlpScalarKind)
            {
                rlpItem = (kind as IRlpScalarKind).EncodeToRlp(obj);
                return rlpItem.Encode();
            }
            else if(kind is IRlpArrayKind)
            {
                if(obj is Array)
                {
                    rlpItem = (kind as IRlpArrayKind).EncodeToRlp(obj);
                    return rlpItem.Encode();
                }
                else
                {
                    throw new ArgumentException("invalid item type");
                }
            }
            else if(kind is IRplStructKind)
            {
                rlpItem = (kind as IRplStructKind).EncodeToRlp(obj);
                return rlpItem.Encode();
            }
            else if(kind is IRlpCustomKind)
            {
                rlpItem = (kind as IRlpCustomKind).EncodeToRlp(obj);
                return rlpItem.Encode();
            }
            return rlpItem.Encode();
        }

        public static dynamic Decode(IRlpKind kind,byte[] data,Type type)
        {
            dynamic result = null;
            IRlpItem rlpItem = new RlpItem(data);
            if(kind is IRlpScalarKind)
            {
                result = (kind as IRlpScalarKind).DecodeFromRlp(rlpItem);
            }
            else if(kind is IRlpArrayKind)
            {
                result = (kind as IRlpArrayKind).DecodeFromRlp(rlpItem,type);
            }
            else if(kind is IRplStructKind)
            {
                result = (kind as IRplStructKind).DecodeFromRlp(rlpItem,type);
            }
            else if(kind is IRlpCustomKind)
            {
                result = (kind as IRlpCustomKind).DecodeFromRlp(rlpItem,type);
            }
            return result;
        }
        
    }
}