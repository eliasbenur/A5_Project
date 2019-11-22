using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using UnityEngine.AI;
using UnityEditor;
#if UNITY_EDITOR
using UnityEditor;
#endif



public class GuardIAController : MonoBehaviour
{
    public PlayMakerFSM playerMakerSFM;
    FOV_v3 fow;
    public List<Vector2> pointsToPatroll = new List<Vector2>();

    public int minPositionsPatrolling, maxPositionsPatrolling;

    public ia_BehaviourType behaviourType;
    [HideInInspector]
    public List<GameObject> patrollZones_List, patrollPoints_List;

    //IA
    public Vector2 target;
    public Vector2 target_tmp;
    public Vector2 iniPos;
    public Vector2 stationaryDirection;

    public float speed = 3;
    public float targetDistanceDetection = 0.2f;

    bool movingToPoint = false;

    NavMeshAgent agent;

    public bool chasingPlayer = false;

    //Donut
    public bool chasingDonut = false;
    public bool eatingDonut = false;
    public GameObject donutRef;
    public float eatingDonutTime;

    public float chasingDistance;
    public float chasingSpeedFactor;

    public SpriteRenderer playerSpottedSprite;

    public bool checkingZone = false;
    public SpriteRenderer checkingZoneSprite;
    public int checkingZoneNumPoints;
    public Vector2 checkingZone_RandomZoneSize;

    public Bounds OrthographicBounds(Camera camera)
    {
        float screenAspect = (float)Screen.width / (float)Screen.height;
        float cameraHeight = camera.orthographicSize * 2;
        Bounds bounds = new Bounds(
            camera.transform.position,
            new Vector3(cameraHeight * screenAspect, cameraHeight, 0));
        return bounds;
    }

    // Start is called before the first frame update
    void Start()
    {
        //ObjectRefs.Instance.GAIC.Add(this);
        if (playerMakerSFM == null)
        {
            gameObject.GetComponent<PlayMakerFSM>();
        }

        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;

        fow = GetComponent<FOV_v3>();

        if (behaviourType == ia_BehaviourType.Stationary)
        {
            iniPos = transform.position;
        }

        agent.speed = speed;

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
            if (behaviourType == ia_BehaviourType.SpawnedIA)
            {
                Bounds CameraBound = OrthographicBounds(Camera.main);
                CameraBound.center = new Vector3(CameraBound.center.x, CameraBound.center.y, 0);
                CameraBound.size = new Vector3(CameraBound.size.x + 15, CameraBound.size.y + 15, 0);
                Vector2 IABound = (Vector2)transform.position;
                if (!CameraBound.Contains(IABound))
                {
                    Debug.Log("Hey");
                    StartCoroutine(DesPawn(1));
                }
            }
        }
        else
        {
            if (fow.objToCheckList.Count == 0)
            {
                if (Vector2.Distance(agent.destination, transform.position) < chasingDistance)
                {
                    /*if (checkingZone)
                    {*/
                        chasingPlayer = false;
                        checkingZone = false;
                        agent.speed = speed / chasingSpeedFactor;
                        playerMakerSFM.SendEvent("SearchAction");

                        playerSpottedSprite.enabled = false;
                        checkingZoneSprite.enabled = false;
                    /*}
                    else
                    {
                        checkingZone = true;

                        chasingPlayer = false;
                        playerSpottedSprite.enabled = false;
                        checkingZoneSprite.enabled = true;

                        Bounds bound_tmp = new Bounds(transform.position, checkingZone_RandomZoneSize);
                        for (int x = 0; x < checkingZoneNumPoints; x++)
                        {
                            Vector2 newPoint = Outils.RandomPointInBounds(bound_tmp);
                            pointsToPatroll.Add(newPoint);
                        }
                    }*/


                }
            }
            else
            {
                agent.SetDestination(fow.objToCheckList[0].transform.position);

                if (Vector2.Distance(fow.objToCheckList[0].transform.position, transform.position) < chasingDistance)
                {
                    Debug.Log("PlayerChased!");
                    ObjectRefs.Instance.menuCanvas.GetComponent<LevelMenu_Manager>().Active_LosePanel();
                }
            }
        }
        CheckPlayer();
    }



    public void PlayerNoiseDetected(Vector3 target_)
    {
        agent.SetDestination(target_);
        playerMakerSFM.SendEvent("ChasingPlayer");
        agent.speed = speed * chasingSpeedFactor;
        chasingPlayer = true;
        agent.isStopped = false;

        //Sprite Spotted
        playerSpottedSprite.enabled = true;
        ObjectRefs.Instance.soungManager.PlayguardesAttention();
    }

    public IEnumerator DesPawn(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(this.gameObject);
    }

    public void StartChasingPlayerVarInis()
    {
        playerMakerSFM.SendEvent("ChasingPlayer");
        if (agent == null)
        {
            agent = GetComponent<NavMeshAgent>();
        }
        agent.speed = speed * chasingSpeedFactor;
        chasingPlayer = true;
        agent.isStopped = false;
        eatingDonut = false;
        chasingDonut = false;
        StopAllCoroutines();

        //Sprite Spotted
        playerSpottedSprite.enabled = true;
        ObjectRefs.Instance.soungManager.PlayguardesAttention();
    }

    public void CheckPlayer()
    {
        if (fow.objToCheckList.Count > 0)
        {
            if (!chasingPlayer)
            {
                foreach (Transform objs in fow.objToCheckList)
                {
                    if (objs.gameObject.layer == 9)//Player
                    {
                        StartChasingPlayerVarInis();
                    }
                    else if (objs.gameObject.layer == 13) // Donut
                    {
                        if (!chasingDonut)
                        {
                            pointsToPatroll.Insert(0, objs.transform.position);
                            agent.SetDestination(pointsToPatroll[0]);
                            donutRef = objs.gameObject;
                            //playerMakerSFM.SendEvent("ChasingDonut");
                            agent.isStopped = false;
                            chasingDonut = true;
                        }
                    }
                }

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
            if (chasingDonut)
            {
                if (!eatingDonut)
                {
                    StartCoroutine(EatingDonnut());
                }
            }
            else
            {
                agent.isStopped = true;
                pointsToPatroll.RemoveAt(0);
                playerMakerSFM.SendEvent("PointReached");
                return;
            }
        }
        else
        {
            agent.SetDestination(pointsToPatroll[0]);
        }
    }

    IEnumerator EatingDonnut()
    {
        eatingDonut = true;
        yield return new WaitForSeconds(eatingDonutTime);
        Destroy(donutRef);
        chasingDonut = false;
        agent.isStopped = true;
        pointsToPatroll.RemoveAt(0);
        playerMakerSFM.SendEvent("PointReached");
        eatingDonut = false;

    }

    /*Returns the points that is going to patroll in a RandomZone*/
    public void GetPointsToPatroll()
    {
        pointsToPatroll = new List<Vector2>();
        
        //int randomZone = Random.Range(0, ObjectRefs.Instance.GetPatrollZoneList().Count);
        switch (behaviourType)
        {
            case ia_BehaviourType.Stationary:
                pointsToPatroll.Add(iniPos);
                break;
            case ia_BehaviourType.RandomZone:
                int randomZone = Random.Range(0, patrollZones_List.Count);
                int nulOfPoints = Random.Range(minPositionsPatrolling, maxPositionsPatrolling);
                for (int x = 0; x < nulOfPoints; x++)
                {
                    Vector2 newPoint = Outils.RandomPointInBounds(patrollZones_List[randomZone].GetComponent<BoxCollider2D>().bounds);
                    pointsToPatroll.Add(newPoint);
                }
                break;
            case ia_BehaviourType.SuccessivePoints:
                for (int x = 0; x < patrollPoints_List.Count; x++)
                {
                    Vector2 newPoint = patrollPoints_List[x].transform.position;
                    pointsToPatroll.Add(newPoint);
                }
                break;
        }
        

        playerMakerSFM.SendEvent("PatrollZoneSetUp");
    }



#if UNITY_EDITOR


    [CustomEditor(typeof(GuardIAController))]
    public class GuardIAControllerEditor : Editor
    {
        private GuardIAController GAC { get { return (target as GuardIAController); } }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            EditorGUI.BeginChangeCheck();
            ia_BehaviourType iaB = GAC.behaviourType;
            EditorGUILayout.Space();
            switch (iaB)
            {
                case ia_BehaviourType.RandomZone:
                    var list = GAC.patrollZones_List;
                    int newCount = Mathf.Max(0, EditorGUILayout.IntField("randomZone", list.Count));
                    while (newCount < list.Count)
                        list.RemoveAt(list.Count - 1);
                    while (newCount > list.Count)
                        list.Add(null);

                    for (int i = 0; i < list.Count; i++)
                    {
                        list[i] = (GameObject)EditorGUILayout.ObjectField(list[i], typeof(GameObject));
                    }
                    break;
                case ia_BehaviourType.Stationary:

                    break;
                case ia_BehaviourType.SuccessivePoints:
                    var list2 = GAC.patrollPoints_List;
                    int newCount2 = Mathf.Max(0, EditorGUILayout.IntField("randomZone", list2.Count));
                    while (newCount2 < list2.Count)
                        list2.RemoveAt(list2.Count - 1);
                    while (newCount2 > list2.Count)
                        list2.Add(null);

                    for (int i = 0; i < list2.Count; i++)
                    {
                        list2[i] = (GameObject)EditorGUILayout.ObjectField(list2[i], typeof(GameObject));
                    }
                    break;
                    

            }


            if (EditorGUI.EndChangeCheck())
                EditorUtility.SetDirty(GAC);
        }
    }
#endif
}
