using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameObject currentObjective_Canvas;
    public GameObject objectiveDone_Canvas;

    //If the player has taken a treasure in the Lvl
    public bool objectiveDone;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        //Desactive treasure already stolen
        foreach (stringAndBool sb in ObjectRefs.Instance.objectivesData.obj)
        {
            if (sb.stolen)
            {
                //Destroy(GameObject.Find(sb.name));
                GameObject.Find(sb.name).SetActive(false);
            }
        }
    }

    public void CheckObjective(GameObject newObject)
    {
        //If the treasure taken is in the list of objectives
        /*if (Outils.GameObjectExistInArray(ObjectRefs.Instance.objectivesRef_List, newObject))
        {
            currentObjective_Canvas.gameObject.SetActive(false);
            objectiveDone_Canvas.gameObject.SetActive(true);
            objectiveDone = true;
        }*/
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    /*Player detected by a Metal Detector */
    public void DetectorMetal()
    {
        ObjectRefs.Instance.menuCanvas.GetComponent<LevelMenu_Manager>().Active_LosePanel();
    }
    /*Player detected by a Camera */
    public void DetectorCamera()
    {
        ObjectRefs.Instance.menuCanvas.GetComponent<LevelMenu_Manager>().Active_LosePanel();
    }
}
