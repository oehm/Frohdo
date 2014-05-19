using UnityEngine;
using System.Collections;

public class VarnishObj : Command {

    private GameObject toVarnish;
    
    public void setUpCommand(GameObject obj)
    {
        toVarnish = obj;
    }
        
    public bool exectute()
    {
        Coatable v = toVarnish.GetComponent<Coatable>();      
        if(v == null)
        {
            return false;
        }
        v.coat();
        return true;
    }
    
    public void undo()
    {
        Coatable v = toVarnish.GetComponent<Coatable>();
        v.uncoat();      
    }
    
    public void redo()
    {
        exectute();
    }

    public void freeResources(){}
}
