using UnityEngine;
using System.Collections;

public class EditorObjectPlacement : MonoBehaviour
{
    public GameObject level;
    public GameObject gridPref;
    public EditCommandManager commandManager;

    private bool ready = false;
    private Vector2[] planeSizes;
    private float[] depth;

    private GameObject curSelected = null;
    private Vector2 mousePos = new Vector2(0, 0);
    private int activeLayer = 2;


    private GameObject [][][] grids;

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

    public void Update()
    {
        if (!ready) return;
        if (curSelected == null) return;
        if (isOnPlane(getObjPosition()))
        {
            curSelected.transform.position = getObjPosition();
        }

    }
    private void makeGrid()
    {

        grids = new GameObject[planeSizes.Length][][];
        for (int i = 0; i < level.GetComponentsInChildren<Layer>().Length; i++)
        {
            GameObject bg = Instantiate(gridPref, new Vector3(0, 0, depth[i]), Quaternion.identity) as GameObject;
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
        if (Gui_Main.isMouseOnGui(mousePos)) return;

        if (curSelected != null && isOnPlane(curSelected.transform.position))
        {
            ObjHelper htemp = curSelected.GetComponent<ObjHelper>();
            Vector2 p = new Vector2(curSelected.transform.position.x,curSelected.transform.position.y);
            Vector2 para = new Vector2(level.GetComponentsInChildren<Layer>()[activeLayer].gameObject.transform.position.x,level.GetComponentsInChildren<Layer>()[activeLayer].gameObject.transform.position.y);
            p-= para;
            htemp.pos = p;
            curSelected.layer = 8 + activeLayer;
            
            InsertObject command = new InsertObject();
            command.setUpCommand(curSelected, level.GetComponentsInChildren<Layer>()[activeLayer].gameObject);
            commandManager.executeCommand(command);
        }
        //try to select a obj
        else
        {

        }
    }
    public void mouseUp()
    {

    }

    public void mouseMove(Vector2 mousePos_)
    {
        mousePos = mousePos_;
    }

    public void selectObject(LevelObject levelObj)
    {
        if (curSelected != null)
        {
            DestroyImmediate(curSelected);
        }
        curSelected = Instantiate(LevelObjectController.Instance.GetPrefabByName(levelObj.name)) as GameObject;
        curSelected.AddComponent<ObjHelper>();
        ObjHelper htemp = curSelected.GetComponent<ObjHelper>();
        htemp.Objname = levelObj.name;
        htemp.color = levelObj.color;

        Color c = LevelObjectController.Instance.GetColor(levelObj.color);
        curSelected.GetComponentInChildren<Renderer>().material.color = c;
        curSelected.transform.position = getObjPosition();
    }

    public void setActiveLayer(int layer)
    {
        activeLayer = layer;
    }

    private Vector3 getObjPosition()
    {
        Vector3 pos = Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, depth[activeLayer] - Camera.main.transform.position.z));
        //If LAyers are paralax -> SNAP ON PARRALAX LAYERS!!
        pos.x = Mathf.Floor(pos.x) + curSelected.transform.localScale.x / 2;

        pos.y = Mathf.Floor(pos.y) + curSelected.transform.localScale.y / 2;

        return pos;
    }

    private bool isOnPlane(Vector3 pos)
    {
        float xp = pos.x;
        float yp = pos.y;
        Vector3 layercenter = level.GetComponentsInChildren<Layer>()[activeLayer].gameObject.transform.position;

        return (xp > layercenter.x - planeSizes[activeLayer].x / 2 && xp < layercenter.x + planeSizes[activeLayer].x / 2 && yp > layercenter.y - planeSizes[activeLayer].y / 2 && yp < layercenter.y + planeSizes[activeLayer].y / 2);
    }

    public void updateXMLLevelObjects(LevelEditorParser l)
    {
        l.clear();
        Layer[] layer = level.GetComponentsInChildren<Layer>();
        for(int i=0; i<layer.Length; i++)
        {
            GameObject obj = layer[i].gameObject;
            ObjHelper[] hs = obj.GetComponentsInChildren<ObjHelper>();
            foreach(ObjHelper h in hs)
            {
                LevelObject lobj =new LevelObject();
                lobj.color = h.color;
                lobj.name = h.Objname;
                lobj.pos = new SerializableVector2(h.pos);
                l.addLevelObject(i, lobj);
            }
        }
    }
}
