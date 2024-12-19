using System.IO;

namespace DodgeBlock.data.dotNet;

public static class XmlSerializer
{
    // méthode pour désérialiser un fichier XML vers un objet
    public static T Deserialization<T>(string filePath)
    {
        System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(T));
        using (StreamReader reader = new StreamReader(filePath))
        {
            return (T)serializer.Deserialize(reader);
        }
    }

    // méthode pour sérialiser un objet vers un fichier XML
    public static void Serialization<T>(T obj, string filePath)
    {
        System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(T));
        using (StreamWriter writer = new StreamWriter(filePath))
        {
            serializer.Serialize(writer, obj);
        }
    }
}
