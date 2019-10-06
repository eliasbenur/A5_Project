﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerNoise : MonoBehaviour
{
    public float noiseRadius;

    CircleCollider2D circlecoll;

    public List<GameObject> guardList = new List<GameObject>();

    public LayerMask toNotify;

    

    // Start is called before the first frame update
    void Start()
    {
        circlecoll = GetComponent<CircleCollider2D>();
        if (circlecoll == null)
        {
            circlecoll = gameObject.AddComponent<CircleCollider2D>();
        }
        circlecoll.isTrigger = true;
        circlecoll.radius = noiseRadius;
    }

    // Update is called once per frame
    void Update()
    {
        circlecoll.radius = noiseRadius;
        NoiseUpdate();
    }

    void NoiseUpdate()
    {
        if (guardList.Count > 0)
        {
            for (int x = 0; x < guardList.Count; x++)
            {
                if (CalculatePathLength(transform.position, guardList[x]) < noiseRadius)
                {
                    guardList[x].GetComponent<GuardIAController>().PlayerNoiseDetected(transform.position);
                }
            }

        }
    }

    float CalculatePathLength(Vector3 targetPosition, GameObject originalObject)
    {
        NavMeshPath path = new NavMeshPath();
        NavMeshAgent nav = originalObject.GetComponent<NavMeshAgent>();

        if (nav.enabled)
            nav.CalculatePath(targetPosition, path);
        Vector3[] allWayPoints = new Vector3[path.corners.Length + 2];

        allWayPoints[0] = transform.position;
        allWayPoints[allWayPoints.Length - 1] = targetPosition;

        for (int i=0; i<path.corners.Length; i++)
        {
            allWayPoints[i + 1] = path.corners[i];
        }

        float pathLength = 0f;

        for (int i=0; i < allWayPoints.Length -1; i++)
        {
            pathLength += Vector3.Distance(allWayPoints[i], allWayPoints[i + 1]);
        }

        return pathLength / 2;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (((1 << collision.gameObject.layer) & toNotify) != 0)
        {
            guardList.Add(collision.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (((1 << collision.gameObject.layer) & toNotify) != 0)
        {
            guardList.Remove(collision.gameObject);
        }
    }
}