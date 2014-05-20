using UnityEngine;
using System.Collections.Generic;
using System.Xml.Serialization;

[XmlRoot("Level")]
public class LevelXML
{
    //[XmlElement("Size")]
    //public SerializableVector2 size;
    [XmlElement("BackgroundColor")]
    public string backgroundColor;
    [XmlArray("Layers"), XmlArrayItem("Layer")]
    public List<LayerXML> layers = new List<LayerXML>();
}

public class LayerXML
{
    [XmlAttribute("LayerID")]
    public int layerId;
    [XmlArray("LevelObjects"), XmlArrayItem("LevelObject")]
    public List<LevelObjectXML> levelObjects = new List<LevelObjectXML>();
    public CharacterObjectXML Character;
}

public class LevelObjectXML
{
    public string name;
    public string color;
    public SerializableVector2 pos;
}

public class CharacterObjectXML
{
    public SerializableVector2 pos;
}