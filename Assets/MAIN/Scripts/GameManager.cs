using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public List<Transform> SpawnPoint= new List<Transform>();
    float chrono = 0.4f;
    public Text chronoText;

    public CanvasManager canvas;
    public ScoreManager scoreManager;
    public GameObject[] objectiveList;

    public GameObject objective;
    public Sprite objectiveSprite;
    public bool done;

    private void Awake()
    {
        Instance = this;
        //InitialPlayerPrefTabScore();
        debugTab();
        EndScoreTable();
    }

    void Start()
    {
        objective = objectiveList[Random.Range(0, objectiveList.Length)];
        objectiveSprite = objective.GetComponent<Sprite>();
    }


    void Update()
    {
        //AddChrono();
    }

    public void CheckObjective(GameObject newObject)
    {
        
        //if (newObject == objective)
        if(Outils.GameObjectExistInArray(objectiveList, newObject))
        {
            canvas.SwitchPanel(true);
            done = true;
        }
        else
        {
            canvas.SwitchPanel(false);
            done = false;
        }
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void AddChrono()
    {
        chrono += Time.deltaTime;
        float minutes = Mathf.Floor(chrono / 60);
        float seconds = Mathf.Floor(chrono % 60);
        float millieme = Mathf.Floor((chrono - (int)chrono) * 100);
        if(chronoText!= null)chronoText.text = " Chono : " + minutes.ToString() + " : " + seconds.ToString() + " : " + millieme.ToString();

    }

    void InitialPlayerPrefTabScore()
    {
        if (PlayerPrefs.GetInt("FirstTime") == 0)
        {
            PlayerPrefs.SetInt("FirstTime", 1); 
            for(int i = 1; i<6; i++)
            {
                PlayerPrefs.SetFloat("MaxScore" + i.ToString(), Mathf.Infinity);
            }
        }
    }

    void EndScoreTable()
    {
        float endChorno = chrono;
        int place = 0;
        for (int i = 1; i < 6; i++)
        {
            if(endChorno < PlayerPrefs.GetFloat("MaxScore" + i.ToString()))
            {
                place = i;
                break;
            }       
        }
        if (place != 0)
        {
            for (int i = 5; i >= place; i--)
            {
                int j = i - 1;
                PlayerPrefs.SetFloat("MaxScore" + i.ToString(), PlayerPrefs.GetFloat("MaxScore" + j.ToString()));
            }
            PlayerPrefs.SetFloat("MaxScore" + place.ToString(), endChorno);
        }
        for (int i = 1; i < 6; i++)
        {

           // Debug.Log(PlayerPrefs.GetFloat("MaxScore" + i.ToString()));
        }


    }
    void debugTab()
    {
        PlayerPrefs.SetFloat("MaxScore1", 0.1f);
        PlayerPrefs.SetFloat("MaxScore2", 00.3f);
        PlayerPrefs.SetFloat("MaxScore3", 00.4f);
        PlayerPrefs.SetFloat("MaxScore4", 00.5f);
        PlayerPrefs.SetFloat("MaxScore5", 00.6f);
    }
    public void DetectorMetal()
    {
        // a remplir!!!! bisous elias
        Debug.Log("att Metal");
    }
    public void DetectorCamera()
    {
        Debug.Log("att Camera");
        // a remplir
    }
}
/*
 chrono fin 
 Deplacement
     */
