using UnityEngine;
using UnityEditor;
using System.Collections;
using System;

public class LevelObjectCreator : EditorWindow {
    [MenuItem("Window/LevelObjectCreator")]
    public static void ShowWindow()
    {
        //Show existing window instance. If one doesn't exist, make one.
        EditorWindow.GetWindow(typeof(LevelObjectCreator));
    }

    public GameObject emptyLevelObject_;

    //default values for convinience, values are not saved when window closes

    //general
    private string name_ = "";
    private Texture2D texture_;

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
        //general
        GUILayout.Label("general", EditorStyles.boldLabel);
        name_ = EditorGUILayout.TextField("Name", name_);
        texture_ = (Texture2D)EditorGUILayout.ObjectField("texture", texture_, typeof(Texture2D), false);
        

        //gridable
        GUILayout.Label("gridable", EditorStyles.boldLabel);
        width_ = EditorGUILayout.IntField("width", width_);
        height_ = EditorGUILayout.IntField("height", height_);

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
            Debug.Log("apllied: " + name_);
            createLevelObjectPrefab();

        }
        
    }

    private void createLevelObjectPrefab(){
        //GameObject levelObject = editorVersion_;//new GameObject();//GameObject.Instantiate(emptyLevelObject_) as GameObject;



        //UnityEngine.Object prefab = PrefabUtility.CreateEmptyPrefab("Assets/Prefabs/LevelObjects/Test/" + name_ + ".prefab");
        //PrefabUtility.ReplacePrefab(levelObject, prefab, ReplacePrefabOptions.ConnectToPrefab);
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
}
