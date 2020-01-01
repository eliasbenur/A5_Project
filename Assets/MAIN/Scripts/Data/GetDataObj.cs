using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using BayatGames.SaveGameFree;

public class GetDataObj : MonoBehaviour
{
    public ObjectRemaning objRemaning;

    public void Start()
    {

    }


    public void GetData()
    {
        objRemaning?.obj.Clear();
        var t = FindObjectsOfType<Tresor>();
        foreach(Tresor tres in t)
        {
            objRemaning?.obj.Add(new stringAndBool(tres.name, false, tres.gameObject.GetComponent<SpriteRenderer>().sprite));
        }
        //EditorUtility.SetDirty(objRemaning);
        SaveGame.Save<ObjectRemaning>("ObjectRemaining", objRemaning);

    }
}

[ExecuteInEditMode]
[CustomEditor(typeof(GetDataObj))]
public class CustomInspectorGetData : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        //EditorGUILayout.LabelField("jnwludhbnw f");
        GetDataObj g = (GetDataObj)target;
        if (GUILayout.Button("SetData")) {
            g.GetData();
        }
    }

}

