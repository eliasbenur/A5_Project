using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.U2D;

public class RoomFOW : MonoBehaviour
{
    public bool entered;
    public bool isOpen;

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
                    }
                    gameObject.GetComponent<SpriteShapeRenderer>().enabled = false;
                    gameObject.GetComponent<PolygonCollider2D>().enabled = false;
                    isOpen = true;
                    entered = true;
                }
                break;
            }
        }
        if (isOpen)
        {

            gameObject.GetComponent<SpriteShapeRenderer>().enabled = true;
            Color col = gameObject.GetComponent<SpriteShapeRenderer>().color;
            gameObject.GetComponent<SpriteShapeRenderer>().color = new Color(col.r, col.g, col.b, 128);
            gameObject.GetComponent<PolygonCollider2D>().enabled = true;
            isOpen = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Death")
        {
            guards.Add(collision.gameObject.GetComponent<SpriteRenderer>());
            collision.gameObject.GetComponent<SpriteRenderer>().enabled = false;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Death")
        {
            guards.Remove(collision.gameObject.GetComponent<SpriteRenderer>());
            collision.gameObject.GetComponent<SpriteRenderer>().enabled = true;
        }
    }
}
