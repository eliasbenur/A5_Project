using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class Stat : ScriptableObject
{
    public float speed = 5;
    public float force = 5;


#if UNITY_EDITOR

    [MenuItem("Assets/Create/Stat")]
    public static void CreateStat()
    {
        string path = EditorUtility.SaveFilePanelInProject("Save Stat", "New Stat", "Asset", "Save Stat", "Assets");
        if (path == "")
            return;
        AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<Stat>(), path);
    }
#endif
}
