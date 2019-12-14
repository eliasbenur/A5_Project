using Rewired;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelMenu_Manager : MonoBehaviour
{
    [Header("Panels")]
    public GameObject pause_Panel;
    public GameObject lose_Panel;
    public GameObject win_Panel;
    public GameObject objectivesPanel;
    Animator objetivesPanelAnimaton;
    ObjectRemaning objectivesData;

    Player player;

    //GamePad MenuNavigation "Focus"
    [Header ("GamePad MenuNavigation")]
    public Button defaultPauseButton;
    public Button defaultLoseButton;
    public Button defaultWinButton;
    Button activeDefaultButton;


    bool losed = false;
    public bool canShowObjective = true;

    // Start is called before the first frame update
    void Start()
    {
        objectivesData = ObjectRefs.Instance.objectivesData;
        player = ReInput.players.GetPlayer(0);
    }

    // Update is called once per frame
    void Update()
    {
        if (player.GetButtonDown("Pause"))
        {
            if (pause_Panel.activeSelf)
            {
                Distactive_PausePanel();
            }
            else
            {
                Active_PausePanel();
            }
        }

        if (player.GetButtonDown("ShowObjectives") && canShowObjective)
        {
            if (!objectivesPanel.activeSelf)
            {

                objectivesPanel.SetActive(true);
                Time.timeScale = 0;
            }
        }
        if (player.GetButtonDown("Back"))
        {
            if (objectivesPanel.activeSelf)
            {
                objectivesPanel.SetActive(false);
                Time.timeScale = 1;
            }
        }

        //Refocus the GamePad Menu Navigation if the players makes a input with
        if (activeDefaultButton != null)
        {
            float hor_axis = player.GetAxis("Horizontal");
            float ver_axis = player.GetAxis("Vertical");
            if ((hor_axis != 0 || ver_axis != 0) && EventSystem.current.currentSelectedGameObject == null)
            {
                activeDefaultButton.Select();
            }

        }
    }

    public void Active_PausePanel()
    {
        pause_Panel.SetActive(true);
        Time.timeScale = 0;
        activeDefaultButton = defaultPauseButton;
        activeDefaultButton.Select();
    }

    public void Distactive_PausePanel()
    {
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

    public void RestartGameButton()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("MAIN");
    }

    public void QuitButton()
    {
        Application.Quit();
    }

    public void Active_LosePanel()
    {
        if (!losed)
        {
            GameObject.Find("Player").GetComponent<PlayerControl>().StopVibrations();
            Time.timeScale = 0;
            lose_Panel.SetActive(true);
            activeDefaultButton = defaultLoseButton;
            activeDefaultButton.Select();
            losed = true;
        }

    }

    public void Active_WinPanel()
    {
        GameObject.Find("Player").GetComponent<PlayerControl>().StopVibrations();
        Time.timeScale = 0;
        win_Panel.SetActive(true);
        activeDefaultButton = defaultWinButton;
        activeDefaultButton.Select();
    }
}
