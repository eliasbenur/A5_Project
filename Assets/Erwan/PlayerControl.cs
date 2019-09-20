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
    public Vector2 moveVector;


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
}
