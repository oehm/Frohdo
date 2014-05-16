﻿using UnityEngine;
using System.Collections;

public class LevelEditorParser : MonoBehaviour
{

    public string savePath;
    public EditorObjectPlacement editObjPlament;

    public string levelName { get; set; }
    
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
        System.IO.Directory.CreateDirectory(Application.dataPath + savePath + levelName);
        SceneManager.Instance.levelToLoad = Application.dataPath + savePath + levelName + "\\" + levelName + ".xml";
        SceneManager.Instance.levelToEdit = Application.dataPath + savePath + levelName + "\\" + levelName + ".xml";
        XML_Loader.Save(Application.dataPath + savePath + levelName + "\\" + levelName + ".xml", level);
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

    public void clear()
    {
        for (int i = 0; i < level.layers.Count; i++)
        {
            level.layers[i].levelObjects.Clear();
        }
    }
}
