using UnityEngine;
using System.Collections;

public class LevelEditorParser : MonoBehaviour {

    public string savePath;

    private string levelName;
    private Level level;
    
	void Start () {
        level = new Level();
        foreach(Layer l in GetComponentsInChildren<Layer>()  )
        {
            level.layers.Add(new LayerXML());
        }
        int count = 0;
        foreach(LayerXML l in level.layers)
        {
            l.layerId = count;
            count++;
        }
    }

    public void setLevelName(string name)
    {
        levelName = name;
    }
    public void setLevelBackground(string color)
    {
        level.backgroundColor = color;
    }

    public void setSize(Vector2 size)
    {
        level.size = new SerializableVector2(size);
    }

    public void saveLevel()
    {
        XML_Loader.Save(Application.dataPath+savePath+levelName, level);
    }

	
}
