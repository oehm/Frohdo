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

[System.Serializable]
public class SerializableVector2
{
    public double X;
    public double Y;

    public Vector2 Vector2
    {
        get
        {
            return new Vector2((float)X, (float)Y);
        }
    }

    public SerializableVector2() { }
    public SerializableVector2(Vector2 vector)
    {
        double val;
        X = double.TryParse(vector.x.ToString(), out val) ? val : 0.0;
        Y = double.TryParse(vector.y.ToString(), out val) ? val : 0.0;
    }
}