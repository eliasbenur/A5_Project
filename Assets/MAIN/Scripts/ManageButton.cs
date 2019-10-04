using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ManageButton : MonoBehaviour
{
    public string NameScenePlay = "Play";
    public GameObject option;
    public GameObject ChoosePlayer;
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
