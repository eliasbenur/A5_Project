using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectRefs : MonoBehaviour
{

    public GameObject patrollZones;
    private List<GameObject> patrollZones_List;
    // Start is called before the first frame update
    void Start()
    {
        foreach (Transform child in patrollZones.transform)
        {
            patrollZones_List.Add(child.gameObject);
        }
    }

    List<GameObject> GetPatrollZoneList()
    {
        return patrollZones_List;
    }
}
