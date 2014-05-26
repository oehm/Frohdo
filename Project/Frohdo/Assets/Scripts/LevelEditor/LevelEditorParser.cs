using UnityEngine;
using System.Collections;

public class LevelEditorParser
{

    private static LevelEditorParser instance = null;
    public static LevelEditorParser Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new LevelEditorParser();
            }
            return instance;
        }
    }

    public string savePath;
    
    private LevelXML level;
    

    void Start()
    {
        
    }

    public void initEmpty()
    {
        level = new LevelXML();
        for (int i = 0; i < GlobalVars.Instance.LayerCount; i++)
        {
            level.layers.Add(new LayerXML());
        }
        int count = 0;
        foreach (LayerXML l in level.layers)
        {
            l.layerId = count;
            count++;
        }
        SceneManager.Instance.levelToLoad.LevelTitle = "Enter Level Name";
    }

    public void setLevelBackground(string color)
    {
        level.backgroundColor = color;
    }

    public void saveLevel()
    {
        clear();
        updateXMLLevelObjects();

        System.IO.Directory.CreateDirectory(Application.dataPath + savePath + SceneManager.Instance.levelToLoad.LevelTitle);
        SceneManager.Instance.levelToLoad = new LevelAndType(Application.dataPath + savePath + SceneManager.Instance.levelToLoad.LevelTitle + "\\" + SceneManager.Instance.levelToLoad.LevelTitle + ".xml", LevelLoader.LevelType.Custom);
        SceneManager.Instance.levelToEdit = Application.dataPath + savePath + SceneManager.Instance.levelToLoad.LevelTitle + "\\" + SceneManager.Instance.levelToLoad.LevelTitle + ".xml";
        XML_Loader.Save(Application.dataPath + savePath + SceneManager.Instance.levelToLoad.LevelTitle + "\\" + SceneManager.Instance.levelToLoad.LevelTitle + ".xml", level);
    }

    public void addLevelObject(int layerIndex, LevelObjectXML obj)
    {
        //if(layerIndex >= level.layers.Count || layerIndex<= 0) return;
        level.layers[layerIndex].levelObjects.Add(obj);
    }

    public void addCharacter(int layerIndex, CharacterObjectXML character)
    {
        level.layers[layerIndex].Character = character;
    }

    private void clear()
    {
        for (int i = 0; i < level.layers.Count; i++)
        {
            level.layers[i].levelObjects.Clear();
        }
    }

    private void updateXMLLevelObjects()
    {
        Layer[] layer = GameObject.Find("SceneObjects").GetComponentsInChildren<Layer>();
        for (int i = 0; i < layer.Length; i++)
        {
            GameObject obj = layer[i].gameObject;
            Gridable[] hs = obj.GetComponentsInChildren<Gridable>();
            foreach (Gridable h in hs)
            {
                if (h.gameObject.name == "Character")
                {
                    CharacterObjectXML character = new CharacterObjectXML();
                    character.pos = new SerializableVector2(h.gameObject.transform.localPosition);
                    addCharacter(i, character);
                    continue;
                }
                LevelObjectXML lobj = new LevelObjectXML();
                Colorable colorable = h.gameObject.GetComponentInChildren<Colorable>();
                if (colorable != null)
                {
                    lobj.color = colorable.colorString;
                }
                lobj.name = h.gameObject.name;
                lobj.pos = new SerializableVector2(h.gameObject.transform.localPosition);
                addLevelObject(i, lobj);
            }
        }
    }
}
