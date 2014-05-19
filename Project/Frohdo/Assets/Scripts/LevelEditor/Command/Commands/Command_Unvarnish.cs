using UnityEngine;
using System.Collections;

public class UnvarnishObj: Command{

    private GameObject toVarnish;

    public void setUpCommand(GameObject obj)
    {
        toVarnish = obj;
    }

    public bool exectute()
    {
        Coatable v = toVarnish.GetComponent<Coatable>();
        if (v == null)
        {
            return false;
        }
        v.uncoat();
        return true;
    }

    public void undo()
    {
        Coatable v = toVarnish.GetComponent<Coatable>();
        v.coat();
    }

    public void redo()
    {
        exectute();
    }

    public void freeResources() { }
}
