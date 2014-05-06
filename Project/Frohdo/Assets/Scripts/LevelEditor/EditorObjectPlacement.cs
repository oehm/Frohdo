using UnityEngine;
using System.Collections;

public class EditorObjectPlacement : MonoBehaviour
{
    public GameObject gridPref;
    public GameObject level;
    public Camera cam;

    private bool ready = false;
    private Vector2[] planeSizes;
    private float[] depth;


    private bool[][][] grids;

    public void init(Vector2 size)
    {
        planeSizes = CalculatePlaneInFrustum.getPlaneSizes(size, cam);
        depth = new float[planeSizes.Length];
        depth[0] = GlobalVars.Instance.layerZPos[0];
        depth[1] = GlobalVars.Instance.layerZPos[1];
        depth[2] = GlobalVars.Instance.layerZPos[2];
        depth[3] = GlobalVars.Instance.layerZPos[3];
        depth[4] = GlobalVars.Instance.layerZPos[4];

        makeGrid();
        ready = true;
    }

    public void Update()
    {
        if (!ready) return;
    }
    private void makeGrid()
    {

        grids = new bool[planeSizes.Length][][];
        for (int i = 0; i < level.GetComponentsInChildren<Layer>().Length; i++)
        {
            GameObject upperBorder = Instantiate(gridPref, new Vector3(0, planeSizes[i].y / 2 + 0.5f, depth[i]), Quaternion.identity) as GameObject;
            upperBorder.transform.localScale = new Vector3(planeSizes[i].x+2,1,1);
            upperBorder.transform.parent = level.GetComponentsInChildren<Layer>()[i].transform;

            GameObject lowerBorder = Instantiate(gridPref, new Vector3(0, -planeSizes[i].y / 2 - 0.5f, depth[i]), Quaternion.identity) as GameObject;
            lowerBorder.transform.localScale = new Vector3(planeSizes[i].x+2, 1, 1);
            lowerBorder.transform.parent = level.GetComponentsInChildren<Layer>()[i].transform;

            GameObject leftBorder = Instantiate(gridPref, new Vector3(-planeSizes[i].x / 2 - 0.5f, 0, depth[i]), Quaternion.identity) as GameObject;
            leftBorder.transform.localScale = new Vector3(1, planeSizes[i].y, 1);
            leftBorder.transform.parent = level.GetComponentsInChildren<Layer>()[i].transform;

            GameObject rightBorder = Instantiate(gridPref, new Vector3(planeSizes[i].x / 2 + 0.5f, 0, depth[i]), Quaternion.identity) as GameObject;
            rightBorder.transform.localScale = new Vector3(1, planeSizes[i].y, 1);
            rightBorder.transform.parent = level.GetComponentsInChildren<Layer>()[i].transform;

            grids[i] = new bool[(int)planeSizes[i].x][];
            for (int x = 0; x < (int)planeSizes[i].x; x++)
            {
                grids[i][x] = new bool[(int)planeSizes[i].y];
                for (int y = 0; y < (int)planeSizes[i].y; y++)
                {
                    grids[i][x][y] = false;
                }
            }
        }
    }
}
