﻿using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Xml;
using System;
namespace parser
{
    public class JsonReader
    {
        public String XmlToJson(XmlDocument xml) {
            Console.WriteLine("Parser started");
            String json = JsonConvert.SerializeObject(xml);
            JObject processor = JObject.Parse(json);
            JToken data = processor.SelectToken("$.JavaProject.data");
            string result = "{\"data\": "+data.ToString()+"}";
            Console.WriteLine(data);
            Console.WriteLine("Parser closed");
            return result;
        }
    }
}