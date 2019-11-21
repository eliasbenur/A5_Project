using Rewired;
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
    public GameObject panelObjImg, panelObjText, canSwitchLeft, canSwitchRight;
    int index = 0;

    public Button default_Button;

    public Stat stat;

    private void Start()
    {
        player = ReInput.players.GetPlayer(0);

        if (panelObjImg != null && objectRemaning != null)
        {
            int j = 0;
            for (int i = 0; i < panelObjImg.transform.childCount; i++)
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

                    panelObjImg.transform.GetChild(i).GetChild(0).GetComponent<Image>().sprite =
                        objectRemaning.obj[i].sprite;
                    panelObjText.transform.GetChild(i).GetComponent<Text>().text =
                        objectRemaning.obj[i].name;
                    if (objectRemaning.obj[i].stolen) panelObjImg.transform.GetChild(i).GetChild(1).gameObject.SetActive(true);
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
            case 7:
                stat.power = Power.DejaVu;
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


        //Carrosel
        if (player.GetButtonDown("SwitchObjL"))
        {
            if (index > 0)
            {
                index--;
                if (index == 0) canSwitchLeft.SetActive(false);
                if (objectRemaning.obj.Count > 5) canSwitchRight.SetActive(true);
                for (int i = 0; i < panelObjImg.transform.childCount; i++)
                {
                    panelObjImg.transform.GetChild(i).GetChild(0).GetComponent<Image>().sprite =
                        objectRemaning.obj[index + i].sprite;
                    panelObjText.transform.GetChild(i).GetComponent<Text>().text =
                        objectRemaning.obj[index + i].name;
                    if (objectRemaning.obj[index + i].stolen) panelObjImg.transform.GetChild(i).GetChild(1).gameObject.SetActive(true);
                    else
                    {
                        panelObjImg.transform.GetChild(i).GetChild(1).gameObject.SetActive(false);
                    }
                }
            }
        }
        //if (Input.GetKeyDown("e"))
        if (player.GetButtonDown("SwitchObjR"))
        {
            if (index < objectRemaning.obj.Count - 5)
            {
                index++;
                if (index == objectRemaning.obj.Count - 5) canSwitchRight.SetActive(false);
                if (objectRemaning.obj.Count > 5) canSwitchLeft.SetActive(true);
                for (int i = 0; i < panelObjImg.transform.childCount; i++)
                {
                    panelObjImg.transform.GetChild(i).GetChild(0).GetComponent<Image>().sprite =
                            objectRemaning.obj[index + i].sprite;
                    panelObjText.transform.GetChild(i).GetComponent<Text>().text =
                        objectRemaning.obj[index + i].name;
                    if (objectRemaning.obj[index + i].stolen) panelObjImg.transform.GetChild(i).GetChild(1).gameObject.SetActive(true);
                    else
                    {
                        panelObjImg.transform.GetChild(i).GetChild(1).gameObject.SetActive(false);
                    }
                }
            }
        }
    }
}
