using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using UnityEngine.AI;

public class GuardIAController : MonoBehaviour
{
    public PlayMakerFSM playerMakerSFM;
    public List<Vector2> pointsToPatroll = new List<Vector2>();

    public int minPositionsPatrolling, maxPositionsPatrolling;

    //IA
    public Vector2 target;

    public float speed = 3;
    public float targetDistanceDetection = 0.2f;

    bool movingToPoint = false;

    NavMeshAgent agent;

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
    }

    // Update is called once per frame
    void Update()
    {
        movingToPoint = playerMakerSFM.FsmVariables.FindFsmBool("movingToPoint").Value;
        if (movingToPoint)
        {
            //Movement
            IsPointReached();
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
            pointsToPatroll.RemoveAt(0);
            agent.isStopped = true;
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
