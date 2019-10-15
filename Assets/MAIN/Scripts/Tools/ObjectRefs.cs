﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectRefs : MonoBehaviour
{
    public static ObjectRefs Instance { get; private set; }

    // Declare any public variables that you want to be able 
    // to access throughout your scene
    [Header("NavMesh")]
    public GameObject patrollZones;
    private List<GameObject> patrollZones_List;
    public GameObject menuCanvas;
    public GameObject NavMesh;
    [Header("MiniGame LockedDoors")]
    public Image[] inputContainer;
    public Sprite[] inputSet;
    [Header("Player")]
    public GameObject player;


    private void Awake()
    {
        if (Instance == null) { Instance = this; }
        else { Destroy(gameObject); }
        // Cache references to all desired variables
        if (patrollZones == null) { patrollZones = GameObject.Find("PatrollPoints"); }
        if (menuCanvas == null) { menuCanvas = GameObject.Find("MenuCanvas"); }
        if (NavMesh == null) { NavMesh = GameObject.Find("NavMesh"); }
        if (player == null) { player = GameObject.Find("Player"); }
    }
    // Start is called before the first frame update
    void Start()
    {
        patrollZones_List = new List<GameObject>();
        foreach (Transform child in patrollZones.transform)
        {
            patrollZones_List.Add(child.gameObject);
        }
    }

    public List<GameObject> GetPatrollZoneList()
    {
        return patrollZones_List;
    }
}
