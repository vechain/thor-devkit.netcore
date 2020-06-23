using System.Numerics;
using System.Collections.Generic;
using System.Reflection;
using System;
using Org.VeChain.Thor.Devkit.Extension;
using System.Globalization;
using Newtonsoft.Json.Linq;
using System.Linq;
using Newtonsoft.Json;
using System.ComponentModel;

namespace Org.VeChain.Thor.Devkit.Rlp
{
    public class RlpBigIntegerKind : IRlpScalarKind
    {
        public string Name { get; set; }
        public bool Nullable { get; set; }

        public RlpBigIntegerKind(bool nullable = false, int maxbytes = 0)
        {
            this.Nullable = nullable;
            this._maxBytes = maxbytes;
        }

        public RlpBigIntegerKind(string name, bool nullable = false, int maxbytes = 0)
        : this(nullable, maxbytes)
        {
            this.Name = name;
        }
        public IRlpItem EncodeToRlp(dynamic obj)
        {
            if (obj == null && this.Nullable)
            { return new RlpItem(); }

            BigInteger value = BigInteger.Zero;
            bool canParse = BigInteger.TryParse(obj.ToString(), out value);
            if (canParse)
            {
                return value.EncodeToRlpItem();
            }
            else
            {
                throw new ArgumentException("value type invalid");
            }
        }

        public dynamic DecodeFromRlp(IRlpItem rlp)
        {
            if (this.Nullable && (rlp == null || rlp.RlpData.Length == 0))
            {
                return BigInteger.Zero;
            }

            if (this._maxBytes != 0 && rlp.RlpData.Length > this._maxBytes)
            {
                throw new ArgumentException("Exceed the maximum maxbytes");
            }
            
            return (rlp as RlpItem).DecodeToBigInteger();
        }
        
        public IRlpItem EncodeWithJson(JToken jvalue)
        {
            if(this.Nullable && (jvalue == null || jvalue.Type == JTokenType.Null))
            {
                return new RlpItem(); 
            }

            return this.EncodeToRlp((jvalue as JValue).Value);
        }

        public JValue DecodeToJson(IRlpItem rlp)
        {
            BigInteger value = this.DecodeFromRlp(rlp);
            return new JValue(value.ToString(""));
        }
        
        private int _maxBytes = 0;
    }

    public class RLpBooleanKind:IRlpScalarKind
    {
        public string Name { get; set; }
        public bool Nullable { get; set; }

        public RLpBooleanKind(bool nullable = false)
        {
            this.Nullable = nullable;
        }

        public RLpBooleanKind(string name, bool nullable = false):this(nullable)
        {
            this.Name = name;
        }
    
        public IRlpItem EncodeToRlp(dynamic obj)
        {
            if (obj == null && this.Nullable)
            { return new RlpItem(); }

            Boolean value = false;
            bool canParse = Boolean.TryParse(obj.ToString(),out value);
            if(canParse)
            {
                return value.EncodeToRlpItem();
            }
            else
            {
                throw new ArgumentException("value type invalid");
            }
        }

        public dynamic DecodeFromRlp(IRlpItem rlp)
        {
            if (this.Nullable && (rlp == null || rlp.RlpData.Length == 0))
            {
                return false;
            }
            
            return (rlp as RlpItem).DecodeToBoolean();
        }

        public IRlpItem EncodeWithJson(JToken jvalue)
        {
            if(this.Nullable && (jvalue == null || jvalue.Type == JTokenType.Null))
            {
                return new RlpItem(); 
            }

            return this.EncodeToRlp((jvalue as JValue).Value);
        }

        public JValue DecodeToJson(IRlpItem rlp)
        {
            BigInteger value = this.DecodeFromRlp(rlp);
            return new JValue(value);
        }
    }

    public class RlpIntKind : IRlpScalarKind
    {
        public string Name { get; set; }
        public bool Nullable { get; set; }

        public Type SourceType
        {
            get { return typeof(int); }
        }

        public RlpIntKind():this("",0,false){}
        public RlpIntKind(string name):this(name,0,false){}

        public RlpIntKind(int maxbytes = 0, bool nullable = false):this("",maxbytes,nullable){}

        public RlpIntKind(bool nullable = false):this("",0,nullable){}

        public RlpIntKind(string name,bool nullable = false):this(name,0,nullable){}

        public RlpIntKind(string name, int maxbytes = 0, bool nullable = false)
        {
            this._maxBytes = maxbytes;
            this.Nullable = nullable;
            this.Name = name;
        }

        public IRlpItem EncodeToRlp(dynamic obj)
        {
            if (this.Nullable && obj == null)
            {
                return new RlpItem();
            }

            Int32 value = 0;
            bool canParse = Int32.TryParse(obj.ToString(), out value);
            if (canParse)
            {
                IRlpItem item = value.EncodeToRlpItem();
                if (this._maxBytes != 0 && item.RlpData.Length > this._maxBytes)
                {
                    throw new ArgumentException("Exceed the maximum maxbytes");
                }
                return item;
            }
            else
            {
                throw new ArgumentException("value type invalid");
            }
        }

        public dynamic DecodeFromRlp(IRlpItem rlp)
        {
            if (this.Nullable && (rlp == null || rlp.RlpData.Length == 0))
            {
                return 0;
            }

            if (this._maxBytes != 0 && rlp.RlpData.Length > this._maxBytes)
            {
                throw new ArgumentException("Exceed the maximum maxbytes");
            }
            return (rlp as RlpItem).DecodeToInt();
        }

        public IRlpItem EncodeWithJson(JToken jvalue)
        {
            if(this.Nullable && (jvalue == null || jvalue.Type == JTokenType.Null))
            {
                return new RlpItem(); 
            }

            return this.EncodeToRlp((jvalue as JValue).Value);
        }

        public JValue DecodeToJson(IRlpItem rlp)
        {
            int value = this.DecodeFromRlp(rlp);
            return new JValue(value);
        }

        private int _maxBytes = 0;
    }

    public class RlpLongKind : IRlpScalarKind
    {
        public string Name { get; set; }
        public bool Nullable { get; set; }

        public Type SourceType
        {
            get { return typeof(long); }
        }

        public RlpLongKind(int maxbytes = 0, bool nullable = false)
        {
            this._maxBytes = maxbytes;
            this.Nullable = nullable;
        }

        public RlpLongKind(string name, int maxbytes = 0, bool nullable = false) : this(maxbytes, nullable)
        {
            this.Name = name;
        }

        public IRlpItem EncodeToRlp(dynamic obj)
        {
            if (this.Nullable && obj == null)
            {
                return new RlpItem();
            }

            Int64 value;
            bool canParse = Int64.TryParse(obj.ToString(), out value);
            if (canParse)
            {
                IRlpItem item = value.EncodeToRlpItem();
                if (this._maxBytes != 0 && item.RlpData.Length > this._maxBytes)
                {
                    throw new ArgumentException("Exceed the maximum maxbytes");
                }
                return item;
            }
            else
            {
                throw new ArgumentException("value type invalid");
            }
        }

        public dynamic DecodeFromRlp(IRlpItem rlp)
        {
            if (this.Nullable && (rlp == null || rlp.RlpData.Length == 0))
            {
                return 0;
            }

            if (this._maxBytes != 0 && rlp.RlpData.Length > this._maxBytes)
            {
                throw new ArgumentException("Exceed the maximum maxbytes");
            }
            return (rlp as RlpItem).DecodeToLong();
        }

        public IRlpItem EncodeWithJson(JToken jvalue)
        {
            if(this.Nullable && (jvalue == null || jvalue.Type == JTokenType.Null))
            {
                return new RlpItem(); 
            }

            return this.EncodeToRlp((jvalue as JValue).Value);
        }

        public JValue DecodeToJson(IRlpItem rlp)
        {
            long value = this.DecodeFromRlp(rlp);
            return new JValue(value);
        }

        private int _maxBytes = 0;
    }

    public class RlpStringKind : IRlpScalarKind
    {
        public string Name { get; set; }
        public bool Nullable { get; set; }

        public Type SourceType
        {
            get { return typeof(string); }
        }

        public RlpStringKind(int maxLength = 0, bool nullable = false)
        {
            this._maxLength = maxLength;
            this.Nullable = nullable;
        }

        public RlpStringKind(string name, int maxLength = 0) : this(maxLength)
        {
            this.Name = name;
        }

        public IRlpItem EncodeToRlp(dynamic obj)
        {
            if (this.Nullable && obj == null)
            {
                return new RlpItem();
            }

            if (this._maxLength != 0 && obj.Length > this._maxLength)
            {
                throw new ArgumentException("Exceed the maxLength maxbytes");
            }
            return obj is string ? (obj as string).EncodeToRlpItem() : obj.ToString().EncodeToRlpItem();
        }

        public dynamic DecodeFromRlp(IRlpItem rlp)
        {
            if (this.Nullable && (rlp == null || rlp.RlpData.Length == 0))
            {
                return "";
            }
            string value = (rlp as RlpItem).DecodeToString();

            if (this._maxLength != 0 && value.Length > this._maxLength)
            {
                throw new ArgumentException("Exceed the maxLength maxbytes");
            }
            return value;
        }

        public IRlpItem EncodeWithJson(JToken jvalue)
        {
            if(this.Nullable && (jvalue == null || jvalue.Type == JTokenType.Null))
            {
                return new RlpItem(); 
            }

            return this.EncodeToRlp((jvalue as JValue).Value);
        }

        public JValue DecodeToJson(IRlpItem rlp)
        {
            return new JValue(this.DecodeFromRlp(rlp));
        }

        private int _maxLength = 0;
    }

    public class RlpHexStringKind : IRlpScalarKind
    {
        public string Name { get; set; }
        public bool Nullable { get; }

        public Type SourceType
        {
            get { return typeof(string); }
        }

        public RlpHexStringKind(bool nullable = false, int byteLength = 0)
        {
            this.Nullable = nullable;
            this._byteLength = byteLength * 2;
        }

        public RlpHexStringKind(string name, bool nullable = false, int byteLength = 0) : this(nullable, byteLength)
        {
            this.Name = name;
        }

        public IRlpItem EncodeToRlp(dynamic obj)
        {
            if (obj.Length == 0 && this.Nullable)
            {
                return new RlpItem();
            }

            string value = obj is String ? obj : obj.ToString();

            if (value.IsHexString())
            {
                if (this._byteLength != 0 && value.Length - 2 > this._byteLength)
                {
                    throw new ArgumentException("Exceed the maximum byteLength");
                }

                if (value.ToString().ToLower().Substring(0, 2) == "0x")
                {
                    value = value.Replace("0x", String.Empty);
                }
                value = "0x" + value.TrimStart('0');
                return new RlpItem(value.ToBytes());
            }
            else
            {
                throw new ArgumentException("it's not hexstring");
            }
        }

        public dynamic DecodeFromRlp(IRlpItem rlp)
        {
            string data = (rlp as RlpItem).RlpData.ToHexString();

            if (this._byteLength == 0)
            {
                return data != "0x" ? data : "";
            }
            else if (data.Length - 2 <= this._byteLength)
            {
                if (this.Nullable && data.Replace("0x", String.Empty).Length == 0)
                {
                    data = "";
                }
                else
                {
                    data = "0x" + data.Replace("0x", String.Empty).PadLeft(this._byteLength, '0');
                }
                return data;
            }
            else
            {
                throw new ArgumentException("Exceed the maximum byteLength");
            }
        }

        public IRlpItem EncodeWithJson(JToken jvalue)
        {
            if(this.Nullable && (jvalue == null || jvalue.Type == JTokenType.Null))
            {
                return new RlpItem(); 
            }

            return this.EncodeToRlp((jvalue as JValue).Value);
        }

        public JValue DecodeToJson(IRlpItem rlp)
        {
            return new JValue(this.DecodeFromRlp(rlp));
        }

        private int _byteLength = 0;
    }

    public class RlpBytesKind : IRlpScalarKind
    {
        public string Name { get; set; }
        public bool Nullable { get; set; }

        public Type SourceType
        {
            get { return typeof(byte[]); }
        }

        public RlpBytesKind(bool nullable = false):this("",0,nullable){}
        public RlpBytesKind(int maxbytes = 0, bool nullable = false):this("",maxbytes,nullable)
        {
            this._maxBytes = maxbytes;
            this.Nullable = nullable;
        }

        public RlpBytesKind(string name, int maxbytes = 0, bool nullable = false)
        {
            this._maxBytes = maxbytes;
            this.Nullable = nullable;
            this.Name = name;
        }

        public IRlpItem EncodeToRlp(dynamic obj)
        {
            if(!this.Nullable)
            {
                if(obj == null || ((obj is byte[]) && (obj as byte[]).Length == 0) || ((obj is string) && (obj as string).Length == 0))
                {
                    return new RlpItem();
                }
            }

            if(obj is string && (obj as string).IsHexString())
            {
                if (this._maxBytes != 0 && obj.Length > this._maxBytes)
                {
                    throw new ArgumentException("Exceed the maximum maxbytes");
                }
                return new RlpItem((obj as string).ToBytes());
            }

            if (obj is byte[])
            {
                if (this._maxBytes != 0 && obj.Length > this._maxBytes)
                {
                    throw new ArgumentException("Exceed the maximum maxbytes");
                }
                return new RlpItem(obj);
            }
            else
            {
                throw new ArgumentException("invalid item type");
            }
        }

        public dynamic DecodeFromRlp(IRlpItem rlp)
        {
            if (this.Nullable && (rlp == null || rlp.RlpData.Length == 0))
            {
                return new byte[0];
            }

            if (this._maxBytes != 0 && rlp.RlpData.Length > this._maxBytes)
            {
                throw new ArgumentException("Exceed the maximum maxbytes");
            }
            return (rlp as RlpItem).RlpData;
        }

        public IRlpItem EncodeWithJson(JToken jvalue)
        {
            if(this.Nullable && (jvalue == null || jvalue.Type == JTokenType.Null))
            {
                return new RlpItem(); 
            }

            return this.EncodeToRlp((jvalue as JValue).Value);
        }

        public JValue DecodeToJson(IRlpItem rlp)
        {
            byte[] value = this.DecodeFromRlp(rlp);
            string hexStr = value.ToHexString();
            return new JValue(hexStr != "0x" ? hexStr : "");
        }

        private int _maxBytes = 0;
    }

    public class RlpArrayKind : IRlpArrayKind
    {
        public string Name { get; set; }
        public bool Nullable { get; set; }
        public IRlpKind ItemKind { get; }

        public RlpArrayKind(IRlpKind itemKind):this(itemKind,"",0,false)
        {
            this.ItemKind = itemKind;
        }

        public RlpArrayKind(IRlpKind itemKind, int maxLength) : this(itemKind,"",maxLength,false)
        {
            this._maxLength = maxLength;
        }

        public RlpArrayKind(IRlpKind itemKind, string name) : this(itemKind,name,0,false)
        {
            this.Name = name;
        }

        public RlpArrayKind(IRlpKind itemKind, string name, int maxLength,bool nullable)
        {
            this.ItemKind = itemKind;
            this.Name = name;
            this._maxLength = maxLength;
            this.Nullable = nullable;
        }

        public RlpArray EncodeToRlp(dynamic items)
        {
            if(this.Nullable && items == null){ return new RlpArray(); }
            string jsonStr = JsonConvert.SerializeObject(items,new JsonBytesConverter());
            if(jsonStr != "" && jsonStr != "{}" || jsonStr != "[]")
            {
                JArray jArray = JArray.Parse(jsonStr);
                return this.EncodeWithJson(jArray);
            }
            else
            {
                if(this.Nullable)
                {
                    return new RlpArray();
                }
                else
                {
                    throw new ArgumentException("the array is empty");
                }
            }
        }

        public dynamic DecodeFromRlp(IRlpItem rlp, Type type)
        {
            if (!type.IsArray) { throw new ArgumentException("the type isn't array"); }
            if(this.Nullable && (rlp == null || rlp.RlpData.Length == 0))
            {
                return default(Type);
            }
            JArray json = this.DecodeToJson(rlp);
            dynamic array = JsonConvert.DeserializeObject(json.ToString(), type,new JsonBytesConverter());
            return array;
        }

        public T[] DecodeFromRlp<T>(IRlpItem rlp)
        {
            JArray json = this.DecodeToJson(rlp);
            if(this.Nullable && (rlp == null || rlp.RlpData.Length == 0))
            {
                return default(T[]);
            }
            dynamic array = JsonConvert.DeserializeObject(json.ToString(), typeof(T[]),new JsonBytesConverter());
            return array;
        }

        public RlpArray EncodeWithJson(JToken items)
        {
            if(this.Nullable && (items == null || items.Type == JTokenType.Null)) { return new RlpArray(); }
            RlpArray rlpArray = new RlpArray();
            foreach (var item in (items as JArray))
            {
                if (this.ItemKind is IRlpScalarKind)
                {
                    IRlpItem rlpItem = (this.ItemKind as IRlpScalarKind).EncodeWithJson(item as JValue);
                    rlpArray.Add(rlpItem);
                }
                else if (this.ItemKind is IRlpArrayKind)
                {
                    if (item is JArray)
                    {
                        JArray jArray = item as JArray;
                        RlpArray rArray = (this.ItemKind as IRlpArrayKind).EncodeWithJson(jArray);
                        rlpArray.Add(rArray);
                    }
                    else
                    {
                        throw new ArgumentException("invalid item type");
                    }
                }
                else if (this.ItemKind is IRplStructKind)
                {
                    RlpArray rArray = (this.ItemKind as IRplStructKind).EncodeWithJson(item);
                    rlpArray.Add(rArray);
                }
                else if(this.ItemKind is IRlpCustomKind)
                {
                    IRlpItem rlpItem = (this.ItemKind as IRlpCustomKind).EncodeWithJson(item);
                    rlpArray.Add(rlpItem);
                }
            }
            return rlpArray;
        }

        public JArray DecodeToJson(IRlpItem rlp)
        {
            JArray jsonArray = new JArray();
            RlpArray list;
            if (rlp.RlpType == RlpType.Array)
            {
                list = (rlp as RlpArray);
            }
            else
            {
                list = (new RlpArray()).Decode(rlp.RlpData) as RlpArray;
            }

            if (this._maxLength != 0 && list.Count > this._maxLength)
            {
                throw new ArgumentException("Exceed the maximum maxLength");
            }

            foreach (IRlpItem rlpItem in list)
            {
                if (this.ItemKind is IRlpScalarKind)
                {
                    JValue jvalue = (this.ItemKind as IRlpScalarKind).DecodeToJson(rlpItem);
                    jsonArray.Add(jvalue);
                }
                else if (this.ItemKind is IRlpArrayKind)
                {
                    JArray items = (this.ItemKind as IRlpArrayKind).DecodeToJson(rlpItem);
                    jsonArray.Add(items);

                }
                else if (this.ItemKind is IRplStructKind)
                {
                    JToken item = (this.ItemKind as IRplStructKind).DecodeToJson(rlpItem);
                    jsonArray.Add(item);
                }
                else if(this.ItemKind is IRlpCustomKind)
                {
                    JToken item = (this.ItemKind as IRlpCustomKind).DecodeToJson(rlpItem);
                    jsonArray.Add(item);
                }
            }

            return jsonArray;
        }

        private int _maxLength = 0;
    }

    public class RlpStructKind : IRplStructKind
    {
        public string Name { get; set; }
        public bool Nullable { get; set; }

        public RlpStructKind(bool nullable = false)
        {
            this.Properties = new List<IRlpKind>();
            this.Nullable = nullable;
        }

        public RlpStructKind(string name, bool nullable = false) : this(nullable)
        {
            this.Name = name;
        }
        public List<IRlpKind> Properties { get; set; }

        public RlpArray EncodeToRlp(dynamic obj)
        {
            if(this.Nullable && obj == null) { return new RlpArray(); }
            RlpArray array = new RlpArray();
            string jsonStr = JsonConvert.SerializeObject(obj,new JsonBytesConverter());
            JObject jsonObj = JObject.Parse(jsonStr);
            array = this.EncodeWithJson(jsonObj);
            return array;
        }

        public dynamic DecodeFromRlp(IRlpItem rlp, Type type)
        {
            JToken json = this.DecodeToJson(rlp);
            dynamic value = JsonConvert.DeserializeObject(json.ToString(), type,new JsonBytesConverter());
            return value;
        }

        public RlpArray EncodeWithJson(JToken item)
        {
            if(this.Nullable && (item == null || item.Type == JTokenType.Null)) { return new RlpArray(); }
            RlpArray result = new RlpArray();

            foreach (IRlpKind kind in Properties)
            {
                if (kind is IRlpScalarKind)
                {
                    IRlpItem rlpItem = (kind as IRlpScalarKind).EncodeWithJson(item[kind.Name]);
                    result.Add(rlpItem);
                }
                else if (kind is IRlpArrayKind)
                {
                    RlpArray rArray = (kind as IRlpArrayKind).EncodeWithJson(item[kind.Name]);
                    result.Add(rArray);
                }
                else if (kind is IRplStructKind)
                {
                    RlpArray rArray = (kind as IRplStructKind).EncodeWithJson(item[kind.Name]);
                    result.Add(rArray);
                }
                else if(kind is IRlpCustomKind)
                {
                    IRlpItem rlpItem = (kind as IRlpCustomKind).EncodeWithJson(item[kind.Name]);
                    result.Add(rlpItem);
                }
            }
            return result;
        }

        public JToken DecodeToJson(IRlpItem rlp)
        {
            JObject json = new JObject();
            RlpArray rlpArray = new RlpArray();

            if (rlp.RlpType == RlpType.Array)
            {
                rlpArray = (rlp as RlpArray);
            }
            else
            {
                rlpArray = ((new RlpArray()).Decode(rlp.RlpData) as RlpArray);
            }

            if (this.Nullable && rlpArray.Count == 0)
            {
                return new JObject();
            }

            if (rlpArray.Count == Properties.Count)
            {
                for (int index = 0; index < rlpArray.Count; index++)
                {
                    IRlpKind kind = Properties[index];
                    if (kind is IRlpScalarKind)
                    {
                        JValue jvalue = (kind as IRlpScalarKind).DecodeToJson(rlpArray[index]);
                        json[kind.Name] = jvalue;
                    }
                    else if (kind is IRlpArrayKind)
                    {
                        var value = (kind as IRlpArrayKind).DecodeToJson(rlpArray[index]);
                        json[kind.Name] = value;
                    }
                    else if (kind is IRplStructKind)
                    {
                        var value = (kind as IRplStructKind).DecodeToJson(rlpArray[index]);
                        json[kind.Name] = value;
                    }
                    else if(kind is IRlpCustomKind)
                    {
                        var value = (kind as IRlpCustomKind).DecodeToJson(rlpArray[index]);
                        json[kind.Name] = value;
                    }
                }
            }
            else
            {
                throw new ArgumentException("Decode properties count not match properties count");
            }

            return json;
        }
    }
}