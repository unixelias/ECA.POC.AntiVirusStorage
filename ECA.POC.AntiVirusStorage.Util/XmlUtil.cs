using System.IO;
using System.Xml.Serialization;

namespace ECA.POC.AntiVirusStorage.Util
{
    public static class XmlUtil
    {
        public static T DeserializarXml<T>(this string xml)
        {
            XmlSerializer serializador = new(typeof(T));
            using StringReader leitor = new(xml);
            return (T)serializador.Deserialize(leitor);
        }
    }
}