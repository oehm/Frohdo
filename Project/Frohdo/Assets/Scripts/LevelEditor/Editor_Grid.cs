using UnityEngine;
using System.Collections;

public class Editor_Grid
{
    private static Editor_Grid instance = null;
    public static Editor_Grid Instance
    {
        get
        {
            return instance;
        }
    }

    public GameObject[][][] levelGrid { get; set; }
    public Vector2[] planeSizes { get; private set; }

    public void initGrid(Vector2 size, GameObject level, GameObject layerBg_pref)
    {
        Vector2[] planeSizes = CalculatePlaneInFrustum.getPlaneSizes(size, Camera.main);

        levelGrid = new GameObject[planeSizes.Length][][];
        for (int i = 0; i < GlobalVars.Instance.LayerCount; i++)
        {
            GameObject bg = GameObject.Instantiate(layerBg_pref, new Vector3(0, 0, GlobalVars.Instance.layerZPos[i] + 0.02f), Quaternion.identity) as GameObject;
            bg.transform.localScale = planeSizes[i];
            bg.transform.parent = level.GetComponentsInChildren<Layer>()[i].transform;
            bg.layer = 14 + i;

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

