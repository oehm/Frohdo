using UnityEngine;
using System.Collections;

[System.Serializable]
public class mArray
{
    public bool this[int i]
    {
        get { return arr[i]; }
        set { arr[i] = value; }

    }

    public bool[] arr;
}

public class Gridable : MonoBehaviour
{
    public int height;
    public int width;
    public mArray[] hitMat;

    public GameObject editorVersion = null;
    public bool[] availableInLayer;
}
