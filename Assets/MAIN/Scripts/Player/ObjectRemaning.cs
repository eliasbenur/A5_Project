using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class ObjectRemaning : ScriptableObject
{
    public List<stringAndBool> obj = new List<stringAndBool>();

#if UNITY_EDITOR

    [MenuItem("Assets/Create/ObjectRemaning")]
    public static void CreateStat()
    {
        string path = EditorUtility.SaveFilePanelInProject("Save ObjectRemaning", "New ObjectRemaning", "Asset", "Save ObjectRemaning", "Assets");
        if (path == "")
            return;
        AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<ObjectRemaning>(), path);
    }
#endif
}
