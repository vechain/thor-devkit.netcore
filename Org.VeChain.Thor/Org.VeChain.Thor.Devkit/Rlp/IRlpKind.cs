namespace Org.VeChain.Thor.Devkit.Rlp
{
    public interface IRlpKind
    {
        string Name { get; set; }
    }

    public interface IRlpScalarKind:IRlpKind{}

    public interface IRlpScalarKind<T>:IRlpScalarKind
    {
        IRlpItem EncodeToRlp(T obj);
        T DecodeFromRlp(IRlpItem rlp);
    }

    public interface IRlpArrayKind<D>:IRlpKind where D:IRlpKind{}

    public interface IRlpArrayKind<D,T>:IRlpArrayKind<D> where D : IRlpKind
    {
        RlpArray EncodeToRlp(T[] items);
        T[] DecodeFromRlp(RlpArray rlp);
    }

    public interface IStructKind:IRlpKind
    {
        IRlpKind[] Properties { get;set; }
    }

    public interface IStructKind<T>:IStructKind
    {
        RlpArray EncodeToRlp(T obj);
        T DecodeFromRlp(RlpArray rlp);
    }
}