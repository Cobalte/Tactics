using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TerrainTools))]
public class TerrainToolsEditor : Editor
{
    public override void OnInspectorGUI() {
        serializedObject.Update();
        EditorGUILayout.PropertyField(serializedObject.FindProperty("terrainParent"), true);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("tileObject"), true);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("rows"), true);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("columns"), true);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("spacingX"), true);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("spacingY"), true);
        serializedObject.ApplyModifiedProperties();
        
        TerrainTools tools = (TerrainTools)target;
        if (GUILayout.Button("Create Layout")) {
            tools.CreateLayout();
        }
    }
}
