using UnityEngine;
using System.Collections;

public interface Command
{
    bool exectute();
    void undo();
    void redo();
    void freeResources();
}


public class InsertObject : Command
{
    private GameObject obj;
    private GameObject layer;
    private int matLAyer;
    private Vector3 pos;
    private EditorObjectPlacement editor;

    public void freeResources()
    {
        GameObject.Destroy(obj);
    }

    public void setUpCommand(GameObject o, GameObject l, EditorObjectPlacement e)
    {
        obj = GameObject.Instantiate(o) as GameObject;
        obj.SetActive(false);
        layer = l;
        editor = e;
        matLAyer = e.activeLayer;
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
        ObjHelper htemp = obj.GetComponent<ObjHelper>();
        Vector3 pos = obj.transform.position;
        //test if theres is an object
        int planeW = editor.grids[matLAyer].Length;
        int planeH = editor.grids[matLAyer][0].Length;
        for (int x = (int)pos.x + planeW / 2 - htemp.width / 2, xm = 0; x <= planeW / 2 + (int)pos.x; x++, xm++)
        {
            for (int y = (int)pos.y + planeH / 2 - htemp.height / 2, ym = 0; y <= planeH / 2 + (int)pos.y; y++, ym++)
            {
                if (htemp.hitMat[htemp.width * ym + xm] && editor.grids[matLAyer][x][y] != null)
                {
                    return false;
                }
            }
        }
        //if not 
        for (int x = (int)pos.x + planeW / 2 - htemp.width / 2, xm = 0; x <= planeW / 2 + (int)pos.x; x++, xm++)
        {
            for (int y = (int)pos.y + planeH / 2 - htemp.height / 2, ym = 0; y <= planeH / 2 + (int)pos.y; y++, ym++)
            {
                if (htemp.hitMat[htemp.width * ym + xm])
                {
                    editor.grids[matLAyer][x][y] = obj;
                }
            }
        }

        return true;

    }

    public void undo()
    {
        obj.SetActive(false);

        ObjHelper htemp = obj.GetComponent<ObjHelper>();
        Vector3 pos = obj.transform.position;
        int planeW = editor.grids[matLAyer].Length;
        int planeH = editor.grids[matLAyer][0].Length;
        for (int x = (int)pos.x + planeW / 2 - htemp.width / 2, xm = 0; x <= planeW / 2 + (int)pos.x; x++, xm++)
        {
            for (int y = (int)pos.y + planeH / 2 - htemp.height / 2, ym = 0; y <= planeH / 2 + (int)pos.y; y++, ym++)
            {
                if (htemp.hitMat[htemp.width * ym + xm])
                {
                    editor.grids[matLAyer][x][y] = null;
                }
            }
        }
    }

    public void redo()
    {
        obj.SetActive(true);
        placeObject();
    }
}

public class DeleteObj : Command
{
    private GameObject obj;
    private int matLAyer;
    private Vector3 pos;
    private EditorObjectPlacement editor;

    public void freeResources()
    {
        GameObject.Destroy(obj);
    }

    public void setUpCommand(GameObject o, EditorObjectPlacement e)
    {
        obj = o;
        editor = e;
        matLAyer = e.activeLayer;
    }

    public bool exectute()
    {
        obj.SetActive(false);

        ObjHelper htemp = obj.GetComponent<ObjHelper>();
        Vector3 pos = obj.transform.position;
        int planeW = editor.grids[matLAyer].Length;
        int planeH = editor.grids[matLAyer][0].Length;
        for (int x = (int)pos.x + planeW / 2 - htemp.width / 2, xm = 0; x <= planeW / 2 + (int)pos.x; x++, xm++)
        {
            for (int y = (int)pos.y + planeH / 2 - htemp.height / 2, ym = 0; y <= planeH / 2 + (int)pos.y; y++, ym++)
            {
                if (htemp.hitMat[htemp.width * ym + xm])
                {
                    editor.grids[matLAyer][x][y] = null;
                }
            }
        }
        return true;
    }

    private void placeObject()
    {
        ObjHelper htemp = obj.GetComponent<ObjHelper>();
        Vector3 pos = obj.transform.position;
        //test if theres is an object
        int planeW = editor.grids[matLAyer].Length;
        int planeH = editor.grids[matLAyer][0].Length;
        for (int x = (int)pos.x + planeW / 2 - htemp.width / 2, xm = 0; x <= planeW / 2 + (int)pos.x; x++, xm++)
        {
            for (int y = (int)pos.y + planeH / 2 - htemp.height / 2, ym = 0; y <= planeH / 2 + (int)pos.y; y++, ym++)
            {
                if (htemp.hitMat[htemp.width * ym + xm])
                {
                    editor.grids[matLAyer][x][y] = obj;
                }
            }
        }
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

    public bool exectute()
    {
        obj.GetComponent<Renderer>().material.color = color;

        return true;
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

