using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameObject objectiveCanvas;
    public GameObject[] objectiveList;

    public GameObject objective;
    public Sprite objectiveSprite;
    public bool done;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        objective = objectiveList[Random.Range(0, objectiveList.Length)];
        objectiveSprite = objective.GetComponent<Sprite>();
    }


    void Update()
    {

    }

    public void CheckObjective(GameObject newObject)
    {
        //ObjectRefs.Instance.objRemaning.obj.Contains();
        if (Outils.GameObjectExistInArray(objectiveList, newObject))
        {
            //objectiveCanvas.SwitchPanel(true);
            objectiveCanvas.transform.GetChild(0).gameObject.SetActive(false);
            objectiveCanvas.transform.GetChild(1).gameObject.SetActive(true);
            done = true;
        }
        else
        {
            //objectiveCanvas.SwitchPanel(false);
            objectiveCanvas.transform.GetChild(0).gameObject.SetActive(true);
            objectiveCanvas.transform.GetChild(1).gameObject.SetActive(false);
            done = false;
        }
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void DetectorMetal()
    {
        ObjectRefs.Instance.menuCanvas.GetComponent<LevelMenu_Manager>().Active_LosePanel();
    }
    public void DetectorCamera()
    {
        ObjectRefs.Instance.menuCanvas.GetComponent<LevelMenu_Manager>().Active_LosePanel();
    }
}
