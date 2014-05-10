using UnityEngine;
using System.Collections;

public interface Command
{
    void exectute();
    void undo();
    void redo();
    void freeResources();
}


public class InsertObject : Command
{
    private GameObject pre;
    private GameObject obj;
    private GameObject layer;
    private Vector3 pos;
    
    public void freeResources()
    {
        GameObject.Destroy(pre);
        Debug.Log(pre);
    }
    
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


public class ChangeColor : Command
{
    private GameObject obj;
    private Color color;
    private Color previousColor;

    public void freeResources()
    {
    }

    public void setUpCommand(GameObject o, string c)
    {
        obj = o;
        color = LevelObjectController.Instance.GetColor(c);
        previousColor = obj.GetComponent<Renderer>().material.color;
    }

    public void exectute()
    {
        obj.GetComponent<Renderer>().material.color = color;
    }

    public void undo()
    {
        obj.GetComponent<Renderer>().material.color = previousColor;
    }

    public void redo()
    {
        exectute();
    }
}

