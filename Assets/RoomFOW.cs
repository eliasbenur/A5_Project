using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.U2D;

public class RoomFOW : MonoBehaviour
{
    public bool entered;
    public bool isOpen;
    public bool checkOpen;
    public bool playerIn;

    public List<SpriteRenderer> guards;
    public Doorv2[] doors;

    void Start()
    {
        guards = new List<SpriteRenderer>();
    }

    void Update()
    {
        for (int i = 0; i < doors.Length; i++)
        {
            if (doors[i].open)
            {
                if (!isOpen)
                {
                    entered = true;
                    foreach (SpriteRenderer rend in guards)
                    {
                        rend.enabled = !rend.enabled;
                        rend.transform.GetChild(0).gameObject.SetActive(true);
                    }
                    if (!playerIn)
                        Reveal();
                    else
                        playerIn = false;
                    isOpen = true;
                    entered = true;
                }
                checkOpen = true;
                break;
            }
        }
        if (checkOpen == false)
        {
            if (isOpen)
            {

                gameObject.GetComponent<SpriteShapeRenderer>().enabled = !gameObject.GetComponent<SpriteShapeRenderer>().enabled;
                Color col = gameObject.GetComponent<SpriteShapeRenderer>().color;
                gameObject.GetComponent<SpriteShapeRenderer>().color = new Color(col.r, col.g, col.b, 0.5f);
                gameObject.GetComponent<PolygonCollider2D>().enabled = !gameObject.GetComponent<PolygonCollider2D>().enabled;
                transform.GetChild(0).gameObject.gameObject.GetComponent<SpriteShapeRenderer>().enabled = !transform.GetChild(0).gameObject.gameObject.GetComponent<SpriteShapeRenderer>().enabled;
                transform.GetChild(0).gameObject.GetComponent<SpriteShapeRenderer>().color = new Color(col.r, col.g, col.b, 0.5f);
                isOpen = false;
            }
        }
        else
            checkOpen = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && !playerIn && isOpen)
        {
            playerIn = true;
            foreach (SpriteRenderer rend in guards)
            {
                rend.enabled = !rend.enabled;
                rend.transform.GetChild(0).gameObject.SetActive(true);
            }
            Reveal();
        }
        if (collision.tag == "Death" && !playerIn)
        {
            guards.Add(collision.gameObject.GetComponent<SpriteRenderer>());
            collision.gameObject.GetComponent<SpriteRenderer>().enabled = false;
            collision.transform.GetChild(0).gameObject.SetActive(false);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Death")
        {
            guards.Remove(collision.gameObject.GetComponent<SpriteRenderer>());
            collision.gameObject.GetComponent<SpriteRenderer>().enabled = true;
            collision.transform.GetChild(0).gameObject.SetActive(true);
        }
    }

    public void Reveal()
    {
        gameObject.GetComponent<SpriteShapeRenderer>().enabled = !gameObject.GetComponent<SpriteShapeRenderer>().enabled;
        gameObject.GetComponent<PolygonCollider2D>().enabled = !gameObject.GetComponent<PolygonCollider2D>().enabled;
        transform.GetChild(0).gameObject.gameObject.GetComponent<SpriteShapeRenderer>().enabled = !transform.GetChild(0).gameObject.gameObject.GetComponent<SpriteShapeRenderer>().enabled;
       
    }
}
