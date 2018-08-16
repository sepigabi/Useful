namespace MyHelpers
{
  using Newtonsoft.Json;
  using Newtonsoft.Json.Converters;
  using Newtonsoft.Json.Linq;
  using Newtonsoft.Json.Serialization;
  using System;
  using System.IO;
  
    public class MyJsonSerializer
    {
        public T Deserialize<T>(string json)
        {
            //var jsonInput = JObject.Parse(json);
            JsonSerializer jsonSerializer = new JsonSerializer()
            {
                DateParseHandling = DateParseHandling.None
            };
            JsonTextReader jsonTextReader = new JsonTextReader(new StringReader(json));
            T obj = jsonSerializer.Deserialize<T>(jsonTextReader);
            
            return obj;
        }

        public string Serialize(Object obj)
        {
            JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings()
            {
                NullValueHandling = NullValueHandling.Ignore,
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                DateParseHandling = DateParseHandling.DateTimeOffset,
            };
            jsonSerializerSettings.Converters.Add(new StringEnumConverter() { CamelCaseText = false });

            string jsonOutput = JsonConvert.SerializeObject(obj, Formatting.Indented, jsonSerializerSettings);
            return jsonOutput;
        }
    }
}
