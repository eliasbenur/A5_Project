using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class Vibration : MonoBehaviour
{
    Player player;
    public float maxDistance = 2.55f;
    public float closeThresholdDistance = 1.9f;
    public float farPower = 0.5f;
    public float closePower = 1f;

    private void Start()
    {
        player = ReInput.players.GetPlayer(0);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Death")
        {
            Vector2 temp = transform.position - collision.transform.position;
            float power = 0.1f;
            if (temp.magnitude < maxDistance)
                power = farPower;
            if (temp.magnitude < closeThresholdDistance)
                power = closePower;
            player.SetVibration(0, power);
            player.SetVibration(1, power);
            player.SetVibration(2, power);
            player.SetVibration(3, power);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Death")
        {
            Vector2 temp = transform.position - collision.transform.position;
            float power = 0.1f;
            if (temp.magnitude < 3.5f)
                power = 0.5f;
            if (temp.magnitude < 2f)
                power = 1f;
            player.SetVibration(0, power);
            player.SetVibration(1, power);
            player.SetVibration(2, power);
            player.SetVibration(3, power);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Death")
        {
            player.SetVibration(0, 0);
            player.SetVibration(1, 0);
            player.SetVibration(2, 0);
            player.SetVibration(3, 0);
        }
    }
}
