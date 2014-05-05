using UnityEngine;
using System.Collections;

public class EditorObjectPlacement : MonoBehaviour
{
    public GameObject gridPref;
    public GameObject level;
    public Camera cam;

    private bool ready = false;
    private Vector2[] planeSizes;
    private GameObject[][][] grids;

    public void init(Vector2 size)
    {
        planeSizes = CalculatePlaneInFrustum.getPlaneSizes(size, cam);        
        makeGrid();
        ready = true;
    }

    public void Update()
    {
        if (!ready) return;
    }
    private void makeGrid()
    {
        float []depth = new float[planeSizes.Length];
        depth[0] = GlobalVars.layer1Z;
        depth[1] = GlobalVars.layer2Z;
        depth[2] = GlobalVars.layer3Z;
        depth[3] = GlobalVars.layer4Z;
        depth[4] = GlobalVars.layer5Z;
        
        grids = new GameObject[planeSizes.Length][][];
        for (int i = 0; i < level.GetComponentsInChildren<Layer>().Length; i++)
        {
            GameObject layer = new GameObject("SelectionGrid" + (i + 1).ToString());
            layer.transform.parent = level.GetComponentsInChildren<Layer>()[i].transform;
            layer.tag = "Grid";

            grids[i] = new GameObject[(int)planeSizes[i].x][];
            for (int x = 0; x < (int)planeSizes[i].x; x++)
            {
                grids[i][x] = new GameObject[(int)planeSizes[i].y];
                for (int y = 0; y < (int)planeSizes[i].y; y++)
                {
                    grids[i][x][y] = Instantiate(gridPref, new Vector3(x - (int)planeSizes[i].x / 2, y - (int)planeSizes[i].y / 2, depth[i]), Quaternion.identity) as GameObject;
                    //grids[i][x][y].transform.localScale = new Vector3(0.75f, 0.75f, 1);
                    grids[i][x][y].layer = 14 + i;
                    grids[i][x][y].tag = "Grid";
                    grids[i][x][y].transform.parent = layer.transform;

                }
            }
        }
    }
}
