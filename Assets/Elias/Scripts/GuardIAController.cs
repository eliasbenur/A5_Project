using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardIAController : MonoBehaviour
{
    public PlayMakerFSM playerMakerSFM;
    public List<Vector2> pointsToPatroll = new List<Vector2>();

    public int minPositionsPatrolling, maxPositionsPatrolling;

    // Start is called before the first frame update
    void Start()
    {
        if (playerMakerSFM == null)
        {
            gameObject.GetComponent<PlayMakerFSM>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
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
        Debug.Log("Test");
        playerMakerSFM.FsmVariables.FindFsmVector2("PointToPatroll").Value = pointsToPatroll[0];
    }
}
