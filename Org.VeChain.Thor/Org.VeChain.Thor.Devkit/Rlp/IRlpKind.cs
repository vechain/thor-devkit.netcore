using System.Collections.Generic;
using System;
using Newtonsoft.Json.Linq;

namespace Org.VeChain.Thor.Devkit.Rlp
{
    public interface IRlpKind
    {
        string Name { get; set; }
        bool Nullable { get; }
    }

    public interface IRlpScalarKind:IRlpKind
    {
        IRlpItem EncodeToRlp(dynamic obj);
        dynamic DecodeFromRlp(IRlpItem rlp);
        IRlpItem EncodeWithJson(JToken jvalue);
        JValue DecodeToJson(IRlpItem rlp);
    }

    public interface IRlpArrayKind:IRlpKind
    {
        IRlpKind ItemKind { get; }
        RlpArray EncodeToRlp(dynamic items);
        dynamic DecodeFromRlp(IRlpItem rlp,Type type);
        T[] DecodeFromRlp<T>(IRlpItem rlp);
        RlpArray EncodeWithJson(JToken items);
        JArray DecodeToJson(IRlpItem rlp);
    }

    public interface IRplStructKind:IRlpKind
    {
        List<IRlpKind> Properties { get;set; }
        RlpArray EncodeToRlp(dynamic obj);
        dynamic DecodeFromRlp(IRlpItem rlp,Type type);
        RlpArray EncodeWithJson(JToken item);
        JToken DecodeToJson(IRlpItem rlp);
    }

    public interface IRlpCustomKind:IRlpKind
    {
        IRlpItem EncodeToRlp(dynamic obj);
        dynamic DecodeFromRlp(IRlpItem rlp,Type type);
        IRlpItem EncodeWithJson(JToken item);
        JToken DecodeToJson(IRlpItem rlp);
    }
}