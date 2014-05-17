using UnityEngine;
using System.Collections;

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
        if (colorable != null)
        {
            previousColor = colorable.colorString;
        }
        color = c;
    }

    public bool exectute()
    {
        Colorable colorable = obj.GetComponent<Colorable>();
        if (colorable != null)
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
