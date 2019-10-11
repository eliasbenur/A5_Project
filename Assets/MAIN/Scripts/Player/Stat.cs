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
    public float dashSpeed = 10;
    public float timeDash;
    public float BruitRadius = 1;
    public Power power = Power.None;
    [System.NonSerialized]
    public int nbKey =3;
    [HideInInspector]
    public int nbAntiCam = 3;
    [HideInInspector]
    public GameObject ObjAntiCamera;

    public Stat (int Speed, int Force, Power Pow)
    {
        speed = Speed;
        force = Force;
        power = Pow;
    }

#if UNITY_EDITOR

    [MenuItem("Assets/Create/Stat")]
    public static void CreateStat()
    {
        string path = EditorUtility.SaveFilePanelInProject("Save Stat", "New Stat", "Asset", "Save Stat", "Assets");
        if (path == "")
            return;
        AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<Stat>(), path);
    }

    [CustomEditor(typeof(Stat))]
    public class StatEditor : Editor
    {
        private Stat stat { get { return (target as Stat); } }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            EditorGUI.BeginChangeCheck();
            Power power = stat.power;
            EditorGUILayout.Space();
            switch (stat.power)
            {
                case Power.None:
                    break;
                case Power.AllKey:
                    stat.nbKey = EditorGUILayout.IntField("Key:", stat.nbKey);
                    break;
                case Power.CameraOff:
                    stat.nbAntiCam = EditorGUILayout.IntField("AntiCam:", stat.nbAntiCam);
                    var objAnt = stat.ObjAntiCamera;
                    stat.ObjAntiCamera = (GameObject)EditorGUILayout.ObjectField(objAnt, typeof(GameObject));
                    break;
            }
            
            
            if (EditorGUI.EndChangeCheck())
                EditorUtility.SetDirty(stat);
        }
    }
#endif

}
