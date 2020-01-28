using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecretPathDoorTrigger : MonoBehaviour
{
    public LayerMask playerMask;

    public GameObject Door;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (((1 << collision.gameObject.layer) & playerMask) != 0)
        {
            Door.SetActive(false);
        }
    }

}
