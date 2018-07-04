using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Xml.Serialization;

namespace LindDotNetCore.Utils
{
    /// <summary>
    /// 序列化过滤器特性标识
    /// </summary>
    [AttributeUsageAttribute(AttributeTargets.Property)]
    public abstract class SerializationFilterAttribute : Attribute
    { }

    /// <summary>
    /// 序列化工具类
    /// </summary>
    public class SerializationHelper
    {
        #region XML

        /// <summary>
        /// XML序列化
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string SerializeToXml(object obj)
        {
            string s = "";
            using (MemoryStream ms = new MemoryStream())
            {
                try
                {
                    XmlSerializer serializer = new XmlSerializer(obj.GetType());
                    ms.Seek(0, SeekOrigin.Begin);
                    serializer.Serialize(ms, obj);
                    s = Encoding.ASCII.GetString(ms.ToArray());
                }
                catch (SerializationException e)
                {
                    Console.WriteLine("Failed to serialize. Reason: " + e.Message);
                    throw;
                }
            }
            return s;
        }

        /// <summary>
        /// XML返序列化
        /// </summary>
        /// <param name="type"></param>
        /// <param name="s"></param>
        /// <returns></returns>
        public static object DeserializeFromXml(Type type, string s)
        {
            return DeserializeFromXml<object>(s);
        }

        /// <summary>
        /// XML泛型反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="s"></param>
        /// <returns></returns>
        public static T DeserializeFromXml<T>(string s) where T : class, new()
        {
            var o = new T();

            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                o = serializer.Deserialize(new StringReader(s)) as T;
            }
            catch (SerializationException e)
            {
                throw new Exception("Failed to deserialize. Reason: " + e.Message);
            }
            return o;
        }

        #endregion XML

        #region Binary

        /// <summary>
        /// 二进制序列化
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static byte[] SerializeToBinary(object value)
        {
            BinaryFormatter serializer = new BinaryFormatter();
            MemoryStream memStream = new MemoryStream();
            memStream.Seek(0, 0);
            serializer.Serialize(memStream, value);
            return memStream.ToArray();
        }

        /// <summary>
        /// 二进制反序列化
        /// </summary>
        /// <param name="someBytes"></param>
        /// <returns></returns>
        public static object DeserializeFromBinary(byte[] someBytes)
        {
            IFormatter bf = new BinaryFormatter();
            object res = null;
            if (someBytes == null)
                return null;
            using (var memoryStream = new MemoryStream())
            {
                memoryStream.Write(someBytes, 0, someBytes.Length);
                memoryStream.Seek(0, 0);
                memoryStream.Position = 0;
                res = bf.Deserialize(memoryStream);
            }
            return res;
        }

        #endregion Binary

        #region JSON

        /// <summary>
        /// 字符串反序列化
        /// </summary>
        /// <param name="strBase64"></param>
        /// <returns></returns>
        public static T DeserializeFromJson<T>(string jsonStr)
        {
            var settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };
            return JsonConvert.DeserializeObject<T>(jsonStr);
        }

        /// <summary>
        /// 字符串序列化
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string SerializeToJson<T>(T obj, SerializationFilterAttribute filter = null)
        {
            var settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };
            if (filter != null)
            {
                dynamic d = new System.Dynamic.ExpandoObject();
                foreach (var item in typeof(T).GetProperties())
                {
                    if (item.GetCustomAttributes(false).Contains(filter))
                    {
                        (d as System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<string, object>>).Add(new System.Collections.Generic.KeyValuePair<string, object>(item.Name, item.GetValue(obj)));
                    }
                }
                return JsonConvert.SerializeObject(d);
            }
            else
            {
                return JsonConvert.SerializeObject(obj);
            }
        }

        #endregion JSON
    }
}