using UnityEngine;
using System.Collections;

public class LevelElements : MonoBehaviour {

    public bool[][] quadMat;
    public int width;
    public int height;
    public GameObject prefab;

    protected void Init()
    {
        quadMat = new bool[width][];
        for (int i = 0; i < width; i++)
        {
            quadMat[i] = new bool[height];
        }
    }
}
