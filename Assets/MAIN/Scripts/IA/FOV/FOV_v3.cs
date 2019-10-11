using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FOV_v3 : MonoBehaviour
{
    public float viewRadius;
    [Range(0, 360)]
    public float viewAngle;

    public LayerMask obstacleMask, playerMask;
    public MeshFilter viewMeshFilter;

    public float meshResolution;
    public int edgeResolveIterations;
    public float edgeDstThreshold;
    public float maskCutawayDst = .1f;

    Mesh viewMesh;
    List<Vector3> viewPoints = new List<Vector3>();
    Vector3 edgeMinPoint, edgeMaxPoint;

    NavMeshAgent agent;
    Vector2 direction_tmp;

    Collider2D[] playerInRadius;
    public List<Transform> visiblePlayer = new List<Transform>();

    void Start()
    {
        viewMesh = new Mesh();
        viewMesh.name = "View Mesh";
        viewMeshFilter.mesh = viewMesh;

        agent = GetComponent<NavMeshAgent>();
    }

    void LateUpdate()
    {//draw field of view
        Vector2 direction;
        //Direction Set up
        if (agent == null)
        {
            direction = (Vector2)(Quaternion.Euler(0, 0, transform.eulerAngles.z) * Vector2.down);
           // Debug.Log(transform.eulerAngles.z);
            direction_tmp = direction;
        }
        else
        {
            if ((Vector2)agent.velocity.normalized == Vector2.zero)
            {
                direction = direction_tmp;
            }
            else
            {
                direction = agent.velocity.normalized;
                direction_tmp = direction;
            }
        }
        


        int stepCount = Mathf.RoundToInt(viewAngle * meshResolution);
        float stepAngleSize = viewAngle / stepCount;
        viewPoints.Clear();
        ViewCastInfo oldViewCast = new ViewCastInfo();
        for (int i = 0; i <= stepCount; i++)
        {
            //float angle = -transform.eulerAngles.z - viewAngle / 2 + stepAngleSize * i;
            float angle;
            if (direction.x < 0)
            {
                angle = -Vector2.Angle(Vector2.up, direction) - viewAngle / 2 + stepAngleSize * i;
            }
            else
            {
                angle = Vector2.Angle(Vector2.up, direction) - viewAngle / 2 + stepAngleSize * i;
            }

            ViewCastInfo newViewCast = ViewCast(angle);

            if (i > 0)
            {
                bool edgeDstThresholdExceeded = Mathf.Abs(oldViewCast.dst - newViewCast.dst) > edgeDstThreshold;
                if (oldViewCast.hit != newViewCast.hit || (oldViewCast.hit && newViewCast.hit && edgeDstThresholdExceeded))
                {
                    FindEdge(oldViewCast, newViewCast, out edgeMinPoint, out edgeMaxPoint);
                    if (edgeMinPoint != Vector3.zero)
                        viewPoints.Add(edgeMinPoint);
                    if (edgeMaxPoint != Vector3.zero)
                        viewPoints.Add(edgeMaxPoint);
                }

            }

            viewPoints.Add(newViewCast.point);
            oldViewCast = newViewCast;
        }

        int vertexCount = viewPoints.Count + 1;
        Vector3[] vertices = new Vector3[vertexCount];
        int[] triangles = new int[(vertexCount - 2) * 3];

        vertices[0] = Vector3.zero;
        for (int i = 0; i < vertexCount - 1; i++)
        {
            vertices[i + 1] = transform.InverseTransformPoint(viewPoints[i]) + Vector3.up * maskCutawayDst;

            if (i < vertexCount - 2)
            {
                triangles[i * 3] = 0;
                triangles[i * 3 + 1] = i + 1;
                triangles[i * 3 + 2] = i + 2;
            }
        }

        viewMesh.Clear();

        viewMesh.vertices = vertices;
        viewMesh.triangles = triangles;
        viewMesh.RecalculateNormals();

        FindVisiblePlayer();
    }

    void FindVisiblePlayer()
    {
        playerInRadius = Physics2D.OverlapCircleAll(transform.position, viewRadius);

        visiblePlayer.Clear();

        for (int x = 0; x < playerInRadius.Length; x++)
        {
            Transform player = playerInRadius[x].transform;
            //Debug.Log(playerMask.value);
            if (((1 << player.gameObject.layer) & playerMask) != 0)
            {
                Vector2 dirPlayer = new Vector2(player.position.x - transform.position.x, player.position.y - transform.position.y);
                if (Vector2.Angle(dirPlayer, direction_tmp.normalized) < (viewAngle / 2))
                {
                    float distancePlayer = Vector2.Distance(transform.position, player.position);

                    if (!Physics2D.Raycast(transform.position, dirPlayer, distancePlayer, obstacleMask))
                    {
                        visiblePlayer.Add(player);
                    }
                }
            }
        }
    }

    void FindEdge(ViewCastInfo minViewCast, ViewCastInfo maxViewCast, out Vector3 minPoint, out Vector3 maxPoint)
    {
        float minAngle = minViewCast.angle;
        float maxAngle = maxViewCast.angle;
        minPoint = Vector3.zero;
        maxPoint = Vector3.zero;

        for (int i = 0; i < edgeResolveIterations; i++)
        {
            float angle = (minAngle + maxAngle) / 2;
            ViewCastInfo newViewCast = ViewCast(angle);

            bool edgeDstThresholdExceeded = Mathf.Abs(minViewCast.dst - newViewCast.dst) > edgeDstThreshold;
            if (newViewCast.hit == minViewCast.hit && !edgeDstThresholdExceeded)
            {
                minAngle = angle;
                minPoint = newViewCast.point;
            }
            else
            {
                maxAngle = angle;
                maxPoint = newViewCast.point;
            }
        }
    }

    ViewCastInfo ViewCast(float globalAngle)
    {
        Vector3 dir = DirFromAngle(globalAngle);
        var hit = Physics2D.Raycast(transform.position, dir, viewRadius, obstacleMask);

        if (hit)
            return new ViewCastInfo(true, hit.point, hit.distance, globalAngle);

        return new ViewCastInfo(false, transform.position + dir * viewRadius, viewRadius, globalAngle);
    }

    public Vector3 DirFromAngle(float angleInDegrees)
    {
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), Mathf.Cos(angleInDegrees * Mathf.Deg2Rad), 0);
    }

    public struct ViewCastInfo
    {
        public bool hit;
        public Vector3 point;
        public float dst;
        public float angle;

        public ViewCastInfo(bool _hit, Vector3 _point, float _dst, float _angle)
        {
            hit = _hit;
            point = _point;
            dst = _dst;
            angle = _angle;
        }
    }
}
