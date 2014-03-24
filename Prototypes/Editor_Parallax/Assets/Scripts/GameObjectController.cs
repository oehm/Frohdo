using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameObjectController : MonoBehaviour {

    private Vector2 lastmousePos = new Vector2(0,0);
	private bool leftDown = false;
    private bool rightDown = false;
    
    public GameObject gridTile;
    public int gridResX = 10;
    public int gridResY = 10;
    public int gridSize = 1;
    public int layersNumb = 3;

    private List<GameObject> []sceneObjects;
    private GameObject prefab = null;
    private GameObject curObj = null;
    private MeshRenderer mRenderer = null;
    private GameObject hit = null;
    int[][] state;
    public int hitIndex = 0;
    bool isValid = false;

    private GameObject paraFixPoint;

    public int mainLayer = 1;
    private int activeLayer = 0;
    private bool[] visible;

    private float paraX = 0;
    private float offset = 0;
    public float paraForce = 1;

    private GameObject[] layers;


    //private bool leftDown = false;

    private GameObject[][] grids;
    // Use this for initialization
	void Start () {
        paraFixPoint = new GameObject("ParallaxFixPoint");
        InitGrid();
        paraX = paraFixPoint.transform.position.x;
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonDown(1))
        {
            rightDown = true;
        }
        if (Input.GetMouseButtonUp(1))
        {
            rightDown = false;
        }
        if (Input.GetMouseButtonDown(0))
        {
            leftDown = true;
            if (prefab != null)
            {
                curObj = Instantiate(prefab,new Vector3(0,0,0), Quaternion.identity) as GameObject;
                curObj.name = "ToInsert";
                mRenderer = curObj.GetComponent<MeshRenderer>();
                mRenderer.enabled = false;
            }
        }


        if (Input.GetMouseButtonUp(0) && curObj != null)
        {
            if (isValid && state[activeLayer][hitIndex] != 2)
            {
                state[activeLayer][hitIndex] = 2;
                curObj.name = prefab.name;
                curObj.transform.parent = layers[activeLayer].transform;
                sceneObjects[activeLayer].Add(curObj);
                isValid = false;
                curObj = null;
                mRenderer = null;
            }
            else
            {
                Destroy(curObj);
            }
            leftDown = false;
        }

        if (leftDown && prefab!=null && curObj != null)
        {
            Ray r = Camera.main.ScreenPointToRay(new Vector3(Input.mousePosition.x,Input.mousePosition.y,0));
            Vector3 origin = r.origin;
            origin.z = activeLayer;
            r.origin = origin;
            RaycastHit rInfo;
            isValid = false;
            if (Physics.Raycast(r,out rInfo,0.5f))
            {
                mRenderer.enabled = true;
                hitIndex = int.Parse(rInfo.collider.gameObject.name);
                hit = rInfo.collider.gameObject;
                isValid = true;
                
                curObj.transform.position = hit.transform.position;
            }
        }

        
        float camX = paraFixPoint.transform.position.x;
        for (int i = 0; i < layersNumb; i++)
        {
            if (i > mainLayer)
            {
                offset = Mathf.Pow(1.5f, (layersNumb - (i - mainLayer))) * paraForce;

                layers[i].transform.position = new Vector3((paraX - camX) / offset, layers[i].transform.position.y, layers[i].transform.position.z);
            }
            if (i < mainLayer)
            {
                offset = Mathf.Pow(1.5f, (layersNumb - (mainLayer - i))) * paraForce;
                layers[i].transform.position = new Vector3((camX - paraX) / offset, layers[i].transform.position.y, layers[i].transform.position.z);
            }
        }
    }

    void FixedUpdate(){
        //CameraDrag
        if (rightDown)
        {
            paraFixPoint.transform.position = paraFixPoint.transform.position - new Vector3(Input.mousePosition.x - lastmousePos.x, Input.mousePosition.y - lastmousePos.y, 0)/25;
        }
        lastmousePos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
    }

    public void setActiveLayer(int l)
    {
        activeLayer = l;
    }

    public void setVisible(bool[] v)
    {
        for (int i = 0; i < 3; i++)
        {
            if (visible[i] == v[i]) continue;
            MeshRenderer[] renderL = layers[i].GetComponentsInChildren<MeshRenderer>();
            foreach (MeshRenderer m in renderL)
            {
                if (v[i])
                {
                    m.gameObject.collider.enabled = true;
                    m.enabled = true;
                }
                else
                {
                    m.enabled = false;
                    m.gameObject.collider.enabled = false;
                }
            }
            v.CopyTo(visible,0);
        }
    }

    public void setElement(GameObject go)
    {
        prefab = go;
    }

    void InitGrid()
    {
        sceneObjects = new List<GameObject>[layersNumb];
        state = new int[layersNumb][];
        visible = new bool[3];
        layers = new GameObject[layersNumb];
        grids = new GameObject[layersNumb][];
        for (int i = 0; i < layersNumb; i++)
        {
            sceneObjects[i] = new List<GameObject>();
            visible[i] = true;
            layers[i] = new GameObject("Layer" + i);
            int GridResXnew = Mathf.Abs((mainLayer - i))*2  * gridResX;
            if (i == mainLayer) GridResXnew = gridResX;
            grids[i] = new GameObject[GridResXnew * gridResY];
            state[i] = new int[GridResXnew * gridResY];
            for (int y = 0; y < gridResY; y++)
            {
                for (int x = 0; x < GridResXnew; x++)
                {
                    state[i][x + y * GridResXnew] = 0;
                    grids[i][x + y * GridResXnew] = Instantiate(gridTile, new Vector3((-GridResXnew / 2 + x) * gridSize, gridSize * (-gridResY / 2 + y), i), Quaternion.identity) as GameObject;
                    grids[i][x + y * GridResXnew].transform.localScale = new Vector3(gridSize, gridSize, 0);
                    grids[i][x + y * GridResXnew].name = (x + y * GridResXnew).ToString();
                    grids[i][x + y * GridResXnew].transform.parent = layers[i].transform;
                }
            }
        }
        setActiveLayer(mainLayer);
    }
}
