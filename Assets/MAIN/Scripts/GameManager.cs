using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using BayatGames.SaveGameFree;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    //If the player has taken a treasure in the Lvl
    public bool objectiveDone;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        if(SaveGame.Exists("ObjectRemaining"))
            ObjectRefs.Instance.objectivesData = SaveGame.Load<ObjectRemaning>("ObjectRemaining");

        //Desactive treasure already stolen
        foreach (stringAndBool sb in ObjectRefs.Instance.objectivesData.obj)
        {
            if (sb.stolen)
            {
                GameObject treaure_tmp = GameObject.Find(sb.name);
                if(treaure_tmp!=null) treaure_tmp.SetActive(false);
            }
        }
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
