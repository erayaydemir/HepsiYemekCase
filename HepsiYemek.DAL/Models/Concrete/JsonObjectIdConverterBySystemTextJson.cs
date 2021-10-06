using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace HepsiYemek.DAL.Models.Concrete
{
    public class JsonObjectIdConverterBySystemTextJson : JsonConverter<ObjectId>
    {
        public override ObjectId Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) => new ObjectId(JsonSerializer.Deserialize<string>(ref reader, options));

        public override void Write(Utf8JsonWriter writer, ObjectId value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString());
        }
    }
}
