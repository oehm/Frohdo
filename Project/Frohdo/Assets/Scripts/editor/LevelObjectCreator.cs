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
        
    }

    private void createLevelObjectPrefab(){
        if (levelObjectController_ == null)
        {
            Debug.LogError("no levelObjectController set! prefab not created");
            return;
        }


        GameObject levelObject = GameObject.CreatePrimitive(PrimitiveType.Quad);//new GameObject(name_);//GameObject.Instantiate(emptyLevelObject_) as GameObject;

//add components and set stuff


//general
        //create new material and put it on there
        Material newMaterial = new Material(Shader.Find("Transparent/Diffuse"));
        newMaterial.SetTexture("_MainTex", texture_);
        AssetDatabase.CreateAsset(newMaterial, "Assets/Prefabs/LevelObjects/Test/Materials/" + name_ + ".mat");
        levelObject.GetComponent<MeshRenderer>().material = newMaterial;

        //set physLayer and tag
        levelObject.layer = physLayer_;
        levelObject.tag = "LevelObject";

//gridable
        //set scale
        levelObject.transform.localScale = new Vector3(width_, height_, 1.0f);

        //remove mesh collider and add collider2d (better collider tbd)
        DestroyImmediate(levelObject.GetComponent<Collider>());
        levelObject.AddComponent<BoxCollider2D>();

        createCollider();

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


        GameObject prefab = PrefabUtility.CreatePrefab("Assets/Prefabs/LevelObjects/Test/" + name_ + ".prefab", levelObject, ReplacePrefabOptions.ConnectToPrefab) as GameObject;

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

    private PolygonCollider2D createCollider()
    {
        //fill a bool array with the vertices to be set
        bool[][] bArray = new bool[hitMat_.Length + 1][];
        for (int x = 0; x < bArray.Length; x++)
        {
            bArray[x] = new bool[hitMat_[0].arr.Length + 1];
            for (int y = 0; y < bArray[x].Length; y++)
            {
                bool needsVertice = false;

                if (x > 0)
                {
                    if (y > 0)
                    {
                        if (hitMat_[x - 1].arr[y - 1]) needsVertice = true;
                    }
                    if (y < hitMat_[0].arr.Length - 1)
                    {
                        if (hitMat_[x - 1].arr[y + 1]) needsVertice = true;
                    }
                }

                if (x < hitMat_.Length - 1)
                {
                    if (y > 0)
                    {
                        if (hitMat_[x + 1].arr[y - 1]) needsVertice = true;
                    }
                    if (y < hitMat_[0].arr.Length - 1)
                    {
                        if (hitMat_[x + 1].arr[y + 1]) needsVertice = true;
                    }
                }

                bArray[x][y] = needsVertice;
            }
        }


        List<Vector2> vertices = new List<Vector2>();

        Vector2 p = new Vector2(0, 0);
        Vector2 s = new Vector2(0, 0);
        Vector2 dir = new Vector2( 1, 0);

        for (int x = 0; x < bArray.Length && vertices.Count == 0; x++)
        {
            for (int y = 0; y < bArray[x].Length; y++)
            {
                if (bArray[x][y])
                {
                    p = s =  new Vector2(x, y);
                    vertices.Add(p);
                    break;
                }
            }
        }

        do
        {

        } while (!p.Equals(s));

        return null;
    }

}
