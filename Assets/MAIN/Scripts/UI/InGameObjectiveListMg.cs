using Rewired;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameObjectiveListMg : MonoBehaviour
{
    public GameObject panelObjImg, panelObjText, canSwitchLeft, canSwitchRight;
    public ObjectRemaning objectRemaning;
    int index = 0;
    Player player;
    void Start()
    {
        player = ReInput.players.GetPlayer(0);
        if (panelObjImg != null && objectRemaning != null)
        {
            int j = 0;
            for (int i = 0; i < panelObjImg.transform.childCount; i++)
            {
                try
                {

                    panelObjImg.transform.GetChild(i).GetChild(0).GetComponent<Image>().sprite =
                        objectRemaning.obj[i].sprite;
                    panelObjText.transform.GetChild(i).GetComponent<Text>().text =
                        objectRemaning.obj[i].name;
                    if (objectRemaning.obj[i].stolen) panelObjImg.transform.GetChild(i).GetChild(1).gameObject.SetActive(true);
                }
                catch { }
            }
            canSwitchLeft.SetActive(false);
        }
    }
    private void Update()
    {
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
                    if (objectRemaning.obj[index + i].stolen)
                    {
                        panelObjImg.transform.GetChild(i).GetChild(1).gameObject.SetActive(true);
                    }
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
