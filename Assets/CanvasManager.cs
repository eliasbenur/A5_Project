using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SwitchPanel(bool status)
    {
        transform.GetChild(0).gameObject.SetActive(!status);
        transform.GetChild(1).gameObject.SetActive(status);
    }
}
