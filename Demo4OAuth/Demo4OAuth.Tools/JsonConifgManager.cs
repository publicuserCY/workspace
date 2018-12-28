using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace Demo4OAuth.Tools
{
    /// <summary>
    /// Json配置工具
    /// </summary>
    public class JsonConifgManager
    {
        private IJsonConifg jsonConifg;

        public JsonConifgManager()
        {
            jsonConifg = new MessageConfig();
        }

        public JsonConifgManager(object instance)
        {
            jsonConifg = new MissionTemplateConfig(instance);
        }

        public string Read(string key)
        {
            return jsonConifg.Read(key);
        }

        public bool Write(string key, string value)
        {
            return jsonConifg.Write(key, value);
        }
    }

    internal interface IJsonConifg
    {
        string Read(string key);
        bool Write(string key, string value);
    }

    internal class MessageConfig : IJsonConifg
    {
        private const string DIRECTORY = "App_Data";
        private string dir;
        private string path;

        public MessageConfig()
        {
            dir = System.Web.Hosting.HostingEnvironment.MapPath("~/" + DIRECTORY);
            path = dir + "\\message.json";
        }

        Dictionary<string, string> All()
        {
            using (StreamReader streamReader = new StreamReader(path))
            {
                var result = new Dictionary<string, string>();
                try
                {
                    JsonSerializer serializer = new JsonSerializer();
                    serializer.NullValueHandling = NullValueHandling.Ignore;
                    JsonReader reader = new JsonTextReader(streamReader);
                    result = serializer.Deserialize<Dictionary<string, string>>(reader);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                return result;
            }
        }

        public string Read(string key)
        {
            var result = string.Empty;
            All().TryGetValue(key, out result);
            return result;
        }

        public bool Write(string key, string value)
        {
            var dic = All();
            if (dic.ContainsKey(key))
            {
                dic[key] = value;
            }
            else
            {
                dic.Add(key, value);
            }
            using (StreamWriter streamWriter = new StreamWriter(path, false, System.Text.Encoding.UTF8))
            {
                try
                {
                    JsonSerializer serializer = new JsonSerializer();
                    serializer.NullValueHandling = NullValueHandling.Ignore;
                    string json = JsonConvert.SerializeObject(dic, Formatting.Indented);
                    streamWriter.Write(json);
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }
    }

    internal class MissionTemplateConfig : IJsonConifg
    {
        private const string DIRECTORY = "App_Data";
        private string dir;
        private string path;
        private object instance;

        public MissionTemplateConfig(object instance)
        {
            dir = System.Web.Hosting.HostingEnvironment.MapPath("~/" + DIRECTORY);
            path = dir + "\\template.json";
            this.instance = instance;
        }
        Dictionary<string, string> All()
        {
            using (StreamReader streamReader = new StreamReader(path))
            {
                var result = new Dictionary<string, string>();
                try
                {
                    JsonSerializer serializer = new JsonSerializer();
                    serializer.NullValueHandling = NullValueHandling.Ignore;
                    JsonReader reader = new JsonTextReader(streamReader);
                    result = serializer.Deserialize<Dictionary<string, string>>(reader);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                return result;
            }
        }
        public string Read(string key)
        {
            var template = string.Empty;
            if (All().TryGetValue(key, out template))
            {
                template = ReflectTemplate(instance, template);
            }
            return template;
        }

        public bool Write(string key, string value)
        {
            throw new NotImplementedException();
        }

        private string ReflectTemplate(object obj, string template)
        {
            if (obj == null) { return string.Empty; }
            Type type = obj.GetType();
            PropertyInfo[] props = type.GetProperties(BindingFlags.Instance | BindingFlags.Public);
            foreach (var prop in props)
            {
                string placeholder = string.Format("{{{0}.{1}}}", type.Name, prop.Name);
                string value = prop.GetValue(obj) as string ?? string.Empty;
                template = template.Replace(placeholder, value);
            }
            return template;
        }
    }
}