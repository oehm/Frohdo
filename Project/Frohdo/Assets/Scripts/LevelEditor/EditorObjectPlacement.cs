using UnityEngine;
using System.Collections;

public class EditorObjectPlacement : MonoBehaviour
{

    public GameObject gridPref;
    public GameObject level;


    private GameObject[][][] grids;

    public void init(Vector2 size)
    {
        makeGrid(size);
    }
    private void makeGrid(Vector2 size)
    {
        grids = new GameObject[level.GetComponentsInChildren<Layer>().Length][][];
        for (int i = 0; i < level.GetComponentsInChildren<Layer>().Length; i++)
        {
            GameObject layer = new GameObject("SelectionGrid" + (i + 1).ToString());
            layer.transform.parent = level.GetComponentsInChildren<Layer>()[i].transform;
            layer.tag = "Grid";

            grids[i] = new GameObject[(int)size.x][];
            for (int x = 0; x < size.x; x++)
            {
                grids[i][x] = new GameObject[(int)size.y];
                for (int y = 0; y < size.y; y++)
                {
                    grids[i][x][y] = Instantiate(gridPref, new Vector3(x, y, i), Quaternion.identity) as GameObject;
                    grids[i][x][y].layer = 14 + i;
                    grids[i][x][y].tag = "Grid";
                    grids[i][x][y].transform.parent = layer.transform;

                }
            }
        }
    }
}
