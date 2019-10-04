using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GetDataObj : MonoBehaviour
{
    public ObjectRemaning objRemaning;
    public List<Tresor> allObj = new List<Tresor>();
    
 

    public void GetData()
    {
        allObj.Clear();
        objRemaning?.obj.Clear();
        var t = FindObjectsOfType<Tresor>();
        foreach(Tresor tres in t)
        {
            allObj.Add(tres);
            objRemaning?.obj.Add(new stringAndBool(tres.name, false, tres.gameObject.GetComponent<SpriteRenderer>().sprite));
        }

    }
}

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

