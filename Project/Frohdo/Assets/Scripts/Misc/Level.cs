using UnityEngine;
using System.Collections.Generic;
using System.Xml.Serialization;

[XmlRoot("Level")]
public class Level
{
    [XmlArray("LevelObjects")]
    [XmlArrayItem("LevelObject")]
    public List<LevelObject> levelObjects = new List<LevelObject>();
    [XmlAttribute("LevelSize")]
    public SerializableVector2 levelSize;
    [XmlAttribute("PlayerPosition")]
    public SerializableVector2 playerStartPos;
    [XmlAttribute("BackgroundColor")]
    public string backgroundColor;
}