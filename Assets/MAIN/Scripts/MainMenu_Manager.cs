using Rewired;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu_Manager : MonoBehaviour
{
    public string NameScenePlay = "Play";
    public GameObject option;
    public GameObject basePanel;

    public Button default_Button;

    Player player;

    private void Start()
    {
        player = ReInput.players.GetPlayer(0);
        default_Button.Select();
    }
    public void PlayButton()
    {
        SceneManager.LoadScene(NameScenePlay);
    }

    public void OptionButton()
    {
        //option.SetActive(true);
    }

    public void QuitButton()
    {
        Application.Quit();
    }

    private void Update()
    {
        float hor_axis = player.GetAxis("Horizontal");
        float ver_axis = player.GetAxis("Vertical");
        if ((hor_axis != 0 || ver_axis != 0) && EventSystem.current.currentSelectedGameObject == null)
        {
            default_Button.Select();
        }
    }


}
