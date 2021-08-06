using System;
using System.IO;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace VisionFlows
{
    public class XmlHelper
    {
        //此模式为程序使用到时再执行，如果写为公有字段形式，则再加载时就执行
        private static XmlHelper _instance = new XmlHelper();

        public static XmlHelper Instance()
        {
            return _instance;
        }

        public XmlHelper()
        {
            _instance = this;
        }

        public void SerializeToXml(string path, object obj)
        {
            XmlSerializer serializer = new XmlSerializer(obj.GetType());
            string content = string.Empty;
            //serialize
            using (StringWriter writer = new StringWriter())
            {
                serializer.Serialize(writer, obj);
                content = writer.ToString();
            }
            //save to file
            using (StreamWriter stream_writer = new StreamWriter(path))
            {
                stream_writer.Write(content);
            }
        }

        /// <summary>
        /// deserialize xml file to object
        /// </summary>
        /// <param name="path">the path of the xml file</param>
        /// <param name="object_type">the object type you want to deserialize</param>
        public object DeserializeFromXml(string path, Type object_type)
        {
            XmlSerializer serializer = new XmlSerializer(object_type);
            using (StreamReader reader = new StreamReader(path))
            {
                return serializer.Deserialize(reader);
            }
        }

   
    }
}