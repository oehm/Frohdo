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
        Varnishable v = toVarnish.GetComponent<Varnishable>();
        if (v == null)
        {
            return false;
        }
        v.unvarnish();
        return true;
    }

    public void undo()
    {
        Varnishable v = toVarnish.GetComponent<Varnishable>();
        v.varnish();
    }

    public void redo()
    {
        exectute();
    }

    public void freeResources() { }
}
