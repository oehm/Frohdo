using UnityEngine;
using System.Collections;

public class LevelEditorParser : MonoBehaviour {

    public string savePath;
    private string levelName;
    private LevelXML level;
    
	void Start () {
        level = new LevelXML();
        for(int i = 0; i< GlobalVars.Instance.LayerCount; i++)
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

    public void addLevelObject(int layerIndex, LevelObjectXML obj)
    {
        //if(layerIndex >= level.layers.Count || layerIndex<= 0) return;
        level.layers[layerIndex].levelObjects.Add(obj);
    }

    public void clear()
    {
        for(int i=0; i< level.layers.Count; i++)
        {
            level.layers[i].levelObjects.Clear();
        }
    }
}
