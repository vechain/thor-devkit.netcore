using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Org.VeChain.Thor.Devkit.Extension
{
    public class JsonBytesConverter : JsonConverter<byte[]>
    {
        public override byte[] ReadJson(JsonReader reader, Type objectType, [AllowNull] byte[] existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            string value = (string)reader.Value;
            if(value.IsHexString())
            {
                return value.ToBytes();
            }
            else
            {
                throw new ArgumentException("the value isn't hexstring");
            }
        }

        public override void WriteJson(JsonWriter writer, [AllowNull] byte[] value, JsonSerializer serializer)
        {
            if(value == null)
            {
                writer.WriteNull();
            }
            else if(value.Length == 0)
            {
                writer.WriteValue("");
            }
            else
            {
                writer.WriteValue(value.ToHexString());
            }
        }
    }

    public static class JsonExtension
    {
        public static JObject SortProperties(this JObject jobj)
        {
            var sortJObject = new JObject();
            
            foreach(var property in jobj.Properties().ToList().OrderBy(p=>p.Name))
            {
                var value = property.Value as JObject;
                if(value != null){
                    value = value.SortProperties();
                    sortJObject.Add(property.Name,value);
                } else {
                    sortJObject.Add(property.Name,property.Value);
                }
            }
            return sortJObject;
        }
    }
}