using UnityEngine;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

public class XML_Loader : MonoBehaviour
{
    public static void Save(string path, Level level)
    {
        XmlSerializer serializer = new XmlSerializer(typeof(Level));
        using ( FileStream stream = new FileStream(path, FileMode.Create))
        {
            serializer.Serialize(stream, level);
        }
    }

    public static Level Load(string path)
    {
        XmlSerializer serializer = new XmlSerializer(typeof(Level));
        using(FileStream stream = new FileStream(path, FileMode.Open))
        {
            return serializer.Deserialize(stream) as Level;
        }
    }
   
} 
