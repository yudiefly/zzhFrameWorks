using Confluent.Kafka;
using System;
using System.Collections.Generic;
using System.Text;

namespace ZZH.Kafka.Service
{
    public class JsonDeserializer<T> : IDeserializer<T> where T : class
    {
        private readonly Encoding _encoding;
        public JsonDeserializer()
        {
            _encoding = Encoding.UTF8;
        }
        public JsonDeserializer(Encoding encoding)
        {
            _encoding = encoding;
        }

        public T Deserialize(ReadOnlySpan<byte> data, bool isNull, SerializationContext context)
            => Newtonsoft.Json.JsonConvert.DeserializeObject<T>(_encoding.GetString(data.ToArray()));
    }
}
