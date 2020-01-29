using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using MyBox;
public enum GUARD_TYPE { Normal, Elite };

public class GuardIAController_v2 : MonoBehaviour
{
    [Separator("NavMesh")]
    public List<Transform> patrollPoints_List;
    public List<Vector3> pointToGo;
    NavMeshAgent agent;
    public float distancePointsCheck;

    [Separator("Guard Type")]
    public GUARD_TYPE guardType;

    [Separator("FOW")]
    public Vector2 stationaryDirection;
    FOV_vBT fow;
    public float baseviewAngle;
    public float alertviewAngle;


    [Separator("Turning")]
    public float turningDelay;
    float turningDelay_tmp;
    float stationaryDegreeRotation;

    [Separator("SeekZone")]
    public Vector2 checkZoneSize;
    public int checkZone_NumPoints;


    // IA States
    public bool checkingtheZone = false;
    public bool chasingPlayer = false;
    bool turningInPlace = false;
    bool inAlertMode = false;

    [Separator("Sprites")]
    public SpriteRenderer checkingZoneSprite;
    public SpriteRenderer playerSpottedSprite;

    [Separator("Donut")]
    public float eatingDonutDelay;
    float eatingDonutDelay_tmp;
    GameObject donutRef;
    bool eatingDonut;

    [Separator("Spread")]
    public float spreadRadius;

    [Separator("Speed")]
    public AnimationCurve speedCurve;
    float speedCurve_CurrentTime;
    public float speedCurve_MaxTime;
    float currentSpeed;
    public float maxSpeed;
    public float Chasing_maxSpeed;

    [Separator("AlertMode")]
    public float alertModeTime;
    float alertModeTime_tmp;

    [Separator("LayersMasks")]
    public LayerMask guardMask;
    public LayerMask collidersCheckZone;
    public LayerMask playerMask;

    //
    public bool spawnedIA;

    Vector2 newDestination;

    // Start is called before the first frame update
    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;

        fow = GetComponent<FOV_vBT>();

        turningDelay_tmp = -1;
        alertModeTime_tmp = -1;

        pointToGo = new List<Vector3>();

        SetUpPatrollPoints();

        ObjectRefs.Instance.GAIC.Add(this);
    }

    // Update is called once per frame
    void Update()
    {
        SpeedUpdate();
        AlertModeUpdate();

        //Game Over
        if (Vector2.Distance(ObjectRefs.Instance.player.transform.position, transform.position) < distancePointsCheck)
        {
            Debug.Log("PlayerChased!");
            ObjectRefs.Instance.menuCanvas.GetComponent<LevelMenu_Manager>().Active_LosePanel();
        }

        if (spawnedIA)
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

    public IEnumerator DesPawn(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(this.gameObject);
    }

    public void AlertModeUpdate()
    {
        if (alertModeTime_tmp > 0)
        {
            alertModeTime_tmp -= Time.deltaTime;
        }
        else
        {
            if (inAlertMode)
            {
                checkingZoneSprite.enabled = false;
                inAlertMode = false;
                turningInPlace = false;
            }
        }
    }

    public void SpeedUpdate()
    {
        if (speedCurve_CurrentTime < speedCurve_MaxTime)
        {
            speedCurve_CurrentTime += Time.deltaTime;
        }
        if (chasingPlayer || checkingtheZone)
        {
            currentSpeed = Chasing_maxSpeed * speedCurve.Evaluate(speedCurve_CurrentTime / speedCurve_MaxTime);
        }
        else
        {
            currentSpeed = maxSpeed * speedCurve.Evaluate(speedCurve_CurrentTime / speedCurve_MaxTime);
        }
        
        agent.speed = currentSpeed;
    }

    public bool CheckIfPlayerisNearby()
    {
        if (!checkingtheZone) return false;
        Collider2D[] guardsinRdaius = Physics2D.OverlapCircleAll(transform.position, 2);
        for (int x = 0; x < guardsinRdaius.Length; x++)
        {
            Transform guard = guardsinRdaius[x].transform;

            if (guard.name != this.name && ((1 << guard.gameObject.layer) & playerMask) != 0)
            {
                pointToGo.Clear();
                pointToGo.Add(ObjectRefs.Instance.player.transform.position);
                agent.SetDestination(ObjectRefs.Instance.player.transform.position);

                ChasinPlayerIni();
                return true;
            }
        }
        return false;
    }


    public void PlayerNoiseDetected(Vector3 position)
    {
        if (!chasingPlayer)
        {
            pointToGo = new List<Vector3>();
            pointToGo.Clear();
            pointToGo.Add(position);
            if (agent == null)
            {
                agent = GetComponent<NavMeshAgent>();
                if (spawnedIA)
                {
                    patrollPoints_List = new List<Transform>();
                    patrollPoints_List.Add(this.transform);
                }

            }
            agent.SetDestination(position);
            CheckingZoneIni();
            //checkingZoneSprite.enabled = true;
        }
    }

    public void GoingToFirstPoint()
    {
        if (pointToGo.Count > 0)
        {
            agent.SetDestination(pointToGo[0]);
        }

    }

    public bool CheckIfFirstPointReached()
    {
        if (pointToGo.Count > 0 && Vector3.Distance(transform.position, pointToGo[0]) < distancePointsCheck)
        {
            if ((checkingtheZone || inAlertMode) && !turningInPlace)
            {
                turningInPlace = true;
                turningDelay_tmp = turningDelay;
            }
            if (!checkingtheZone)
            {
                speedCurve_CurrentTime = 0;
            }

            return true;
        }
        return false;
    }

    public bool isTurning()
    {
        return turningInPlace;
    }

    public void Turn()
    {
        if (turningDelay_tmp < 0)
        {
            turningInPlace = false;
            GoToNextPoint();
        }
        else
        {
            turningDelay_tmp -= Time.deltaTime;
        }

        stationaryDegreeRotation += 1;
        stationaryDirection = (Vector2)(Quaternion.Euler(0, 0, stationaryDegreeRotation) * Vector2.right);
    }

    public void GoToNextPoint()
    {
        if (pointToGo.Count > 1)
        {
            pointToGo.RemoveAt(0);
        }
        else
        {
            if (checkingtheZone)
            {
                checkingtheZone = false;
                inAlertMode = true;
                alertModeTime_tmp = alertModeTime;
                //checkingZoneSprite.enabled = false;
            }
            SetUpPatrollPoints();
        }
    }

    public bool isInAlertMode()
    {
        return inAlertMode;
    }

    public void SetUpPatrollPoints()
    {
        pointToGo.Clear();
        for (int x = 0; x < patrollPoints_List.Count; ++x)
        {
            pointToGo.Add(patrollPoints_List[x].position);
        }
    }

    public bool IsCheckingTheZone()
    {
        if (pointToGo.Count > 0 && checkingtheZone)
        {
            return true;
        }
        return false;
    }

    public bool canSeeDonut()
    {
        if (fow.donutList.Count > 0 || eatingDonut)
        {
            if (!eatingDonut)
            {
                donutRef = fow.donutList[0].gameObject;
            }
            return true;
        }
        return false;
    }

    public void gotoDonut()
    {
        if (newDestination == Vector2.zero)
        {
            newDestination = fow.donutList[0].position - transform.position;
            stationaryDirection = newDestination;
            newDestination = newDestination - newDestination.normalized + transform.position.ToVector2();
        }
        agent.SetDestination(newDestination);
    }

    public void EatingDonut()
    {
        if (eatingDonutDelay_tmp < 0)
        {
            newDestination = Vector2.zero;
            Destroy(donutRef);
            eatingDonut = false;
            stationaryDirection = new Vector2(1, 0);
        }
        float delta = Time.deltaTime;
        eatingDonutDelay_tmp -= delta;
        donutRef.transform.GetChild(0).position = new Vector2(donutRef.transform.GetChild(0).position.x + (eatingDonutDelay / 27 * delta), donutRef.transform.GetChild(0).position.y);
    }

    public bool isDonutReached()
    {
        if (eatingDonut || Vector3.Distance(newDestination, transform.position) < distancePointsCheck)
        {
            if (!eatingDonut)
            {
                eatingDonut = true;
                eatingDonutDelay_tmp = eatingDonutDelay;
            }
            return true;
        }
        return false;
    }

    public bool canSeePlayer()
    {
        if(fow.objToCheckList.Count > 0)
        {
            if (!chasingPlayer)
            {
                //TODO : SPREAD
                Collider2D[] guardsinRdaius = Physics2D.OverlapCircleAll(transform.position, spreadRadius);
                for (int x = 0; x < guardsinRdaius.Length; x++)
                {

                    Transform guard = guardsinRdaius[x].transform;

                    if (guard.name != this.name && ((1 << guard.gameObject.layer) & guardMask) != 0)
                    {
                        if (Outils.CalculatePathLength(guard.position, transform.position, this.gameObject) < spreadRadius * 1.5)
                        {
                            guard.GetComponent<GuardIAController_v2>().PlayerNoiseDetected(fow.objToCheckList[0].position);
                        }
                    }
                }
            }

            pointToGo.Clear();
            pointToGo.Add(fow.objToCheckList[0].position);
            agent.SetDestination(fow.objToCheckList[0].position);

            ChasinPlayerIni();

            return true;
        }
        else
        {
            if (chasingPlayer)
            {
                switch (guardType)
                {
                    case GUARD_TYPE.Normal:
                        CheckingZoneIni();
                        break;
                    case GUARD_TYPE.Elite:
                        Bounds CameraBound = OrthographicBounds(Camera.main);
                        CameraBound.center = new Vector3(CameraBound.center.x, CameraBound.center.y, 0);
                        CameraBound.size = new Vector3(CameraBound.size.x, CameraBound.size.y, 0);
                        Vector2 IABound = (Vector2)transform.position;
                        if (!CameraBound.Contains(IABound))
                        {
                            chasingPlayer = false;
                            inAlertMode = true;

                            playerSpottedSprite.enabled = false;
                            checkingZoneSprite.enabled = true;

                            GetComponent<FOV_vBT>().viewAngle = alertviewAngle;

                            alertModeTime_tmp = alertModeTime;
                            SetUpPatrollPoints();
                        }
                        else
                        {
                            pointToGo.Clear();
                            pointToGo.Add(ObjectRefs.Instance.player.transform.position);
                            agent.SetDestination(ObjectRefs.Instance.player.transform.position);
                        }

                        break;
                }

            }
            if (!chasingPlayer && !checkingtheZone)
            {
                GetComponent<FOV_vBT>().viewAngle = baseviewAngle;
            }
            return false;
        }
    }


    public Bounds OrthographicBounds(Camera camera)
    {
        float screenAspect = (float)Screen.width / (float)Screen.height;
        float cameraHeight = camera.orthographicSize * 2;
        Bounds bounds = new Bounds(
            camera.transform.position,
            new Vector3(cameraHeight * screenAspect, cameraHeight, 0));
        return bounds;
    }

    public void ChasinPlayerIni()
    {

        turningInPlace = false;

        chasingPlayer = true;
        playerSpottedSprite.enabled = true;

        //Donut Clear
        eatingDonut = false;

        //Seeking Zone
        checkingZoneSprite.enabled = false;

        GetComponent<FOV_vBT>().viewAngle = alertviewAngle;
    }

    public void IASpawned(Vector3 positiontoGo)
    {
        pointToGo.Clear();
        pointToGo.Add(positiontoGo);
        CheckingZoneIni();
    }

    public void CheckingZoneIni()
    {
        checkingtheZone = true;
        chasingPlayer = false;

        playerSpottedSprite.enabled = false;
        checkingZoneSprite.enabled = true;

        GetComponent<FOV_vBT>().viewAngle = alertviewAngle;
        //Points the Check INI
        for (int x = 0; x < checkZone_NumPoints; ++x)
        {
            Bounds bounds_tmp = new Bounds(pointToGo[0], checkZoneSize);
            Vector3 pointTmp = new Vector3(0, 0, 0);
            bool check = false;
            int checkNum = 0;
            while (!check && checkNum < 10)
            {
                pointTmp = Outils.RandomPointInBounds(bounds_tmp);
                check = Outils.IsPointRecheable(pointTmp, pointToGo[0], this.gameObject, collidersCheckZone);
                ++checkNum;
            }
            if (checkNum < 10)
            {
                pointToGo.Add(pointTmp);
            }

        }
    }

    public void Test()
    {
        Debug.Log("Test Tree Methods");
    }

}
