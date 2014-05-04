using UnityEngine;
using System.Collections.Generic;
using System.Xml.Serialization;

[XmlRoot("Level")]
public class Level
{
    [XmlElement("Size")]
    public SerializableVector2 size;
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
    public List<LevelObject> levelObjects = new List<LevelObject>();
}