using UnityEngine;
using System.Collections;

public class Editor_Grid
{
    private static Editor_Grid instance = null;
    public static Editor_Grid Instance
    {
        get
        {
            if(instance == null)
            {
                instance = new Editor_Grid();
            }
            return instance;
        }
    }

    public GameObject[][][] levelGrid { get; set; }
    public Vector2[] planeSizes { get; private set; }
    public GameObject layerBg_pref;

    public void initGrid(Vector2 size)
    {
        planeSizes = CalculatePlaneInFrustum.getPlaneSizes(size, Camera.main);

        levelGrid = new GameObject[planeSizes.Length][][];
        for (int i = 0; i < GlobalVars.Instance.LayerCount; i++)
        {
            levelGrid[i] = new GameObject[(int)planeSizes[i].x][];
            for (int x = 0; x < (int)planeSizes[i].x; x++)
            {
                levelGrid[i][x] = new GameObject[(int)planeSizes[i].y];
                for (int y = 0; y < (int)planeSizes[i].y; y++)
                {
                    levelGrid[i][x][y] = null;
                }
            }
        }
    }
}

