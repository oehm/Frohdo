using UnityEngine;
using System.Collections;

public class SCondition_CharacterSet : SCondition {

    public GameObject playLayer;

    public bool isFullfilled { get; set; }

    public void checkCondition()
    {
        Gridable[] posObjs = playLayer.GetComponentsInChildren<Gridable>();
        foreach(Gridable g in posObjs)
        {
            if(g.gameObject.name == "CharacterEditor")
            {
                if(g.gameObject.activeSelf)
                {
                    isFullfilled = true;
                    return;
                }
            }
        }
        isFullfilled = false;
    }
}
