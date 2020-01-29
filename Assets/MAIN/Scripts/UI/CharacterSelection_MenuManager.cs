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
    public GameObject DefaultPanel, ItemListPanel, panelList, canSwitchLeft, canSwitchRight;
    int index = 0;

    public Button default_Button;
    public Button MainDefaultButton, ListItemDefaultButton;

    public Stat stat;

    private void Start()
    {
        player = ReInput.players.GetPlayer(0);

        if (panelList != null && objectRemaning != null)
        {
            for (int i = 0; i < panelList.transform.childCount; i++)
            {
                if (i < objectRemaning.obj.Count )
                {
                    panelList.transform.GetChild(i).transform.GetChild(2).GetComponent<Image>().sprite = objectRemaning.obj[i].sprite;
                    panelList.transform.GetChild(i).transform.GetChild(0).GetComponent<Text>().text = objectRemaning.obj[i].name;
                    panelList.transform.GetChild(i).transform.GetChild(1).GetComponent<Text>().text = objectRemaning.obj[i].name;
                    if (!objectRemaning.obj[i].stolen) panelList.transform.GetChild(i).GetChild(3).gameObject.SetActive(false);
                }
                else
                {
                    panelList.transform.GetChild(i).gameObject.SetActive(false);
                }
            }

            if (panelList.transform.childCount < 10)
            {
                canSwitchRight.SetActive(false);
            }
            canSwitchLeft.SetActive(false);
        }

        default_Button.Select();
    }

    public void ItemListButton()
    {

        if (ItemListPanel.activeSelf)
        {
            ItemListPanel.SetActive(false);
            DefaultPanel.SetActive(true);

            default_Button = MainDefaultButton;
            default_Button.Select();
        }
        else
        {
            ItemListPanel.SetActive(true);
            DefaultPanel.SetActive(false);

            default_Button = ListItemDefaultButton;
            default_Button.Select();
        }
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
            if (index > 9)
            {
                index -= 10;
                if (index == 0) canSwitchLeft.SetActive(false);
                canSwitchRight.SetActive(true);
                for (int i = 0; i < panelList.transform.childCount; i++)
                {
                    if (i + index < objectRemaning.obj.Count)
                    {
                        panelList.transform.GetChild(i).transform.GetChild(2).GetComponent<Image>().sprite = objectRemaning.obj[i + index].sprite;
                        panelList.transform.GetChild(i).transform.GetChild(0).GetComponent<Text>().text = objectRemaning.obj[i + index].name;
                        panelList.transform.GetChild(i).transform.GetChild(1).GetComponent<Text>().text = objectRemaning.obj[i + index].name;
                        if (!objectRemaning.obj[i + index].stolen)
                        {
                            panelList.transform.GetChild(i).GetChild(3).gameObject.SetActive(false);
                        }
                        else
                        {
                            panelList.transform.GetChild(i).GetChild(3).gameObject.SetActive(true);
                        }


                        panelList.transform.GetChild(i).gameObject.SetActive(true);
                    }
                    else
                    {
                        panelList.transform.GetChild(i).gameObject.SetActive(false);
                    }
                }
            }
        }

        if (player.GetButtonDown("SwitchObjR"))
        {
            if (objectRemaning.obj.Count > index + 10)
            {
                index += 10;
                if (objectRemaning.obj.Count > index + 10){
                    canSwitchRight.SetActive(true);
                }
                else{
                    canSwitchRight.SetActive(false);
                }
                canSwitchLeft.SetActive(true);
                for (int i = 0; i < panelList.transform.childCount; i++)
                {
                    if (i + index < objectRemaning.obj.Count)
                    {
                        panelList.transform.GetChild(i).transform.GetChild(2).GetComponent<Image>().sprite = objectRemaning.obj[i + index].sprite;
                        panelList.transform.GetChild(i).transform.GetChild(0).GetComponent<Text>().text = objectRemaning.obj[i + index].name;
                        panelList.transform.GetChild(i).transform.GetChild(1).GetComponent<Text>().text = objectRemaning.obj[i + index].name;
                        if (!objectRemaning.obj[i + index].stolen)
                        {
                            panelList.transform.GetChild(i).GetChild(3).gameObject.SetActive(false);
                        }
                        else
                        {
                            panelList.transform.GetChild(i).GetChild(3).gameObject.SetActive(true);
                        }

                        panelList.transform.GetChild(i).gameObject.SetActive(true);
                    }
                    else
                    {
                        panelList.transform.GetChild(i).gameObject.SetActive(false);
                    }
                }
            }
        }
    }
}
