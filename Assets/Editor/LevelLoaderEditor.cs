using UnityEngine;
using System.Collections;
using UnityEditor;

/// <summary>
/// Clase que gestiona la creacióndel nivel desde el editor
/// </summary>
[CustomEditor(typeof(LevelLoader))]
public class LevelLoaderEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        LevelLoader levelLoader = (LevelLoader)target;

        // Creates the load level button.
        if (GUILayout.Button("Load Level"))
        {
            levelLoader.InicializarLevel();
        }

        // Creates the clear level button.
        if (GUILayout.Button("Clear Level"))
        {
            levelLoader.Clear();
        }
    }
}