using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using BayatGames.SaveGameFree;

public class End : MonoBehaviour
{
    public LayerMask playerMask;
    ObjectRemaning objectivesData;
    public bool FTUE;

    // Start is called before the first frame update
    void Start()
    {
        objectivesData = ObjectRefs.Instance.objectivesData;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (GameManager.Instance.objectiveDone)
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Is the Player
        if (((1 << collision.gameObject.layer) & playerMask) != 0)
        {
            //If Objective Completed
            if (GameManager.Instance.objectiveDone)
            {
                if (FTUE)
                {
                    if (collision.GetComponent<PlayerControl>().Get_inventory().Count >=3)
                    {

                        // Save FTUE
                        if (SaveGame.Exists("FTUE"))
                        {
                            bool ftueFound = false;
                            FTUE ftue_tmp_ = SaveGame.Load<FTUE>("FTUE");
                            if (!ftue_tmp_.FTUE_1_done)
                            {
                                ftue_tmp_.FTUE_1_done = true;
                                ftueFound = true;
                            }
                            if (!ftue_tmp_.FTUE_2_done && !ftueFound)
                            {
                                ftue_tmp_.FTUE_2_done = true;
                                ftueFound = true;
                            }
                            if (!ftue_tmp_.FTUE_3_done && !ftueFound)
                            {
                                ftue_tmp_.FTUE_3_done = true;
                                ftueFound = true;
                            }

                            SaveGame.Save<FTUE>("FTUE", ftue_tmp_);
                        }

                        for (int x = 0; x < objectivesData.obj.Count; x++)
                        {
                            for (int y = 0; y < collision.GetComponent<PlayerControl>().Get_inventory().Count; ++y)
                            {
                                if (objectivesData.obj[x].name == collision.GetComponent<PlayerControl>().Get_inventory()[y].name)
                                {
                                    objectivesData.obj[x].stolen = true;
                                    ObjectRefs.Instance.soungManager.PlaywinSnd();
                                }
                            }

                        }
                        //EditorUtility.SetDirty(objectivesData);

                        //Save System
                        SaveGame.Save<ObjectRemaning>("ObjectRemaining", objectivesData);

                        FTUE ftue_tmp = SaveGame.Load<FTUE>("FTUE");
                        //ObjectRefs.Instance.menuCanvas.GetComponent<LevelMenu_Manager>().Active_WinPanel();
                        if (ftue_tmp.FTUE_3_done)
                        {
                            ObjectRefs.Instance.menuCanvas.GetComponent<LevelMenu_Manager>().Active_WinPanel();
                        }
                        else
                        {
                            SceneManager.LoadScene("MAIN");
                        }
                        
                    }
                }
                else
                {
                    for (int x = 0; x < objectivesData.obj.Count; x++)
                    {
                        for (int y = 0; y < collision.GetComponent<PlayerControl>().Get_inventory().Count; ++y)
                        {
                            if (objectivesData.obj[x].name == collision.GetComponent<PlayerControl>().Get_inventory()[y].name)
                            {
                                objectivesData.obj[x].stolen = true;
                                ObjectRefs.Instance.soungManager.PlaywinSnd();
                            }
                        }

                    }
                    //EditorUtility.SetDirty(objectivesData);

                    //Save System
                    SaveGame.Save<ObjectRemaning>("ObjectRemaining", objectivesData);

                    ObjectRefs.Instance.menuCanvas.GetComponent<LevelMenu_Manager>().Active_WinPanel();
                }

            }
        }
    }
}
