using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class PlayerControl : MonoBehaviour
{
    Player player;
    public Rigidbody2D rb;

    public float speedMod = 1;
    public bool canMove;
    public bool canInteract;
    public Object interactableObject;
    public Vector2 moveVector;

    public List<Object> inventory = new List<Object>();


    private void Start()
    {
        player = ReInput.players.GetPlayer(0);
    }

    private void Update()
    {
        if (canMove)
        {
            moveVector.x = player.GetAxis("Horizontal");
            moveVector.y = player.GetAxis("Vertical");
        }
        else
        {
            moveVector.Normalize();
        }

        if (player.GetButtonDown("Interact") && canInteract)
        {
            Action(interactableObject);
        }
    }

    private void FixedUpdate()
    {
        if (moveVector != Vector2.zero)
        {
            rb.velocity = new Vector2(moveVector.x * speedMod, moveVector.y * speedMod);
        }
        else
        {
            rb.velocity -= rb.velocity * 0.25f;
        }
    }

    protected void Action(Object newObject)
    {
        if (newObject != null) newObject.ActiveEvent();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Object newObject = collision.GetComponent<Object>();
        if (newObject != null)
        {
            interactableObject = newObject;
            newObject.playerControl = this;
            canInteract = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Object newObject = collision.GetComponent<Object>();
        if (newObject == interactableObject)
        {
            interactableObject = null;
            newObject.playerControl = null;
            canInteract = false;
        }
    }
}
