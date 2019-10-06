using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FOWMesh : MonoBehaviour
{

    FieldOfView_2D fow;
    Mesh mesh;
    RaycastHit2D hit;
    [SerializeField] float meshRes = 4;
    [HideInInspector] public Vector3[] vertices;
    [HideInInspector] public int[] triangles;
    [HideInInspector] public int stepCount;

    private void Start()
    {
        mesh = GetComponent<MeshFilter>().mesh;
        fow = GetComponentInParent<FieldOfView_2D>();
    }

    private void LateUpdate()
    {
        MakeMesh();
    }

    void MakeMesh()
    {
        stepCount = Mathf.RoundToInt(fow.viewAngle * meshRes);

        float stepAngle = fow.viewAngle / stepCount;

        List<Vector3> viewVertex = new List<Vector3>();

        hit = new RaycastHit2D();

        for (int i = 0; i <= stepCount; i++)
        {
            float angle = fow.transform.eulerAngles.y - fow.viewAngle / 2 + stepAngle * i;
            Vector3 dir = fow.DirFromAngle(angle, false);
            hit = Physics2D.Raycast(fow.transform.position, dir, fow.viewRadius, fow.obstacleMask);

            if (hit.collider == null)
            {
                viewVertex.Add(transform.position + dir.normalized * fow.viewRadius);
            }
            else
            {
                viewVertex.Add(transform.position + dir.normalized * hit.distance);
            }
        }

        int vertexCout = viewVertex.Count + 1;

        vertices = new Vector3[vertexCout];
        triangles = new int[(vertexCout - 2) * 3];

        vertices[0] = Vector3.zero;

        for (int i = 0; i < vertexCout -1; i++) {
            vertices[i + 1] = transform.InverseTransformPoint(viewVertex[i]);

            if (i < vertexCout - 2)
            {
                triangles[i * 3 + 2] = 0;
                triangles[i * 3 + 1] = i + 1;
                triangles[i * 3] = i + 2;
            }


        }

        mesh.Clear();

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
    }
}
