using UnityEngine;
using System.Collections;

public interface Command
{
    void exectute();
    void undo();
    void redo();
}


public class InsertObject : Command
{
    private GameObject pre;
    private GameObject obj;
    private GameObject layer;
    private Vector3 pos;
    
    public void setUpCommand(GameObject o, GameObject l)
    {
        pre = GameObject.Instantiate(o) as GameObject;
        pre.SetActive(false);
        layer = l;
    }
    
    public void exectute()
    {
        obj = GameObject.Instantiate(pre) as GameObject;
        obj.SetActive(true);
        pos = obj.transform.position;
        obj.transform.parent = layer.transform;
    }
    
    public void undo()
    {
         GameObject.Destroy(obj);
    }

    public void redo()
    {
        obj = GameObject.Instantiate(pre) as GameObject;
        obj.SetActive(true);
        obj.transform.position = pos;
        obj.transform.parent = layer.transform;
    }
}
