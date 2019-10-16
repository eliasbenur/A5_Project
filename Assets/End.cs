using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class End : MonoBehaviour
{
    public LayerMask playerMask;
    public ObjectRemaning objectRem;

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
                for (int x = 0; x < objectRem.obj.Count; x++)
                {
                    if (objectRem.obj[x].name == collision.GetComponent<PlayerControl>().inventory[0].name)
                    {
                        objectRem.obj[x].stolen = true;
                        ObjectRefs.Instance.soungManager.PlaywinSnd();
                    }
                }
                EditorUtility.SetDirty(objectRem);
                ObjectRefs.Instance.menuCanvas.GetComponent<LevelMenu_Manager>().Active_WinPanel();
            }
        }
    }
}
