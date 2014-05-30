using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System;

public class PolygonCollider2DEditor : EditorWindow
{
    [MenuItem("Window/PolygonCollider2DEditor")]
    public static void ShowWindow()
    {
        //Show existing window instance. If one doesn't exist, make one.
        EditorWindow.GetWindow(typeof(PolygonCollider2DEditor));
    }

    private GameObject gameObject_;
    private List<List<Vector2>> collider_;


    void OnGUI()
    {

        gameObject_ = (GameObject)EditorGUILayout.ObjectField("gameObject or prefab", gameObject_, typeof(GameObject), true);

        if (gameObject_ != null)
        {
            PolygonCollider2D[] colliders = gameObject_.GetComponentsInChildren<PolygonCollider2D>(true);

            int i = 0;
            foreach (PolygonCollider2D collider in colliders)
            {
                EditorGUILayout.LabelField("PolygonCollider2D " + i);
                int length = EditorGUILayout.IntField("Length", collider.points.Length);
                if (length < 3) length = 3;


                Vector2[] temp ;
                if (length != collider.points.Length)
                {
                    temp = new Vector2[length];
                    for (int k = 0; k < length && k < collider.points.Length; k++)
                    {
                        temp[k] = collider.points[k];
                    }
                }
                else
                {
                    temp = collider.points.Clone() as Vector2[];
                }

                int j = 0;
                foreach (Vector2 point in temp)
                {
                    temp[j] = EditorGUILayout.Vector2Field("Vertex " + j, point);
                    j++;
                }

                colliders[i].points = temp;

                i++;
            }


            EditorUtility.SetDirty(gameObject_);
        }
    }

	// Use this for initialization
    //void Start () {
    //    PolygonCollider2D collider1 = gameObject.AddComponent<PolygonCollider2D>();

    //    Vector2[] vertices1 = {  new Vector2(0.5f, 0.5f),
    //                            new Vector2(0.5f, -0.6f),
    //                            new Vector2(0.2f, -0.9f),
    //                            new Vector2(-0.2f, -0.9f),
    //                            new Vector2(-0.5f, -0.6f),
    //                            new Vector2(-0.5f, 0.5f)

    //                         };

    //    collider1.points = vertices1;

    //    PolygonCollider2D collider2 = gameObject.AddComponent<PolygonCollider2D>();

    //    Vector2[] vertices2 = { new Vector2(0.2f, -0.9f),
    //                            new Vector2(0.0f, -1.0f),
    //                            new Vector2(-0.2f, -0.9f)

    //                         };

    //    collider2.points = vertices2;
    //}
	
    //// Update is called once per frame
    //void Update () {
	
    //}
}
