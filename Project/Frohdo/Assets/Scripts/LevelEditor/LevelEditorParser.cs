using UnityEngine;
using System.Collections;

public class LevelEditorParser : MonoBehaviour
{

    public string savePath;
    public EditCommandManager commandManger;
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

    public void loadLevel(string path)
    {
        level = XML_Loader.Load(path);
        int index1 = path.LastIndexOf('\\');
        int index2 = path.LastIndexOf('.');

        levelName = path.Substring(index1+1, index2 - index1-1);

        editObjPlament.init(level.size.Vector2);
        for (int i = 0; i < level.layers.Count; i++)
        {
            GameObject layerObject = gameObject.GetComponentsInChildren<Layer>()[i].gameObject;
            LayerXML layerXML = level.layers[i];

            for (int j = 0; j < layerXML.levelObjects.Count; j++)
            {
                LevelObjectXML levelObjectXML = layerXML.levelObjects[j];
                InsertObject command = new InsertObject();
                command.setUpCommand(levelObjectXML, layerObject.GetComponentInChildren<Layer>(), editObjPlament,i);
                commandManger.executeCommand(command);
            }

            CharacterObjectXML characterXML = layerXML.Character;
            if (characterXML != null)
            {               
                InsertObject command = new InsertObject();
                command.setUpCommand(LevelObjectController.Instance.getCharacter(true),layerObject,editObjPlament,i);
                commandManger.executeCommand(command);
            }
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
