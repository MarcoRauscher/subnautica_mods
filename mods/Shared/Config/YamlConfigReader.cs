using System;
using YamlDotNet.Serialization;
using System.IO;

namespace SharedCode.Config
{
    public static class YamlConfigReader
    {
        public static T Readconfig<T>(string fullFilePath, Action<Exception> errorLogCallback)
        {
            var deserializer = new DeserializerBuilder().Build();

            try
            {
                var config = deserializer.Deserialize<T>(File.ReadAllText(fullFilePath));
                return config;
            }
            catch (Exception e)
            {
                errorLogCallback(e);
            }

            return default(T);

        }
    }
}
