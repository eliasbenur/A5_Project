using Rewired;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelMenu_Manager : MonoBehaviour
{

    public GameObject pause_Panel;
    bool pausePanel_active = false;
    public GameObject lose_Panel;
    public GameObject win_Panel;

    Player player;

    public Button activeDefaultButton;
    public Button defaultPauseButton;
    public Button defaultLoseButton;
    public Button defaultWinButton;

    bool losed = false;

    // Start is called before the first frame update
    void Start()
    {
        player = ReInput.players.GetPlayer(0);
    }

    // Update is called once per frame
    void Update()
    {
        if (player.GetButtonDown("Pause"))
        {
            if (pausePanel_active)
            {
                Distactive_PausePanel();
            }
            else
            {
                Active_PausePanel();
            }
        }

        if (activeDefaultButton != null)
        {
            float hor_axis = player.GetAxis("Horizontal");
            float ver_axis = player.GetAxis("Vertical");
            //Debug.Log(EventSystem.current.currentSelectedGameObject + "////" + hor_axis + "// " + ver_axis + "  /// " + activeDefaultButton);
            if ((hor_axis != 0 || ver_axis != 0) && EventSystem.current.currentSelectedGameObject == null)
            {
                defaultPauseButton.Select();
            }

        }
    }

    public void Active_PausePanel()
    {
        pausePanel_active = true;
        pause_Panel.SetActive(true);
        Time.timeScale = 0;
        activeDefaultButton = defaultPauseButton;
        activeDefaultButton.Select();
    }

    public void Distactive_PausePanel()
    {
        pausePanel_active = false;
        pause_Panel.SetActive(false);
        Time.timeScale = 1;
        activeDefaultButton = null;

    }

    public void ContinueButton()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("CharacterSelectionMenu");
    }

    public void Pause_ContinueButton()
    {
        Distactive_PausePanel();
    }

    public void MainMenuButton()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitButton()
    {
        Application.Quit();
    }

    public void Active_LosePanel()
    {
        if (!losed)
        {
            Time.timeScale = 0;
            lose_Panel.SetActive(true);
            activeDefaultButton = defaultLoseButton;
            activeDefaultButton.Select();
            losed = true;
        }

    }

    public void Active_WinPanel()
    {
        Time.timeScale = 0;
        win_Panel.SetActive(true);
        activeDefaultButton = defaultWinButton;
        activeDefaultButton.Select();
    }
}
