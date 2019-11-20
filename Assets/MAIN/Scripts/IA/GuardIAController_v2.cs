using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public enum GUARD_TYPE { Normal, Elite };

public class GuardIAController_v2 : MonoBehaviour
{

    NavMeshAgent agent;
    public List<Transform> patrollPoints_List;
    public List<Vector3> pointToGo;
    public float distancePointsCheck;

    FOV_vBT fow;

    public Vector2 stationaryDirection;
    public Quaternion stationaryAngle;
    public float stationaryDegreeRotation;

    public Vector2 checkZoneSize;

    public int checkZone_NumPoints;

    public bool checkingtheZone = false;
    public bool chasingPlayer = false;
    public bool turningInPlace = false;
    public bool inAlertMode = false;

    public LayerMask collidersCheckZone;
    public float turningDelay;
    public float turningDelay_tmp;

    public SpriteRenderer checkingZoneSprite;
    public SpriteRenderer playerSpottedSprite;

    public bool eatingDonut;
    public float eatingDonutDelay;
    public float eatingDonutDelay_tmp;
    public GameObject donutRef;

    //Spread
    public GameObject spreadRef;
    public LayerMask guardMask;
    public float spreadRadius;

    //Angles - FOV
    public float baseviewAngle;
    public float alertviewAngle;

    public AnimationCurve speedCurve;
    public float speedCurve_CurrentTime;
    public float speedCurve_MaxTime;
    public float currentSpeed;
    public float maxSpeed;
    public float Chasing_maxSpeed;

    public LayerMask playerMask;

    public float alertModeTime;
    public float alertModeTime_tmp;

    public GUARD_TYPE guardType;


    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;

        fow = GetComponent<FOV_vBT>();

        turningDelay_tmp = -1;
        alertModeTime_tmp = -1;
    }

    // Update is called once per frame
    void Update()
    {
        SpeedUpdate();
        AlertModeUpdate();
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

    public void CheckIfPlayerisNearby()
    {
        if (!checkingtheZone) return;
        Collider2D[] guardsinRdaius = Physics2D.OverlapCircleAll(transform.position, 3);
        for (int x = 0; x < guardsinRdaius.Length; x++)
        {
            Transform guard = guardsinRdaius[x].transform;

            if (guard.name != this.name && ((1 << guard.gameObject.layer) & playerMask) != 0)
            {
                pointToGo.Clear();
                pointToGo.Add(ObjectRefs.Instance.player.transform.position);
                agent.SetDestination(ObjectRefs.Instance.player.transform.position);

                ChasinPlayerIni();
            }
        }
    }


    public void PlayerNoiseDetected(Vector3 position)
    {
        if (!checkingtheZone && !chasingPlayer)
        {
            pointToGo.Clear();
            pointToGo.Add(position);
            agent.SetDestination(position);
            CheckingZoneIni();
            //checkingZoneSprite.enabled = true;
        }
    }

    public void GoingToFirstPoint()
    {
        agent.SetDestination(pointToGo[0]);
    }

    public bool CheckIfFirstPointReached()
    {
        if (Vector3.Distance(transform.position, pointToGo[0]) < distancePointsCheck)
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
        Debug.Log("Turning");
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
        agent.SetDestination(fow.donutList[0].position);
    }

    public void EatingDonut()
    {
        if (eatingDonutDelay_tmp < 0)
        {
            Destroy(donutRef);
            eatingDonut = false;
        }
        eatingDonutDelay_tmp -= Time.deltaTime;
    }

    public bool isDonutReached()
    {
        if (eatingDonut || Vector3.Distance(fow.donutList[0].position, transform.position) < distancePointsCheck)
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


        chasingPlayer = true;
        playerSpottedSprite.enabled = true;

        //Donut Clear
        eatingDonut = false;

        //Seeking Zone
        checkingZoneSprite.enabled = false;

        GetComponent<FOV_vBT>().viewAngle = alertviewAngle;
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
            while (!check)
            {
                pointTmp = Outils.RandomPointInBounds(bounds_tmp);
                check = Outils.IsPointRecheable(pointTmp, pointToGo[0], this.gameObject, collidersCheckZone);
            }
            pointToGo.Add(pointTmp);
        }
    }

    public void Test()
    {
        Debug.Log("Test Tree Methods");
    }

}
