﻿using Rewired;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CharacterSelection_MenuManager : MonoBehaviour
{

    public string NameScenePlay = "Play";
    Player player;

    public ObjectRemaning objectRemaning;
    public GameObject panelObj;

    public Button default_Button;

    public Stat stat;

    private void Start()
    {
        player = ReInput.players.GetPlayer(0);

        if (panelObj != null && objectRemaning != null)
        {
            int j = 0;
            for (int i = 0; i < panelObj.transform.childCount; i++)
            {
                try
                {
                    /*  stringAndBool st = new stringAndBool("", true, null);
                      while(j< objectRemaning.obj.Count&& st.stolen)
                      {
                          st = objectRemaning.obj[j];
                          if (!st.stolen)
                              break;
                          else j++;
                      }*/

                    panelObj.transform.GetChild(i).GetChild(0).GetComponent<Image>().sprite =
                        objectRemaning.obj[i].sprite;
                    if (objectRemaning.obj[i].stolen) panelObj.transform.GetChild(i).GetChild(1).gameObject.SetActive(true);
                    // j++;
                }
                catch { }
            }
        }

        default_Button.Select();
    }

    public void LauchGame(int characterType)
    {
        switch (characterType)
        {
            case 1:
                stat.power = Power.CameraOff;
                break;
            case 2:
                stat.power = Power.AllKey;
                break;
            case 3:
                stat.power = Power.Cheater;
                break;
            case 4:
                stat.power = Power.Hunter;
                break;
            case 5:
                stat.power = Power.Cook;
                break;
            case 6:
                stat.power = Power.Ninja;
                break;
            default:
                Debug.Log("No Character Selected!");
                break;
        }

        SceneManager.LoadScene(NameScenePlay);
    }

    public void BackButton()
    {
        SceneManager.LoadScene("MainMenu");
    }

    private void Update()
    {
        float hor_axis = player.GetAxis("Horizontal");
        float ver_axis = player.GetAxis("Vertical");
        if ((hor_axis != 0 || ver_axis != 0) && EventSystem.current.currentSelectedGameObject == null)
        {
            default_Button.Select();
        }
    }
}
