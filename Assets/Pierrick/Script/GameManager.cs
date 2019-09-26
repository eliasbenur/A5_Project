﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public List<Transform> pointDeSpawn= new List<Transform>();
    float chrono = 0.4f;
    public Text chronoText;
    private void Awake()
    {
        Instance = this;
        //InitialPlayerPrefTabScore();
        debugTab();
        TableauScoreFin();
    }

    void Start()
    {
        
    }


    void Update()
    {
        //AddChrono();
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

    void TableauScoreFin()
    {
        float chornoFin = chrono;
        int placeClassement = 0;
        for (int i = 1; i < 6; i++)
        {
            if(chornoFin < PlayerPrefs.GetFloat("MaxScore" + i.ToString()))
            {
                placeClassement = i;
                break;
            }       
        }
        if (placeClassement != 0)
        {
            for (int i = 5; i >= placeClassement; i--)
            {
                int j = i - 1;
                PlayerPrefs.SetFloat("MaxScore" + i.ToString(), PlayerPrefs.GetFloat("MaxScore" + j.ToString()));
            }
            PlayerPrefs.SetFloat("MaxScore" + placeClassement.ToString(), chornoFin);
        }
        for (int i = 1; i < 6; i++)
        {

            Debug.Log(PlayerPrefs.GetFloat("MaxScore" + i.ToString()));
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
