using UnityEngine;
using System.Collections;

public class InsertObject : Command
{
    private GameObject obj;
    private GameObject layer;
    private int matLAyer;
    private Vector3 pos;

    public void freeResources()
    {
        GameObject.Destroy(obj);
    }

    public void setUpCommand(GameObject o, GameObject l, int layerIndex)
    {
        string color = "";
        Colorable colorable = o.GetComponent<Colorable>();
        if (colorable != null)
        {
            color = colorable.colorString;
        }
        obj = l.GetComponent<Layer>().AddLevelObjectByName(o.name, color, o.transform.localPosition);
        obj.layer = 8 + layerIndex;
        obj.SetActive(false);
        layer = l;
        matLAyer = layerIndex;
    }

    public void setUpCommand(LevelObjectXML o, Layer l, int layerIndex)
    {
        obj = l.GetComponent<Layer>().AddLevelObjectByName(o.name, o.color, o.pos.Vector2);
        obj.layer = 8 + layerIndex;
        obj.SetActive(false);
        layer = l.gameObject;
        matLAyer = layerIndex;
    }

    public bool exectute()
    {
        obj.SetActive(true);
        pos = obj.transform.position;
        obj.transform.parent = layer.transform;

        if (!placeObject())
        {
            GameObject.Destroy(obj);
            freeResources();
            return false;
        }
        return true;
    }

    private bool placeObject()
    {

        Gridable htemp = obj.GetComponent<Gridable>();
        Vector3 pos = obj.transform.localPosition;
        return CommandHelper.setMatrix(ref Editor_Grid.Instance.levelGrid[matLAyer], pos, htemp, obj);

    }

    public void undo()
    {
        obj.SetActive(false);

        Gridable htemp = obj.GetComponent<Gridable>();
        CommandHelper.setMatrixForceOverride(ref Editor_Grid.Instance.levelGrid[matLAyer], obj.transform.localPosition, htemp, null);
    }

    public void redo()
    {
        obj.SetActive(true);
        placeObject();
    }
}