using UnityEngine;
using System.Collections;

public class ObjectPlacement_Main : MonoBehaviour {


    public GameObject pre_grid;
    
    private GameObject[][][] grid;
    private GameObject[] layer;
    // Use this for initialization
	void Start () {
        initGrid();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    private void initGrid()
    {
        layer = new GameObject[GlobalVars.numberofLayers];
        grid = new GameObject[GlobalVars.numberofLayers][][];
        for (int i = 0; i < GlobalVars.numberofLayers; i++)
        {
            layer[i] = new GameObject("Layer_"+i.ToString());
            grid[i] = new GameObject[GlobalVars.levelWidth][];
            for (int x = 0; x < GlobalVars.levelWidth; x++)
            {
                grid[i][x] = new GameObject[GlobalVars.levelHeight];
                for (int y = 0; y < GlobalVars.levelHeight; y++)
                {
                    grid[i][x][y] = (GameObject)Instantiate(pre_grid, new Vector3(x, y, i), Quaternion.identity);
                    grid[i][x][y].transform.parent = layer[i].transform;
                    grid[i][x][y].layer = 8 + i;
                }
            }
        }
    }

}
