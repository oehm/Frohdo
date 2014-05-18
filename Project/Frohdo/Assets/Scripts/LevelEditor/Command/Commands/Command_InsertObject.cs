using UnityEngine;
using System.Collections;

public class InsertObject : Command
{
    private GameObject obj;
    private GameObject layer;
    private int matLAyer;

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
        if (layerIndex == 0)
        {
            moveToLayer(obj, LayerMask.NameToLayer("BackgroundDrawings"));
        }
        obj.SetActive(false);
        layer = l;
        matLAyer = layerIndex;
    }
    private void moveToLayer(GameObject root, int layer)
    {
        root.layer = layer;
        foreach (Transform child in root.transform)
            moveToLayer(child.gameObject, layer);
    }
    public void setUpCommand(LevelObjectXML o, Layer l, int layerIndex)
    {
        obj = l.GetComponent<Layer>().AddLevelObjectByName(o.name, o.color, o.pos.Vector2);
        if (layerIndex == 0)
        {
            moveToLayer(obj, LayerMask.NameToLayer("BackgroundDrawings"));
        }
        obj.SetActive(false);
        layer = l.gameObject;
        matLAyer = layerIndex;
    }

    public bool exectute()
    {
        obj.SetActive(true);
        obj.transform.parent = layer.transform;

        if (!placeObject())
        {
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