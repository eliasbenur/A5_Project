using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerNoise : MonoBehaviour
{
    public Transform toFollow;
    public float noiseRadius;
    CircleCollider2D circlecoll;

    List<GameObject> guardList = new List<GameObject>();

    public LayerMask toNotify;

    public GameObject particleSystemPrefab;
    ParticleSystem particleSystem;

    public float delayNoiseWave;
    float delayNoiseWave_tmp;

    PlayerControl player;

    

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

        GameObject go_tmp = Instantiate(particleSystemPrefab, transform.position, Quaternion.identity);
        go_tmp.transform.parent = transform;
        particleSystem = go_tmp.GetComponent<ParticleSystem>();

        delayNoiseWave_tmp = delayNoiseWave;

        player = ObjectRefs.Instance.player.GetComponent<PlayerControl>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (toFollow != null)
        {
            transform.position = toFollow.position + new Vector3(0,-0.3f,0);
        }
        circlecoll.radius = noiseRadius;
        var main = particleSystem.main;
        main.startSpeed = noiseRadius * 2;

        if (delayNoiseWave_tmp > 0)
        {
            if (player.moveVector.sqrMagnitude > 0)
            {
                delayNoiseWave_tmp -= Time.fixedDeltaTime  * (player.moveVector.sqrMagnitude/1);
            }

        }
        else
        {
            delayNoiseWave_tmp = delayNoiseWave;
            particleSystem.Play();
            ObjectRefs.Instance.soungManager.PlaymovementSnd();
            NoiseUpdate();
        }
    }

    void NoiseUpdate()
    {
        if (guardList.Count > 0)
        {
            for (int x = 0; x < guardList.Count; x++)
            {
                if (CalculatePathLength(transform.position, guardList[x]) < noiseRadius)
                {
                    StartCoroutine(AlertGuard(guardList[x]));
                }
            }
        }
    }

    IEnumerator AlertGuard(GameObject guard)
    {
        yield return new WaitForSeconds(0.2f);
        guard.GetComponent<GuardIAController>().PlayerNoiseDetected(transform.position);
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
