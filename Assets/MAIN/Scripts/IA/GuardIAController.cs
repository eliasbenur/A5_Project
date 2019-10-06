using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using UnityEngine.AI;

public class GuardIAController : MonoBehaviour
{
    public PlayMakerFSM playerMakerSFM;
    FOV_v3 fow;
    public List<Vector2> pointsToPatroll = new List<Vector2>();

    public int minPositionsPatrolling, maxPositionsPatrolling;

    //IA
    public Vector2 target;
    public Vector2 target_tmp;

    public float speed = 3;
    public float targetDistanceDetection = 0.2f;

    bool movingToPoint = false;

    NavMeshAgent agent;

    public bool chasingPlayer = false;

    public float chasingDistance;
    public float chasingSpeedFactor;

    public SpriteRenderer playerSpottedSprite;


    // Start is called before the first frame update
    void Start()
    {
        if (playerMakerSFM == null)
        {
            gameObject.GetComponent<PlayMakerFSM>();
        }

        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;

        fow = GetComponent<FOV_v3>();

    }

    // Update is called once per frame
    void Update()
    {
        if (!chasingPlayer)
        {
            movingToPoint = playerMakerSFM.FsmVariables.FindFsmBool("movingToPoint").Value;
            if (movingToPoint)
            {
                //Movement
                IsPointReached();
            }
        }
        else
        {
            if (fow.visiblePlayer.Count == 0)
            {
                if (Vector2.Distance(agent.destination, transform.position) < chasingDistance)
                {
                    chasingPlayer = false;
                    speed = speed / chasingSpeedFactor;
                    playerMakerSFM.SendEvent("SearchAction");

                    //Sprite Spotted
                    playerSpottedSprite.enabled = false;
                }
            }
            else
            {
                agent.SetDestination(fow.visiblePlayer[0].transform.position);

                if (Vector2.Distance(fow.visiblePlayer[0].transform.position, transform.position) < chasingDistance)
                {
                    Debug.Log("PlayerChased!");
                }
            }
        }
        CheckPlayer();
    }

    public void PlayerNoiseDetected(Vector3 target_)
    {
        agent.SetDestination(target_);
        playerMakerSFM.SendEvent("ChasingPlayer");
        speed = speed * chasingSpeedFactor;
        chasingPlayer = true;
        agent.isStopped = false;

        //Sprite Spotted
        playerSpottedSprite.enabled = true;
    }

    

    public void CheckPlayer()
    {
        if (fow.visiblePlayer.Count > 0)
        {
            if (!chasingPlayer)
            {
                playerMakerSFM.SendEvent("ChasingPlayer");
                speed = speed * chasingSpeedFactor;
                chasingPlayer = true;
                agent.isStopped = false;

                //Sprite Spotted
                playerSpottedSprite.enabled = true;
            }
        }
    }

    public void SetUpPathToPoint()
    {
        if (pointsToPatroll.Count <= 0)
        {
            playerMakerSFM.SendEvent("SearchAction");
        }
        else
        {
            agent.SetDestination(pointsToPatroll[0]);
            agent.isStopped = false;
        }
        
    }

    public void IsPointReached()
    {
        if (pointsToPatroll.Count == 0)
        {
            agent.isStopped = true;
            playerMakerSFM.SendEvent("PointReached");
            return;
        }

        float distance = Vector2.Distance(transform.position, pointsToPatroll[0]);

        if (distance < targetDistanceDetection)
        {
            agent.isStopped = true;
            pointsToPatroll.RemoveAt(0);
            playerMakerSFM.SendEvent("PointReached");
            return;
        }
        else
        {
            agent.SetDestination(pointsToPatroll[0]);
        }
    }

    /*Returns the points that is going to patroll in a RandomZone*/
    public void GetPointsToPatroll()
    {
        pointsToPatroll = new List<Vector2>();
        int randomZone = Random.Range(0, ObjectRefs.Instance.GetPatrollZoneList().Count);
        int nulOfPoints = Random.Range(minPositionsPatrolling, maxPositionsPatrolling);
        for (int x = 0; x < nulOfPoints; x++)
        {
            Vector2 newPoint = Outils.RandomPointInBounds(ObjectRefs.Instance.GetPatrollZoneList()[randomZone].GetComponent<BoxCollider2D>().bounds);
            pointsToPatroll.Add(newPoint);
        }
        playerMakerSFM.SendEvent("PatrollZoneSetUp");
    }
}
