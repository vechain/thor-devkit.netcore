using System.Numerics;
using System.Collections.Generic;

namespace Org.VeChain.Thor.Devkit.Rlp
{
    public class RlpBigIntegerKind:IRlpScalarKind<BigInteger>
    {
        public string Name { get; set;}
        public IRlpItem EncodeToRlp(BigInteger obj)
        {
            return obj.EncodeToRlpItem();
        }

        public BigInteger DecodeFromRlp(IRlpItem rlp)
        {
            return (rlp as RlpItem).DecodeToBigInteger();
        }
    }

    public class RlpIntKind:IRlpScalarKind<int>
    {
        public string Name { get; set;}

        public IRlpItem EncodeToRlp(int obj)
        {
            return obj.EncodeToRlpItem();
        }

        public int DecodeFromRlp(IRlpItem rlp)
        {
            return (rlp as RlpItem).DecodeToInt();
        }
    }

    public class RlpLongKind:IRlpScalarKind<long>
    {
        public string Name { get; set;}

        public IRlpItem EncodeToRlp(long obj)
        {
            return obj.EncodeToRlpItem();
        }

        public long DecodeFromRlp(IRlpItem rlp)
        {
            return (rlp as RlpItem).DecodeToLong();
        }
    }

    public class RlpStringKind:IRlpScalarKind<string>
    {
        public string Name { get; set;}

        public IRlpItem EncodeToRlp(string obj)
        {
            return obj.EncodeToRlpItem();
        }

        public string DecodeFromRlp(IRlpItem rlp)
        {
            return (rlp as RlpItem).DecodeToString();
        }

    }

    public class RlpArrayKind<D,T>:IRlpArrayKind<D,T> where D:IRlpKind,new()
    {
        public string Name { get; set;}

        public RlpArray EncodeToRlp(T[] items)
        {
            IRlpKind kind = new D();
            RlpArray rlpArray = new RlpArray();

            foreach(T item in items)
            {
                if(kind is IRlpScalarKind)
                {
                    rlpArray.Add((kind as IRlpScalarKind<T>).EncodeToRlp(item));
                }

                if(kind is IRlpArrayKind<IRlpKind>)
                {
                    rlpArray.Add((kind as IRlpArrayKind<IRlpKind,dynamic>).EncodeToRlp(item as dynamic[]));
                }

                if(kind is IStructKind)
                {
                    rlpArray.Add((kind as IStructKind<dynamic>).EncodeToRlp(item));
                }
            }

            return rlpArray;
        }

        public T[] DecodeFromRlp(RlpArray rlpData)
        {
            List<T> result = new List<T>();
            byte[][] list = rlpData.DecodeToList();
            IRlpKind definition = new D();

            foreach(byte[] item in list)
            {
                if(definition is IRlpScalarKind)
                {
                    var rlpitem = new RlpItem(item);
                    result.Add((definition as IRlpScalarKind<T>).DecodeFromRlp(rlpitem));
                }

                if(definition is IRlpArrayKind<IRlpKind>)
                {
                    var rlpArray = new RlpArray(item);
                    result.AddRange((definition as IRlpArrayKind<IRlpKind,T>).DecodeFromRlp(rlpArray));
                }

                if(definition is IStructKind)
                {
                    var rlpArray = new RlpArray(item);
                    result.Add((definition as IStructKind<T>).DecodeFromRlp(rlpArray));
                }
            }

            return result.ToArray();
        }
    }
}