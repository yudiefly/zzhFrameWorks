using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace ZZH.Configuration.Yaml
{
    internal class YamlConfigurationProvider : FileConfigurationProvider
    {
        public YamlConfigurationProvider(FileConfigurationSource source) : base(source)
        {
        }

        public override void Load(Stream stream)
        {
            var parser = new YamlConfigurationFileParser();

            Data = parser.Parse(stream);
        }
    }
}
