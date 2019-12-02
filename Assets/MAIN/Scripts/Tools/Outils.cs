using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public static class Outils
{
    public static Vector2 RandomPointInBounds(Bounds bounds)
    {
        return new Vector2(
            Random.Range(bounds.min.x, bounds.max.x),
            Random.Range(bounds.min.y, bounds.max.y));
    }

    public static bool GameObjectExistInArray(GameObject[] list_objs, GameObject gameobj)
    {
        foreach (GameObject list_obj in list_objs)
        {
            if (list_obj.Equals(gameobj))
            {
                return true;
            }
        }
        return false;
    }


    public static bool IsPointRecheable(Vector3 targetPosition,Vector3 originPosition, GameObject originalObject, LayerMask layerM)
    {
        NavMeshPath path = new NavMeshPath();
        NavMeshAgent nav = originalObject.GetComponent<NavMeshAgent>();

        bool nose = false;
        if (nav.enabled)
        {
            //nav.CalculatePath(targetPosition, path);
            nose = NavMesh.CalculatePath(originPosition, targetPosition, NavMesh.AllAreas, path);
        }
        if (!nose)
        {
            return false;
        }
            
        Vector3[] allWayPoints = new Vector3[path.corners.Length + 2];

        //allWayPoints[0] = transform.position;
        allWayPoints[0] = originPosition;
        allWayPoints[allWayPoints.Length - 1] = targetPosition;

        for (int i = 0; i < path.corners.Length; i++)
        {
            allWayPoints[i + 1] = path.corners[i];
        }

        float pathLength = 0f;

        for (int i = 0; i < allWayPoints.Length - 1; i++)
        {
            //Debug.Log(NavMesh.CalculatePath(allWayPoints[i], allWayPoints[i + 1], NavMesh.AllAreas, path));
            RaycastHit2D hit = Physics2D.Raycast(allWayPoints[i], allWayPoints[i + 1] - allWayPoints[i], Vector2.Distance(allWayPoints[i], allWayPoints[i + 1]), layerM);
            Debug.DrawLine(allWayPoints[i], allWayPoints[i + 1], Color.red,9999);
            if (hit.collider != null)
            {
                return false;
            }
            else
            {
                pathLength += Vector3.Distance(allWayPoints[i], allWayPoints[i + 1]);
            }

        }

        //return pathLength / 2;
        return true;
    }

    public static float CalculatePathLength(Vector3 targetPosition, Vector3 originalPosition, GameObject originalObject)
    {
        NavMeshPath path = new NavMeshPath();
        NavMeshAgent nav = originalObject.GetComponent<NavMeshAgent>();

        if (nav.enabled)
            nav.CalculatePath(targetPosition, path);
        Vector3[] allWayPoints = new Vector3[path.corners.Length + 2];

        allWayPoints[0] = originalPosition;
        allWayPoints[allWayPoints.Length - 1] = targetPosition;

        for (int i = 0; i < path.corners.Length; i++)
        {
            allWayPoints[i + 1] = path.corners[i];
        }

        float pathLength = 0f;

        for (int i = 0; i < allWayPoints.Length - 1; i++)
        {
            pathLength += Vector3.Distance(allWayPoints[i], allWayPoints[i + 1]);
        }

        return pathLength / 2;
    }

    public static Bounds OrthographicBounds(Camera camera)
    {
        float screenAspect = (float)Screen.width / (float)Screen.height;
        float cameraHeight = camera.orthographicSize * 2;
        Bounds bounds = new Bounds(
            camera.transform.position,
            new Vector3(cameraHeight * screenAspect, cameraHeight, 0));
        return bounds;
    }
}
