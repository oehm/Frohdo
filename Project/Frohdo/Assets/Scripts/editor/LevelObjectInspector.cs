using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(Gridable))]
public class LevelObjectInspector : Editor
{
    SerializedProperty array;
    SerializedProperty width;
    SerializedProperty height;
    SerializedProperty editorVersion;
    SerializedProperty availableInLayer;

    void OnEnable()
    {
        array = serializedObject.FindProperty("hitMat");
        width = serializedObject.FindProperty("width");
        height = serializedObject.FindProperty("height");

        editorVersion = serializedObject.FindProperty("editorVersion");
        availableInLayer = serializedObject.FindProperty("availableInLayer");
    }

    public override void OnInspectorGUI()
    {
        //base.OnInspectorGUI();

        serializedObject.Update();
        
        EditorGUILayout.PropertyField(width);
        EditorGUILayout.PropertyField(height);


        //EditorGUILayout.PropertyField(array,true);
        
        for (int x = 0; x < width.intValue; x++)
        {
            SerializedProperty node = array.GetArrayElementAtIndex(x);
            node.FindPropertyRelative("arr").arraySize = height.intValue;
        }
        array.arraySize = width.intValue;

        for (int y = 0; y < height.intValue; y++)       
        {
            GUILayout.BeginHorizontal("");
            for (int x = 0; x < width.intValue; x++)
            {
                array.GetArrayElementAtIndex(x).FindPropertyRelative("arr").GetArrayElementAtIndex(y).boolValue = GUILayout.Toggle(array.GetArrayElementAtIndex(x).FindPropertyRelative("arr").GetArrayElementAtIndex(y).boolValue, x.ToString() + "," + y.ToString());
            }
            GUILayout.EndHorizontal();
        }



        array.arraySize = width.intValue;

        availableInLayer.arraySize = GlobalVars.Instance.LayerCount;

        EditorGUILayout.PropertyField(editorVersion);
        
        GUILayout.BeginHorizontal("");
        for (int x = 0; x < availableInLayer.arraySize; x++)
        {
            availableInLayer.GetArrayElementAtIndex(x).boolValue = GUILayout.Toggle(availableInLayer.GetArrayElementAtIndex(x).boolValue, "Layer " + x.ToString());
        }
        GUILayout.EndHorizontal();

        serializedObject.ApplyModifiedProperties();
    }
}