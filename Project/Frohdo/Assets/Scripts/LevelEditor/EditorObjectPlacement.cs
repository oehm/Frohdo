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
    public int activeLayer = 2;


    public GameObject[][][] grids;

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
        curSelected.transform.position = getObjPosition();

    }
    private void makeGrid()
    {

        grids = new GameObject[planeSizes.Length][][];
        for (int i = 0; i < GlobalVars.Instance.LayerCount; i++)
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

        if (curSelected != null && isOnPlane(curSelected))
        {
            ObjHelper htemp = curSelected.GetComponent<ObjHelper>();
            Vector2 p = new Vector2(curSelected.transform.position.x, curSelected.transform.position.y);
            Vector2 para = new Vector2(level.GetComponentsInChildren<Layer>()[activeLayer].gameObject.transform.position.x, level.GetComponentsInChildren<Layer>()[activeLayer].gameObject.transform.position.y);
            p -= para;
            htemp.pos = p;
            curSelected.layer = 8 + activeLayer;

            InsertObject command = new InsertObject();
            command.setUpCommand(curSelected, level.GetComponentsInChildren<Layer>()[activeLayer].gameObject,this);
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

    public void updateObject(LevelObject levelObj)
    {
        if (curSelected != null)
        {
            DestroyImmediate(curSelected);
        }
        curSelected = Instantiate(LevelObjectController.Instance.GetPrefabByName(levelObj.name)) as GameObject;
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

    private bool isOnPlane(GameObject obj)
    {
        Vector3 pos = obj.transform.position;
        Vector3 scale = obj.transform.localScale;

        Vector3 layercenter = level.GetComponentsInChildren<Layer>()[activeLayer].gameObject.transform.position;

        Rect layerRec = new Rect(layercenter.x - planeSizes[activeLayer].x / 2, layercenter.y - planeSizes[activeLayer].y / 2, planeSizes[activeLayer].x, planeSizes[activeLayer].y);
        Rect objRec = new Rect(pos.x - scale.x / 2, pos.y - scale.y / 2, scale.x, scale.y);

        return (layerRec.xMin <= objRec.xMin && layerRec.xMax >= objRec.xMax && layerRec.yMin <= objRec.yMin && layerRec.yMax >= objRec.yMax);
    }


    public void updateXMLLevelObjects(LevelEditorParser l)
    {
        l.clear();
        Layer[] layer = level.GetComponentsInChildren<Layer>();
        for (int i = 0; i < layer.Length; i++)
        {
            GameObject obj = layer[i].gameObject;
            ObjHelper[] hs = obj.GetComponentsInChildren<ObjHelper>();
            foreach (ObjHelper h in hs)
            {
                LevelObject lobj = new LevelObject();
                lobj.color = h.color;
                lobj.name = h.Objname;
                lobj.pos = new SerializableVector2(h.pos);
                l.addLevelObject(i, lobj);
            }
        }
    }
}
