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

    public void freeResources()
    {
        GameObject.Destroy(obj);
    }

    public void setUpCommand(GameObject o, GameObject l, int layerIndex)
    {
        string color = "";
        Colorable colorable = o.GetComponent<Colorable>();
        if(colorable != null)
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

        //test if theres is an object
        int pW = Editor_Grid.Instance.levelGrid[matLAyer].Length;
        int pH = Editor_Grid.Instance.levelGrid[matLAyer][0].Length;
        Vector2 objPos = new Vector2((int)(pos.x + (pW + 1) / 2), (int)(pos.y + (pH + 1) / 2));
        int w = (htemp.width + 1) / 2;
        int h = (htemp.height + 1) / 2;
        int w2 = htemp.width / 2;
        int h2 = htemp.height / 2;
        for (int x = (int)objPos.x - w2, xm = 0; x < (int)objPos.x + w; x++, xm++)
        {
            for (int y = (int)objPos.y - h2, ym = 0; y < (int)objPos.y + h; y++, ym++)
            {
                if (htemp.hitMat[xm][ym] && Editor_Grid.Instance.levelGrid[matLAyer][x][y] != null)
                {
                    return false;
                }
            }
        }

        //if not 
        for (int x = (int)objPos.x - w2, xm = 0; x < (int)objPos.x + w; x++, xm++)
        {
            for (int y = (int)objPos.y - h2, ym = 0; y < (int)objPos.y + h; y++, ym++)
            {
                if (htemp.hitMat[xm][ym])
                {
                    Editor_Grid.Instance.levelGrid[matLAyer][x][y] = obj;
                }
            }
        }

        return true;

    }

    public void undo()
    {
        obj.SetActive(false);

        Gridable htemp = obj.GetComponent<Gridable>();
        int pW = Editor_Grid.Instance.levelGrid[matLAyer].Length;
        int pH = Editor_Grid.Instance.levelGrid[matLAyer][0].Length;
        Vector2 objPos = new Vector2((int)(pos.x + (pW + 1) / 2), (int)(pos.y + (pH + 1) / 2));
        int w = (htemp.width + 1) / 2;
        int h = (htemp.height + 1) / 2;
        int w2 = htemp.width / 2;
        int h2 = htemp.height / 2;
        for (int x = (int)objPos.x - w2, xm = 0; x < (int)objPos.x + w; x++, xm++)
        {
            for (int y = (int)objPos.y - h2, ym = 0; y < (int)objPos.y + h; y++, ym++)
            {
                if (htemp.hitMat[xm][ym])
                {
                    Editor_Grid.Instance.levelGrid[matLAyer][x][y] = null;
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
        int pW = Editor_Grid.Instance.levelGrid[matLAyer].Length;
        int pH = Editor_Grid.Instance.levelGrid[matLAyer][0].Length;
        Vector2 objPos = new Vector2((int)(pos.x + (pW + 1) / 2), (int)(pos.y + (pH + 1) / 2));
        int w = (htemp.width + 1) / 2;
        int h = (htemp.height + 1) / 2;
        int w2 = htemp.width / 2;
        int h2 = htemp.height / 2;
        for (int x = (int)objPos.x - w2, xm = 0; x < (int)objPos.x + w; x++, xm++)
        {
            for (int y = (int)objPos.y - h2, ym = 0; y < (int)objPos.y + h; y++, ym++)
            {
                if (htemp.hitMat[xm][ym])
                {
                    Editor_Grid.Instance.levelGrid[matLAyer][x][y] = null;
                }
            }
        }
        return true;
    }

    private void placeObject()
    {
        Gridable htemp = obj.GetComponent<Gridable>();
        Vector3 pos = obj.transform.position;
        //test if theres is an object
        int pW = Editor_Grid.Instance.levelGrid[matLAyer].Length;
        int pH = Editor_Grid.Instance.levelGrid[matLAyer][0].Length;
        Vector2 objPos = new Vector2((int)(pos.x + (pW + 1) / 2), (int)(pos.y + (pH + 1) / 2));
        int w = (htemp.width + 1) / 2;
        int h = (htemp.height + 1) / 2;
        int w2 = htemp.width / 2;
        int h2 = htemp.height / 2;
        for (int x = (int)objPos.x - w2, xm = 0; x < (int)objPos.x + w; x++, xm++)
        {
            for (int y = (int)objPos.y - h2, ym = 0; y < (int)objPos.y + h; y++, ym++)
            {
                if (htemp.hitMat[xm][ym])
                {
                    Editor_Grid.Instance.levelGrid[matLAyer][x][y] = obj;
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
    private string color;
    private string previousColor;

    public void freeResources()
    {
    }

    public void setUpCommand(GameObject o, string c)
    {
        obj = o;
        Colorable colorable = obj.GetComponentInChildren<Colorable>();
        if(colorable != null)
        {
            previousColor = colorable.colorString;
        }
        color = c;
    }

    public bool exectute()
    {
        Colorable colorable = obj.GetComponent<Colorable>();
        if(colorable != null)
        {
            colorable.colorIn(color);
        }
        else
        {
            return false;
        }
        return true;
    }

    public void undo()
    {
        obj.GetComponentInChildren<Colorable>().colorIn(previousColor);
    }

    public void redo()
    {
        exectute();
    }
}

