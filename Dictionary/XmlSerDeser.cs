using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Dictionaries
{
    public static class XmlSerDeser
    {
        static XmlSerializer GetSerializer(string typeName)
        {
            switch (typeName)
            {
                case "DictCollection":
                    return new XmlSerializer(typeof(DictCollection));

                case "Dictionary":
                    return new XmlSerializer(typeof(Dictionary));
 
                case "Log":
                    return new XmlSerializer(typeof(Log));
            }
            throw new TypeAccessException("Error type name");
        }


        public static void Serialize(Object obj, string path)
        {
            XmlSerializer xmlSerializer = GetSerializer(obj.GetType().Name);

            using (FileStream fs = new FileStream(path, FileMode.Truncate))
            {
                xmlSerializer.Serialize(fs, obj);
            }
        }


        public static Object Deserialize(string typeName, string path)
        {
            XmlSerializer xmlSerializer = GetSerializer(typeName);

            using (FileStream fs = new FileStream(path, FileMode.Open))
            {
                return xmlSerializer.Deserialize(fs);
            }
        }

    }
}
