using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Grid : MonoBehaviour
{

    public GameObject gridTile;
    public GameObject block;
    public GameObject roof;
    public GameObject floor;
    public GameObject left;
    public GameObject right;
   
    
    private GameObject curBlock = null;
    private List<GameObject>[] blocks;
    private MeshRenderer blockRenderer;

    public Material gridTileIdle;
    public Material gridTileOkay;
    public Material gridTileBad;

    public Camera cam;

    private bool rightDown=false;
    private bool validPos = false;
    private int hitIndex = 0;

    private Material[] gridTiles;
    private GameObject[] grid;

    private int curId=0;

    int[][] state;
    // Use this for initialization
    void Start()
    {
        grid = new GameObject[100];
        gridTiles = new Material[3];
        gridTiles[0] = gridTileIdle;
        gridTiles[1] = gridTileOkay;
        gridTiles[2] = gridTileBad;

        state = new int[3][];
        state[0] = new int[100];
        state[1] = new int[100];
        state[2] = new int[100];

        blocks = new List<GameObject>[3];
        blocks[0] = new List<GameObject>();
        blocks[1] = new List<GameObject>();
        blocks[2] = new List<GameObject>();

        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                grid[i+j*10] = Instantiate(gridTile, new Vector3(-4.5f+i, -4.5f+j, -1.0f),Quaternion.identity) as GameObject;
                grid[i + j * 10].name = (i + j * 10).ToString();
                state[0][i + j * 10] = 0;
                state[1][i + j * 10] = 0;
                state[2][i + j * 10] = 0;
            }
        }

    }

    // Update is called once per frame
    void Update()
    {
        GameObject hit = null;
        
        if (Input.GetMouseButtonDown(0))
        {
            rightDown = true;
            if (curBlock == null)
            {
                curBlock = Instantiate(block, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
                curBlock.name = "To Insert";
                blockRenderer = curBlock.GetComponent<MeshRenderer>();
                blockRenderer.enabled = false;
            }

        }
        if (Input.GetMouseButtonUp(0))
        {
            if (validPos && state[curId][hitIndex] != 2)
            {
                state[curId][hitIndex] = 2;
                curBlock.name = "Layer" + curId + "_" + hitIndex;
                blocks[curId].Add(curBlock);
                validPos = false;
                curBlock = null;
            }
            else
            {
                Destroy(curBlock);          
            }

            rightDown = false;
        }

        if (rightDown)
        {
            Ray r = cam.ScreenPointToRay(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));
            RaycastHit rInfo;
            validPos = false;
            if (Physics.Raycast(r, out rInfo))
            {
                blockRenderer.enabled = true;
                hit = rInfo.collider.gameObject;
                validPos = true;
                hitIndex = int.Parse(rInfo.collider.gameObject.name);
            }
         
        }
        
        for(int i=0; i<100;i++){
            if (grid[i] == hit && curBlock!=null)
            {
                curBlock.transform.position = hit.transform.position;
            };
            //grid[i].renderer.material = gridTiles[state[curId][i]];
        }
    }

    public void SetActiveLayer(int id)
    {
        curId = id;
        for (int i = 0; i < 3; i++)
        {
            foreach (GameObject g in blocks[i])
            {
                g.transform.position = new Vector3(g.transform.position.x, g.transform.position.y,i-1 -id);
            }
        }
        floor.transform.position = new Vector3(floor.transform.position.x,floor.transform.position.y,-id);
        roof.transform.position = new Vector3(roof.transform.position.x, roof.transform.position.y, -id);
        left.transform.position = new Vector3(left.transform.position.x, left.transform.position.y, -id);
        right.transform.position = new Vector3(right.transform.position.x, right.transform.position.y, -id);
    }
}
