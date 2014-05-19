using UnityEngine;
using System.Collections;

public class SCondition_Varnishable : SCondition
{
    public GameObject selectedObject;
    
    public bool isFullfilled { get; set; }

    public void checkCondition()
    {
        if(selectedObject == null)
        {
            isFullfilled = false;
        }
        else
        {
            Coatable v =  selectedObject.GetComponentInChildren<Coatable>();
            if (v != null)
            {
                isFullfilled = true;
            }
            else
            {
                isFullfilled = false;
            }
        }
    }
}
