using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using BayatGames.SaveGameFree;

public class FTUE_Manager : MonoBehaviour
{

    public GameObject FTUE1_Prefab, FTUE2_Prefab, FTUE3_Prefab;
    public End EndScript;
    public Stat stat;

    public void Awake()
    {
        if (SaveGame.Exists("FTUE"))
        {
            bool ftueStarted = false;
            FTUE ftue_tmp = SaveGame.Load<FTUE>("FTUE");
            if (!ftue_tmp.FTUE_1_done)
            {
                Instantiate(FTUE1_Prefab, Vector3.zero, Quaternion.identity);
                Debug.Log("FTUE1 Started");
                EndScript.FTUE = true;
                stat.power = Power.Cook;
                ftueStarted = true;
            }

            if (!ftue_tmp.FTUE_2_done && !ftueStarted)
            {
                Instantiate(FTUE2_Prefab, Vector3.zero, Quaternion.identity);
                Debug.Log("FTUE2 Started");
                EndScript.FTUE = true;
                stat.power = Power.Cheater;
                ftueStarted = true;
            }

            if (!ftue_tmp.FTUE_3_done && !ftueStarted)
            {
                Instantiate(FTUE3_Prefab, Vector3.zero, Quaternion.identity);
                Debug.Log("FTUE3 Started");
                stat.power = Power.Hunter;
                EndScript.FTUE = true;
                ftueStarted = true;
            }
        }
        else
        {
            FTUE ftue = new FTUE();
            SaveGame.Save<FTUE>("FTUE", ftue);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void resetFTUE_DATA()
    {
        if (SaveGame.Exists("FTUE"))
        {
            FTUE ftue_tmp = SaveGame.Load<FTUE>("FTUE");
            ftue_tmp.FTUE_1_done = false;
            ftue_tmp.FTUE_2_done = false;
            ftue_tmp.FTUE_3_done = false;
            SaveGame.Save<FTUE>("FTUE", ftue_tmp);
        }
    }

    public void DebugFTUE_DATA()
    {
        if (SaveGame.Exists("FTUE"))
        {
            FTUE ftue_tmp = SaveGame.Load<FTUE>("FTUE");
            Debug.Log("FTUE 1: " + ftue_tmp.FTUE_1_done + " / FTUE 2: " + ftue_tmp.FTUE_2_done + " / FTUE 3: " + ftue_tmp.FTUE_3_done);

        }
    }
}

[ExecuteInEditMode]
[CustomEditor(typeof(FTUE_Manager))]
public class CustomInspectorResetFTUE : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        FTUE_Manager g = (FTUE_Manager)target;
        if (GUILayout.Button("ResetFTUE"))
        {
            g.resetFTUE_DATA();
        }
        if (GUILayout.Button("DebugFTUE_Data"))
        {
            g.DebugFTUE_DATA();
        }
    }

}
