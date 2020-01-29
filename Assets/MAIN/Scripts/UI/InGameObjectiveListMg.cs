using Rewired;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InGameObjectiveListMg : MonoBehaviour
{
    public ObjectRemaning objectRemaning;
    public GameObject ItemListPanel, panelList, canSwitchLeft, canSwitchRight;
    int index = 0;

    Player player;
    void Start()
    {
        player = ReInput.players.GetPlayer(0);
        if (panelList != null && objectRemaning != null)
        {
            for (int i = 0; i < panelList.transform.childCount; i++)
            {
                if (i < objectRemaning.obj.Count)
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
    }

    public void ItemListButton()
    {

        if (ItemListPanel.activeSelf)
        {
            ItemListPanel.SetActive(false);

        }
        else
        {
            ItemListPanel.SetActive(true);
        }
    }
    private void Update()
    {
        float hor_axis = player.GetAxis("Horizontal");
        float ver_axis = player.GetAxis("Vertical");


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
                if (objectRemaning.obj.Count > index + 10)
                {
                    canSwitchRight.SetActive(true);
                }
                else
                {
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
