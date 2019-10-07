using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ManageButton : MonoBehaviour
{
    public string NameScenePlay = "Play";
    public GameObject option;
    public GameObject ChoosePlayer;
    public GameObject panelObj;

    public ObjectRemaning objectRemaning;

    private void Start()
    {
        if (panelObj != null&&objectRemaning!=null)
        {
            int j = 0;
            for (int i = 0; i < panelObj.transform.childCount; i++)
            {
              try {
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
    }
    public void Play()
    {
        SceneManager.LoadScene(NameScenePlay);
    }

    public void Option()
    {
        option.SetActive(true);
    }

    public void OnApplicationQuit()
    {
        Application.Quit();
    }


}
