using BayatGames.SaveGameFree;
using Rewired;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using BayatGames.SaveGameFree;

public class MainMenu_Manager : MonoBehaviour
{
    public string NameScenePlay = "Play";
    public GameObject creditsPanel;
    public GameObject optionPanel;
    public GameObject basePanel;

    public Button default_Button;
    public Button default_CreditsButton;
    public Button default_OptionButton;
    public Button default_BaseButton;

    Player player;

    public ObjectRemaning objRemaning;

    private void Start()
    {
        player = ReInput.players.GetPlayer(0);
        default_Button.Select();
    }
    public void PlayButton()
    {
        if (SaveGame.Exists("FTUE"))
        {
            FTUE ftue_tmp = SaveGame.Load<FTUE>("FTUE");
            if (!ftue_tmp.FTUE_1_done || !ftue_tmp.FTUE_2_done || !ftue_tmp.FTUE_3_done)
            {
                SceneManager.LoadScene("MAIN");
            }
            else
            {
                SceneManager.LoadScene(NameScenePlay);
            }
        }
        else
        {
            SceneManager.LoadScene("MAIN");
        }
    }

    public void CreaditsButton()
    {
        if (creditsPanel.activeSelf) {
            creditsPanel.SetActive(false);
            basePanel.SetActive(true);
            default_Button = default_BaseButton;
            default_Button.Select();
        }
        else
        {
            creditsPanel.SetActive(true);
            basePanel.SetActive(false);
            default_Button = default_CreditsButton;
            default_Button.Select();
        }

    }

    public void OptionsButton()
    {
        if (optionPanel.activeSelf)
        {
            optionPanel.SetActive(false);
            basePanel.SetActive(true);
            default_Button = default_BaseButton;
            default_Button.Select();
        }
        else
        {
            optionPanel.SetActive(true);
            basePanel.SetActive(false);
            default_Button = default_OptionButton;
            default_Button.Select();
        }
    }

    public void ResetSave()
    {
        for (int x = 0; x < objRemaning.obj.Count; x++)
        {
            objRemaning.obj[x].stolen = false;
        }
        SaveGame.Save<ObjectRemaning>("ObjectRemaining", objRemaning);
    }

    public void QuitButton()
    {
        Application.Quit();
    }

    private void Update()
    {
        // Refocus the GamePad Menu Navigation
        float hor_axis = player.GetAxis("Horizontal");
        float ver_axis = player.GetAxis("Vertical");
        if ((hor_axis != 0 || ver_axis != 0) && EventSystem.current.currentSelectedGameObject == null)
        {
            default_Button.Select();
        }
    }


}
