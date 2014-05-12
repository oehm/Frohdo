using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(Gridable))]
public class LevelObjectInspector : Editor
{
    SerializedProperty array;
    SerializedProperty width;
    SerializedProperty height;

    void OnEnable()
    {
        array = serializedObject.FindProperty("hitMat");
        width = serializedObject.FindProperty("width");
        height = serializedObject.FindProperty("height");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        
        EditorGUILayout.PropertyField(width);
        EditorGUILayout.PropertyField(height);

        array.arraySize = width.intValue;

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

        serializedObject.ApplyModifiedProperties();
    }
}