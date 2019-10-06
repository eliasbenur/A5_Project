using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(PolygonCollider2D))]
public class FieldofView : MonoBehaviour
{
    // Start is called before the first frame update

    public float range;
    [Range (0,360)]
    public float angle;

    PolygonCollider2D polygoncoll;
    NavMeshAgent agent;

    Vector2 direction_tmp;

    public List<GameObject> objectsinView;
    public LayerMask layerMask;
    void Start()
    {
        polygoncoll = GetComponent<PolygonCollider2D>();
        agent = GetComponent<NavMeshAgent>();
        UpdateFOVPoints();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateFOVPoints();
        CheckObjects();
    }

    void CheckObjects()
    {
        objectsinView.Clear();

        Collider2D[] tmp = new Collider2D[10];
        ContactFilter2D contactFilter = new ContactFilter2D();
        contactFilter.SetLayerMask(layerMask);
        Physics2D.OverlapCollider(polygoncoll, contactFilter, tmp);
        for (int x = 0; x < tmp.Length; x++)
        {
            if (tmp[x] != null)
            {
                objectsinView.Add(tmp[x].gameObject);
            }
            else
            {
                x = tmp.Length;
            }
        }
    }

    void UpdateFOVPoints()
    {
        Vector2 direction;
        if ((Vector2)agent.velocity.normalized == Vector2.zero)
        {
            direction = direction_tmp;
        }
        else
        {
            direction = agent.velocity.normalized;
            direction_tmp = direction;
        }
        Vector2[] points = new Vector2[6];
        points[0] = Vector2.zero;
        points[1] = Vector2Extension.Rotate(direction, (angle) / 2) * range;
        points[2] = Vector2Extension.Rotate(direction, (angle) / 4) * range;
        points[3] = direction * range;
        points[4] = Vector2Extension.Rotate(direction, (-angle) / 4) * range;
        points[5] = Vector2Extension.Rotate(direction, (-angle) / 2) * range;
        polygoncoll.SetPath(0, points);

    }
}

public static class Vector2Extension
{
    public static Vector2 Rotate(this Vector2 v, float degrees)
    {
        float sin = Mathf.Sin(degrees * Mathf.Deg2Rad);
        float cos = Mathf.Cos(degrees * Mathf.Deg2Rad);

        float tx = v.x;
        float ty = v.y;
        v.x = (cos * tx) - (sin * ty);
        v.y = (sin * tx) + (cos * ty);
        return v;
    }
}
