using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class End : MonoBehaviour
{
    public LayerMask playerMask;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (GameManager.Instance.done)
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Is the Player
        if (((1 << collision.gameObject.layer) & playerMask) != 0)
        {
            //If Objective Completed
            if (GameManager.Instance.done)
            {
                ObjectRefs.Instance.menuCanvas.GetComponent<LevelMenu_Manager>().Active_WinPanel();
            }
        }
    }
}
