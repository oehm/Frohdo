using UnityEngine;
using System.Collections;


public class DeleteObj : Command
{
    private GameObject obj;
    private int matLAyer;
    private Vector3 pos;

    public void freeResources()
    {
        GameObject.Destroy(obj);
    }

    public void setUpCommand(GameObject o, int layerIndex)
    {
        obj = o;
        matLAyer = layerIndex;
    }

    public bool exectute()
    {
        obj.SetActive(false);

        Gridable htemp = obj.GetComponent<Gridable>();
        Vector3 pos = obj.transform.localPosition;
        CommandHelper.setMatrixForceOverride(ref Editor_Grid.Instance.levelGrid[matLAyer], pos, htemp, null);
        return true;
    }

    private void placeObject()
    {
        Gridable htemp = obj.GetComponent<Gridable>();
        Vector3 pos = obj.transform.position;

        CommandHelper.setMatrixForceOverride(ref Editor_Grid.Instance.levelGrid[matLAyer], pos, htemp, obj);
    }

    public void undo()
    {
        obj.SetActive(true);
        placeObject();
    }

    public void redo()
    {
        exectute();
    }
}
