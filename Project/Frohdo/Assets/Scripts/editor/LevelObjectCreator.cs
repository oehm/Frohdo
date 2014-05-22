using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System;

public class LevelObjectCreator : EditorWindow {
    [MenuItem("Window/LevelObjectCreator")]
    public static void ShowWindow()
    {
        //Show existing window instance. If one doesn't exist, make one.
        EditorWindow.GetWindow(typeof(LevelObjectCreator));
    }

    //environment settings
    private LevelObjectController levelObjectController_;

    int[][] vArray;

    //default values for convinience, values are not saved when window closes

    //general
    private string name_ = "";
    private Texture2D texture_;
    private int physLayer_ = LayerMask.NameToLayer("Solids");

    //gridable
    private int height_ = 0;
    private int width_ = 0;
    private mArray[] hitMat_;
    private GameObject editorVersion_;
    private bool[] availableInLayer_ = new bool[4]; //HARDCODED VALUE HERE!!! no possible otherwise

    //colorable
    private bool isColorable_ = true;
    private string color_ = "W";

    //coatable
    private bool isCoatable_ = true;

    //vanishable
    private bool isVanishable_ = true;

    //pukable
    private bool isPukeable_ = true;
    private Pukeable.Behaviour pukeBehaviour_;

    //collectable
    private bool isCollectable_ = false;
    private Collectable.Behaviour collectBehaviour_;

    //usable
    private bool isUsableable_ = false;
    private Usable.Behaviour useBehaviour_;

    //deleting
    string deleteName_ = "";

    void OnGUI()
    {
        //environment settings
        GUILayout.Label("environment settings", EditorStyles.boldLabel);
        levelObjectController_ = (LevelObjectController)EditorGUILayout.ObjectField("levelObjectController", levelObjectController_, typeof(LevelObjectController), true);

        //general
        GUILayout.Label("general", EditorStyles.boldLabel);
        name_ = EditorGUILayout.TextField("Name", name_);
        texture_ = (Texture2D)EditorGUILayout.ObjectField("texture", texture_, typeof(Texture2D), false);
        physLayer_ = EditorGUILayout.LayerField("physic layer", physLayer_);


        //gridable
        GUILayout.Label("gridable", EditorStyles.boldLabel);
        width_ = EditorGUILayout.IntField("width", width_);
        height_ = EditorGUILayout.IntField("height", height_);

        if (height_ < 1)
        {
            height_ = 1;
        }

        if (width_ < 1)
        {
            width_ = 1;
            hitMat_ = new mArray[width_];
            hitMat_[0] = new mArray();
            hitMat_[0].arr = new bool[height_];

        }

        hitMat_ = ResizeArray(hitMat_, new int[] { width_, height_ });
        for (int y = 0; y < height_; y++)
        {
            GUILayout.BeginHorizontal("");
            for (int x = 0; x < width_; x++)
            {
                hitMat_[x].arr[y] = GUILayout.Toggle(hitMat_[x].arr[y], width_ < 10 ? x + ", " + y : "");
            }
            GUILayout.EndHorizontal();
        }

        
        GUILayout.Label("result", EditorStyles.boldLabel);
        if (GUILayout.Button("apply"))
        {
            vArray = createVMat(hitMat_);
        }
        if (vArray != null)
        {
            for (int y = 0; y < vArray[0].Length; y++)
            {
                GUILayout.BeginHorizontal("");
                for (int x = 0; x < vArray.Length; x++)
                {
                    EditorGUILayout.IntField(vArray[x][y], "");
                }
                GUILayout.EndHorizontal();
            }

        }



        editorVersion_ = (GameObject)EditorGUILayout.ObjectField("editor version", editorVersion_, typeof(GameObject), false);
        GUILayout.Label("available in layer", EditorStyles.label);
        GUILayout.BeginHorizontal("");
        for (int x = 0; x < availableInLayer_.Length; x++)
        {
            availableInLayer_[x] = GUILayout.Toggle(availableInLayer_[x], ""+x);
        }
        GUILayout.EndHorizontal();


        //colorable
        isColorable_ = EditorGUILayout.BeginToggleGroup("colorable", isColorable_);
            color_ = EditorGUILayout.TextField("color", color_);

            //coatable
            isCoatable_ = EditorGUILayout.Toggle("coatable", isCoatable_);

            //vanishable
            isVanishable_ = EditorGUILayout.Toggle("vanishable", isVanishable_);

            //pukable
            isPukeable_ = EditorGUILayout.BeginToggleGroup("pukable", isPukeable_);
                pukeBehaviour_ = (Pukeable.Behaviour)EditorGUILayout.EnumPopup("behaviour", pukeBehaviour_);
            EditorGUILayout.EndToggleGroup();

        EditorGUILayout.EndToggleGroup();

        //collectable
        isCollectable_ = EditorGUILayout.BeginToggleGroup("collectable", isCollectable_);
        collectBehaviour_ = (Collectable.Behaviour)EditorGUILayout.EnumPopup("behaviour", collectBehaviour_);
        EditorGUILayout.EndToggleGroup();

        //usable
        isUsableable_ = EditorGUILayout.BeginToggleGroup("usable", isUsableable_);
        useBehaviour_ = (Usable.Behaviour)EditorGUILayout.EnumPopup("behaviour", useBehaviour_);
        EditorGUILayout.EndToggleGroup();

        //apply
        if(GUILayout.Button("apply"))
        {
            createLevelObjectPrefab();

        }

        GUILayout.Space(50.0f);
        GUILayout.Label("delete", EditorStyles.label);
        deleteName_ = EditorGUILayout.TextField("Name", deleteName_);
        if (GUILayout.Button("delete"))
        {
            deleteLevelObjectPrefab(deleteName_);

        }
    }

    private void deleteLevelObjectPrefab(string name)
    {
        GameObject levelObject = levelObjectController_.GetPrefabByName(name);

        levelObjectController_.levelObjectPrefabs_.Remove(levelObject);


        if (levelObject == null) return;

        AssetDatabase.DeleteAsset("Assets/Prefabs/LevelObjects/Final/" + name);
    }

    private void createLevelObjectPrefab(){
        if (levelObjectController_ == null)
        {
            Debug.LogError("no levelObjectController set! prefab not created");
            return;
        }


        GameObject levelObject = GameObject.CreatePrimitive(PrimitiveType.Quad);//new GameObject(name_);//GameObject.Instantiate(emptyLevelObject_) as GameObject;

        AssetDatabase.CreateFolder("Assets/Prefabs/LevelObjects/Final", name_);

//add components and set stuff


//general
        //create new material and put it on there
        Material newMaterial = new Material(Shader.Find("Transparent/BetterDiffuse"));
        newMaterial.SetTexture("_MainTex", texture_);
        AssetDatabase.CreateAsset(newMaterial, "Assets/Prefabs/LevelObjects/Final/" + name_ + "/" + name_ + ".mat");
        levelObject.GetComponent<MeshRenderer>().material = newMaterial;

        //set physLayer and tag
        levelObject.layer = physLayer_;
        levelObject.tag = "LevelObject";

//gridable
        //set scale
        levelObject.transform.localScale = new Vector3(width_, height_, 1.0f);

        //remove mesh collider and add collider2d (better collider tbd)
        DestroyImmediate(levelObject.GetComponent<Collider>());
        //levelObject.AddComponent<BoxCollider2D>();

        createCollider(levelObject);

        //add gridable
        Gridable gridable = levelObject.AddComponent<Gridable>();
        gridable.width = width_;
        gridable.height = height_;
        gridable.hitMat = hitMat_;
        gridable.editorVersion = editorVersion_;
        gridable.availableInLayer = availableInLayer_;

//colorable
        if (isColorable_)
        {
            //add colorable
            Colorable colorable = levelObject.AddComponent<Colorable>();
            colorable.setColorStringUnityEditor(color_, levelObjectController_);

            //add coatable
            if (isCoatable_)
            {
                levelObject.AddComponent<Coatable>();
            }

            //add vanishable
            if (isVanishable_)
            {
                levelObject.AddComponent<Vanishable>();
            }
        }

//pukable
        if (isPukeable_)
        {
            Pukeable pukeable = levelObject.AddComponent<Pukeable>();
            pukeable.behaviour_ = pukeBehaviour_;
        }

//collectable
        if (isCollectable_)
        {
            Collectable collectable = levelObject.AddComponent<Collectable>();
            collectable.behaviour_ = collectBehaviour_;
        }

//usable
        if (isUsableable_)
        {
            Usable usable = levelObject.AddComponent<Usable>();
            usable.behaviour_ = useBehaviour_;
        }

        try
        {
            if (levelObjectController_.GetPrefabByName(name_) != null)
            {
                deleteLevelObjectPrefab(name_);
            }

        }
        catch (Exception e)
        {

        }

        GameObject prefab = PrefabUtility.CreatePrefab("Assets/Prefabs/LevelObjects/Final/" + name_ + "/" + name_ + ".prefab", levelObject, ReplacePrefabOptions.ConnectToPrefab) as GameObject;

        levelObjectController_.levelObjectPrefabs_.Add(prefab);


        EditorUtility.SetDirty(levelObjectController_);

        DestroyImmediate(levelObject);
        AssetDatabase.Refresh();


        Debug.Log("created: " + name_);

    }


    private mArray[] ResizeArray(mArray[] arr, int[] newSizes)
    {
        mArray[] temp = new mArray[newSizes[0]];

        for (int i = 0; i < newSizes[0]; i++ )
        {
            temp[i] = new mArray();
            temp[i].arr = new bool[newSizes[1]];
            if (i < arr.Length)
            {
                int length = arr[i].arr.Length <= temp[i].arr.Length ? arr[i].arr.Length : temp[i].arr.Length;

                Array.Copy(arr[i].arr, temp[i].arr, length);
            }
                //arr[i].arr.CopyTo(temp[i].arr, 0);
        }
        return temp;
    }

    private void createCollider(GameObject g)
    {
        //fill a bool array with the vertices to be set
        vArray = createVMat(hitMat_);

        PolygonCollider2D collider = g.AddComponent<PolygonCollider2D>();

        int count = 0;
        for (int x = 0; x < vArray.Length; x++)
        {
            for (int y = 0; y < vArray[x].Length; y++)
            {
                if (vArray[x][y] != 0)
                {
                    List<Vector2> vertices = new List<Vector2>();
                    //Debug.Log("Path:");

                    traverseHorizontal(x, y, vArray, vertices, new Vector2(x, y));
                    collider.pathCount = count + 1;

                    Vector2[] verticesArr = vertices.ToArray();

                    for(int i = 0; i < verticesArr.Length; i++) // (Vector2 vertex in verticesArr)
                    {
                        verticesArr[i].x /= width_;
                        verticesArr[i].y /= height_;

                        verticesArr[i].x -= 0.5f;
                        verticesArr[i].y -= 0.5f;

                        verticesArr[i].y *= -1.0f;


                    }

                    collider.SetPath(count, verticesArr);
                    count++;
                }
            }
        }
    }

    private void traverseHorizontal(int x, int y, int[][] arr, List<Vector2> result, Vector2 start)
    {
        //Debug.Log("traverseHorizontal at: " + x +" " + y);
        bool on = false;
        int xT = 0;

        int toAdd = -1;

        for (; xT < arr.Length; xT++) //traverse the line
        {
            if (on)//when on a line
            {
                if (xT == x)//and point reached: 
                {
                    break;//break with last saved
                }
                if (arr[xT][y] == 4)
                {
                    //arr[xT][y] = 2;
                    toAdd = xT;
                }
            }
            else//when not on an line
            {
                if (xT == x)//and point reached:
                {
                    for (xT++; xT < arr.Length && arr[xT][y] == 0; xT++) { }//traverse further until something found
                    if (arr[xT][y] == 2) arr[xT][y] = 4; //mark crosspoint
                    toAdd = xT;//save it
                    break;//and break with last saved
                }
                else//and point not reached
                {
                    if (arr[xT][y] != 0) toAdd = xT; //and something found: save it
                }
            }
            if (arr[xT][y] % 2 != 0) on = !on;//switch on line
        }

        Vector2 nextInOutline = new Vector2(toAdd, y);

        result.Add(nextInOutline);
        if ( nextInOutline != start)
        {
            traverseVertical(toAdd, y, arr, result, start);
        }
        arr[x][y]--;
    }

    private void traverseVertical(int x, int y, int[][] arr, List<Vector2> result, Vector2 start)
    {
        //Debug.Log("traverseVertical at: " + x + " " + y);
        bool on = false;
        int yT = 0;

        int toAdd = -1;
        for (; yT < arr[x].Length; yT++) //traverse the line
        {
            if (on)//when on a line
            {
                if (yT == y)//and point reached: 
                {
                    if (arr[x][yT] == 4)
                    {
                        arr[x][yT] = 2;
                        for (yT++; yT < arr[x].Length && arr[x][yT] == 0; yT++) { }//traverse further until something found
                        toAdd = yT;//save it
                        break;//and break with last saved
                    }
                    break;//break with last saved
                }
            }
            else//when not on an line
            {
                if (yT == y)//and point reached:
                {
                    for (yT++; yT < arr[x].Length && arr[x][yT] == 0; yT++) { }//traverse further until something found
                    toAdd = yT;//save it
                    break;//and break with last saved
                }
                else//and point not reached
                {
                    if (arr[x][yT] != 0) toAdd = yT; //and something found: save it
                }
            }
            if (arr[x][yT] % 2 != 0) on = !on;//switch on line
        }

        Vector2 nextInOutline = new Vector2(x, toAdd);

        result.Add(nextInOutline);
        if (nextInOutline != start)
        {
            traverseHorizontal(x, toAdd, arr, result, start);
        }
        arr[x][y]--;
    }


    private int[][] createVMat(mArray[] hitMat)
    {
        int[][] vMat = new int[hitMat.Length + 1][];
        for (int x = 0; x < vMat.Length; x++)
        {
            vMat[x] = new int[hitMat[0].arr.Length + 1];
            for (int y = 0; y < vMat[x].Length; y++)
            {
                vMat[x][y] = 0;
            }
        }
        for (int x = 0; x < hitMat.Length; x++)
        {
            for (int y = 0; y < hitMat[x].arr.Length; y++)
            {
                if (hitMat[x].arr[y])
                {
                    vMat[x  ][y  ] = 1;
                    vMat[x+1][y  ] = 1;
                    vMat[x  ][y+1] = 1;
                    vMat[x+1][y+1] = 1;

                    bool left = false;
                    bool up = false;

                    //check left
                    if (x > 0 && hitMat[x-1].arr[y])
                    {
                        vMat[x  ][y  ] = vMat[x  ][y  ] == 0 ? 1 : 0;
                        vMat[x  ][y+1] = vMat[x  ][y+1] == 0 ? 1 : 0;
                        left = true;
                    }

                    //check up
                    if (y > 0 && hitMat[x].arr[y-1])
                    {
                        vMat[x  ][y  ] = vMat[x  ][y  ] == 0 ? 1 : 0;
                        vMat[x+1][y  ] = vMat[x+1][y  ] == 0 ? 1 : 0;
                        up = true;
                    }

                    //check leftup
                    if (x > 0 && y > 0 && hitMat[x - 1].arr[y - 1])
                    {
                        if (left || up)
                        {
                            vMat[x  ][y  ] = vMat[x  ][y  ] == 0 ? 1 : 0;
                        }
                        else
                        {
                            vMat[x  ][y  ] = 2;
                        }
                    }

                    //check leftdown
                    if ( x > 0 && y < hitMat[x].arr.Length - 1 && hitMat[x - 1].arr[y + 1])
                    {
                        if (left)
                        {
                            vMat[x][y + 1] = vMat[x][y + 1] == 0 ? 1 : 0;
                        }
                        else
                        {
                            vMat[x][y + 1] = 2;
                        }
                    }
                }
            }
        }
        return vMat;
    }
}
