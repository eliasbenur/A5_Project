using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraUpdate : MonoBehaviour
{
    public Transform toFollow;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (toFollow != null)
        {
            transform.position = toFollow.position + new Vector3(0,0, -10);
        }
    }
}
