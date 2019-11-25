using Confluent.Kafka;
using System;
using System.Collections.Generic;
using System.Text;

namespace ZZH.Kafka.Service
{
    public class JsonSerializer<T> : ISerializer<T> //where T : class
    {
        private readonly Encoding _encoding;
        public JsonSerializer()
        {
            _encoding = Encoding.UTF8;
        }
        public JsonSerializer(Encoding encoding)
        {
            _encoding = encoding;
        }
        public byte[] Serialize(T data, SerializationContext context)
            => _encoding.GetBytes(Newtonsoft.Json.JsonConvert.SerializeObject(data));
    }
}
