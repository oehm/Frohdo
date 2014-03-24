using UnityEngine;
using System.Collections;

public class Box : LevelElements {

    void Start()
    {
        base.Init();
        quadMat[0][0] = true; 
    }
}
