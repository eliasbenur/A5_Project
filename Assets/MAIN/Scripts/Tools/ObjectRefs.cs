using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectRefs : MonoBehaviour
{
    public static ObjectRefs Instance { get; private set; }

    // Declare any public variables that you want to be able 
    // to access throughout your scene
    public GameObject patrollZones;
    private List<GameObject> patrollZones_List;
    public GameObject menuCanvas;

    private void Awake()
    {
        if (Instance == null) { Instance = this; }
        else { Destroy(gameObject); }
        // Cache references to all desired variables
        if (patrollZones == null) { patrollZones = GameObject.Find("PatrollPoints"); }
        if (menuCanvas == null) { menuCanvas = GameObject.Find("MenuCanvas"); }
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
