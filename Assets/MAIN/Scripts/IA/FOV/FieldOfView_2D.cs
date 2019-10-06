using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FieldOfView_2D : MonoBehaviour
{
    public float viewRadius = 5;
    public float viewAngle = 135;
    Collider2D[] playerInRadius;
    public LayerMask obstacleMask, playerMask;
    public List<Transform> visiblePlayer = new List<Transform>();

    private void FixedUpdate()
    {
        FindVisiblePlayer();
    }

    void FindVisiblePlayer()
    {
        playerInRadius = Physics2D.OverlapCircleAll(transform.position, viewRadius);

        visiblePlayer.Clear();

        for (int x = 0; x < playerInRadius.Length; x++)
        {
            Transform player = playerInRadius[x].transform;
            Vector2 dirPlayer = new Vector2(player.position.x - transform.position.x, player.position.y - transform.position.y);
            if (Vector2.Angle(dirPlayer, transform.right) < (viewAngle /2))
            {
                float distancePlayer = Vector2.Distance(transform.position, player.position);

                if (!Physics2D.Raycast(transform.position, dirPlayer, distancePlayer, obstacleMask))
                {
                    visiblePlayer.Add(player);
                }
            }
        }
    }

    public Vector2 DirFromAngle(float angleDeg, bool global)
    {
        if (!global)
        {
            angleDeg += transform.eulerAngles.z; /* TODO: Change to VELOCITY of agent !!*/
            //float angle_tmp = Mathf.Atan2(GetComponent<NavMeshAgent>().velocity.y, GetComponent<NavMeshAgent>().velocity.x) * Mathf.Rad2Deg;
            //angleDeg += angle_tmp;
            //angleDeg += Vector2.Angle(transform.position, transform.position + GetComponent<NavMeshAgent>().velocity);
            
        }
        return new Vector2(Mathf.Sin(angleDeg * Mathf.Rad2Deg), Mathf.Cos(angleDeg * Mathf.Deg2Rad));
    }
}
