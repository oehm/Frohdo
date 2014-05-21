using UnityEngine;
using System.Collections;


public class DeleteObj : Command
{
    public GameObject obj;
    private int matLAyer;
    private Vector3 pos;

    public void freeResources()
    {
        if (obj.activeSelf == true)
        {
            GameObject.Destroy(obj);
        }
    }

    public void setUpCommand(GameObject o, int layerIndex)
    {
        obj = o;
        matLAyer = layerIndex;
    }

    public bool exectute()
    {
        obj.SetActive(false);

        Gridable htemp = obj.GetComponentsInChildren<Gridable>(true)[0];
        Vector3 pos = obj.transform.localPosition;
        CommandHelper.setMatrixForceOverride(matLAyer, pos, htemp, null);
        return true;
    }

    private void placeObject()
    {
        Gridable htemp = obj.GetComponentsInChildren<Gridable>(true)[0];
        Vector3 pos = obj.transform.position;
        CommandHelper.setMatrixForceOverride(matLAyer, pos, htemp, obj);
    }

    public void undo()
    {
        Debug.Log("undo!");
        obj.SetActive(true);
        placeObject();
    }

    public void redo()
    {
        exectute();
    }
}
