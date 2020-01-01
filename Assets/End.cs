﻿using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using BayatGames.SaveGameFree;

public class End : MonoBehaviour
{
    public LayerMask playerMask;
    ObjectRemaning objectivesData;

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
