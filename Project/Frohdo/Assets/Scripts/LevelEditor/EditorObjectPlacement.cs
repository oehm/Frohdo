﻿using UnityEngine;
using System.Collections;

public class EditorObjectPlacement : MonoBehaviour
{
    public GameObject level;
    public GameObject gridPref;
    public GameObject marker;
    public EditCommandManager commandManager;
    public Gui_Main gui;

    private bool ready = false;
    private Vector2[] planeSizes;
    private float[] depth;

    private GameObject curSelected = null;
    private LevelObjectXML curLevelObject = null;
    private Vector2 mousePos = new Vector2(0, 0);
    public int activeLayer { get; set; }


    public GameObject[][][] grids { get; set; }

    public void init(Vector2 size)
    {
        planeSizes = CalculatePlaneInFrustum.getPlaneSizes(size, Camera.main);
        depth = new float[planeSizes.Length];
        depth[0] = GlobalVars.Instance.layerZPos[0];
        depth[1] = GlobalVars.Instance.layerZPos[1];
        depth[2] = GlobalVars.Instance.layerZPos[2];
        depth[3] = GlobalVars.Instance.layerZPos[3];
        depth[4] = GlobalVars.Instance.layerZPos[4];

        makeGrid();
        ready = true;
    }

    private bool mouseDown_ = false;
    private GameObject objMakred = null;

    void Awake()
    {
        marker = Instantiate(marker) as GameObject;
        marker.transform.parent = level.transform;
        marker.name = "MARKER";
        marker.SetActive(false);
    }
    
    void Start()
    {

    }

    void Update()
    {
        Transform activeLayerTransform = level.GetComponentsInChildren<Layer>()[GlobalVars.Instance.playLayer].transform;
        bool isCharacterSet = false;
        for (int i = 0; i < activeLayerTransform.childCount; i++)
        {
            if (activeLayerTransform.GetChild(i).name == "CharacterEditor")
            {
                if (activeLayerTransform.GetChild(i).gameObject.activeSelf)
                {
                    isCharacterSet = true;
                    break;
                }
            }
        }
        gui.characterSet = isCharacterSet;

        if (!ready || curLevelObject == null || objMakred != null) return;

        if (curLevelObject.name == "CharacterEditor" && gui.characterSet)
        {
            curSelected = null;
            curLevelObject = null;
            gui.deselectObj();
        }

        if (mouseDown_)
        {
            if (curSelected == null)
            {
                
                curSelected = Instantiate(LevelObjectController.Instance.GetPrefabByName(curLevelObject.name)) as GameObject;
                curSelected.transform.parent = level.transform;
                curSelected.name = curLevelObject.name;
                Colorable colorable = curSelected.GetComponentInChildren<Colorable>();
                if (colorable != null)
                {
                    colorable.colorIn(curLevelObject.color);
                }
                curSelected.transform.position = getObjPosition();
            }
            curSelected.transform.position = getObjPosition();
        }

    }
    private void makeGrid()
    {

        grids = new GameObject[planeSizes.Length][][];
        for (int i = 0; i < GlobalVars.Instance.LayerCount; i++)
        {
            GameObject bg = Instantiate(gridPref, new Vector3(0, 0, depth[i] + 0.02f), Quaternion.identity) as GameObject;
            bg.transform.localScale = planeSizes[i];
            bg.transform.parent = level.GetComponentsInChildren<Layer>()[i].transform;
            bg.layer = 14 + i;

            grids[i] = new GameObject[(int)planeSizes[i].x][];
            for (int x = 0; x < (int)planeSizes[i].x; x++)
            {
                grids[i][x] = new GameObject[(int)planeSizes[i].y];
                for (int y = 0; y < (int)planeSizes[i].y; y++)
                {
                    grids[i][x][y] = null;
                }
            }
        }
    }

    public void mouseDown()
    {
        //chekc if gui clicks on EditScreen
        if (gui.isMouseOnEditScreen(mousePos))
        {
            return;
        }


        if (gui.isMouseOnGui(mousePos))
        {
            objMakred = null;
            marker.SetActive(false);
            curSelected = null;
            gui.showEditMEnu(false);
            //gui.deselectObj();
            return;
        }
        ////check if user clicks on an object
        if (isOnPlane(mousePos))
        {
            Vector3 pos = Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, depth[activeLayer] - Camera.main.transform.position.z));
            int indexX = (int)(pos.x + planeSizes[activeLayer].x / 2);
            int indexY = (int)(pos.y + planeSizes[activeLayer].y / 2);
            if (grids[activeLayer][indexX][indexY] != null)
            {
                objMakred = grids[activeLayer][indexX][indexY];
                curSelected = null;
                curLevelObject = null;
                gui.deselectObj();
                gui.showEditMEnu(true);
                marker.transform.position = objMakred.transform.position + new Vector3(0, 0, -0.01f);
                marker.transform.localScale = objMakred.transform.localScale + new Vector3(0.4f, 0.4f, 0);
                marker.SetActive(true);
            }
            else
            {
                objMakred = null;
                gui.showEditMEnu(false);
                marker.SetActive(false);
            }
        }
        mouseDown_ = true;
    }
    public void mouseUp()
    {
        mouseDown_ = false;

        if (gui.isMouseOnGui(mousePos))
        {
            if (curSelected != null)
            {
                DestroyImmediate(curSelected);
                curSelected = null;
            }
            return;
        }

        if (curSelected != null && isOnPlane(curSelected))
        {
            Vector2 p = new Vector2(curSelected.transform.position.x, curSelected.transform.position.y);
            Vector2 para = new Vector2(level.GetComponentsInChildren<Layer>()[activeLayer].gameObject.transform.position.x, level.GetComponentsInChildren<Layer>()[activeLayer].gameObject.transform.position.y);
            p -= para;

            InsertObject command = new InsertObject();
            command.setUpCommand(curSelected, level.GetComponentsInChildren<Layer>()[activeLayer].gameObject, this,activeLayer);
            commandManager.executeCommand(command);
        }

        if (curSelected != null)
        {
            DestroyImmediate(curSelected);
            curSelected = null;
        }
    }

    public void mouseMove(Vector2 mousePos_)
    {
        mousePos = mousePos_;
    }

    public void updateObject(LevelObjectXML levelObj)
    {
        curLevelObject = levelObj;
    }

    public void updateColor(string color)
    {
        if (curLevelObject != null)
        {
            curLevelObject.color = color;
        }
    }

    public void changeColor(string color)
    {
        ChangeColor command = new ChangeColor();
        command.setUpCommand(objMakred, color);
        commandManager.executeCommand(command);
    }
    public void deleteObj()
    {
        DeleteObj command = new DeleteObj();
        command.setUpCommand(objMakred, this);
        commandManager.executeCommand(command);
        objMakred = null;
        marker.SetActive(false);
        gui.showEditMEnu(false);
    }

    public void setActiveLayer(int layer)
    {
        activeLayer = layer;
        curSelected = null;
        curLevelObject = null;
        gui.deselectObj();
    }

    private Vector3 getObjPosition()
    {
        Vector3 pos = Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, depth[activeLayer] - Camera.main.transform.position.z));
        pos.x = Mathf.Floor(pos.x) + curSelected.transform.localScale.x / 2;
        pos.y = Mathf.Floor(pos.y) + curSelected.transform.localScale.y / 2;

        return pos;
    }

    private bool isOnPlane(GameObject obj)
    {
        Vector3 pos = obj.transform.position;
        Vector3 scale = obj.transform.localScale;
        Vector3 layercenter = level.GetComponentsInChildren<Layer>()[activeLayer].gameObject.transform.position;

        Rect layerRec = new Rect(layercenter.x - planeSizes[activeLayer].x / 2, layercenter.y - planeSizes[activeLayer].y / 2, planeSizes[activeLayer].x, planeSizes[activeLayer].y);
        Rect objRec = new Rect(pos.x - scale.x / 2, pos.y - scale.y / 2, scale.x, scale.y);

        return (layerRec.xMin <= objRec.xMin && layerRec.xMax >= objRec.xMax && layerRec.yMin <= objRec.yMin && layerRec.yMax >= objRec.yMax);
    }

    private bool isOnPlane(Vector3 mPos)
    {
        Vector3 pos = Camera.main.ScreenToWorldPoint(new Vector3(mPos.x, mPos.y, depth[activeLayer] - Camera.main.transform.position.z));
        Vector2 p = new Vector2(pos.x, pos.y);
        Vector3 layercenter = level.GetComponentsInChildren<Layer>()[activeLayer].gameObject.transform.position;

        Rect layerRec = new Rect(layercenter.x - planeSizes[activeLayer].x / 2, layercenter.y - planeSizes[activeLayer].y / 2, planeSizes[activeLayer].x, planeSizes[activeLayer].y);

        return layerRec.Contains(p);
    }


    public void updateXMLLevelObjects(LevelEditorParser l)
    {
        l.clear();
        Layer[] layer = level.GetComponentsInChildren<Layer>();
        for (int i = 0; i < layer.Length; i++)
        {
            GameObject obj = layer[i].gameObject;
            Gridable[] hs = obj.GetComponentsInChildren<Gridable>();
            foreach (Gridable h in hs)
            {
                if (h.gameObject.name == "CharacterEditor")
                {
                    CharacterObjectXML character = new CharacterObjectXML();
                    character.pos = new SerializableVector2(h.gameObject.transform.localPosition);
                    l.addCharacter(i, character);
                    continue;
                }
                LevelObjectXML lobj = new LevelObjectXML();
                Colorable colorable = h.gameObject.GetComponentInChildren<Colorable>();
                if(colorable != null)
                {
                    lobj.color = colorable.colorString;
                }
                lobj.name = h.gameObject.name;
                lobj.pos = new SerializableVector2(h.gameObject.transform.localPosition);
                l.addLevelObject(i, lobj);
            }
        }
    }
}
