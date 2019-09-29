using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using RVO;

public class RVOAgent : MonoBehaviour
{

    [SerializeField]
    UnityEngine.Vector2 target;
    public bool canMove;

    Seeker agentSeeker;
    private List<Vector3> pathNodes = null;
    RVOSimulator simulator = null;
    int agentIndex = -1;
    int currentNodeInThePath = 0;
    bool isAbleToStart = false;

    public float updatePathtime;
    float updatePathtime_tmp;

    public void setTarget(UnityEngine.Vector2 target_)
    {
        currentNodeInThePath = 0;
        pathNodes = new List<Vector3>();
        target = target_;
        agentSeeker.StartPath(transform.position, target, OnPathComplete);
    }
    IEnumerator UpdatePathTarget()
    {
        var path = agentSeeker.StartPath(transform.position, target, OnPathComplete);
        yield return StartCoroutine(path.WaitForPath());
        if (pathNodes != null && pathNodes.Count > 0)
            pathNodes.Remove(pathNodes[0]);
        currentNodeInThePath = 0;
    }

    void Awake()
    {
        canMove = false;
    }

    IEnumerator Start()
    {
        currentNodeInThePath = 0;
        simulator = GameObject.FindGameObjectWithTag("RVOSim").GetComponent<RVOSimulator>();
        pathNodes = new List<Vector3>();
        yield return StartCoroutine(StartPaths());
        agentIndex = simulator.addAgentToSim(transform.position, gameObject, pathNodes);

        if (updatePathtime <= 0)
        {
            Debug.Log("Incorrect Value of updatePathtime, setted to 1");
            updatePathtime = 1;
        }
        updatePathtime_tmp = updatePathtime;

        isAbleToStart = true;
    }
    IEnumerator StartPaths()
    {
        agentSeeker = gameObject.GetComponent<Seeker>();
        var path = agentSeeker.StartPath(transform.position, target, OnPathComplete);
        yield return StartCoroutine(path.WaitForPath());
    }
    public void OnPathComplete(Path p)
    {
        if (p.error)
        {
            Debug.Log("" + this.gameObject.name + " ---- -" + p.errorLog);
        }
        else
        {
            pathNodes = p.vectorPath;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isAbleToStart && agentIndex != -1)
        {
            if (canMove)
            {
                transform.position = toUnityVector(simulator.getAgentPosition(agentIndex));
                /*if (gameObject.name == "IA")
                {
                    Debug.Log(Simulator.Instance.getAgentPrefVelocity(agentIndex));
                }
                UnityEngine.Vector2 newVelocity = (toUnityVector(calculateNextStation()) - (UnityEngine.Vector2)transform.position).normalized;
                Simulator.Instance.setAgentPrefVelocity(agentIndex, toRVOVector(newVelocity));*/

                if (updatePathtime_tmp >= 0)
                {
                    updatePathtime_tmp -= Time.deltaTime;
                }
                else
                {
                    updatePathtime_tmp = updatePathtime;
                    if (agentSeeker.IsDone()) StartCoroutine(UpdatePathTarget());
                }
            }
        }
    }

    public RVO.Vector2 calculateNextStation()
    {
        Vector3 station;
        if (currentNodeInThePath < pathNodes.Count)
        {
            station = pathNodes[currentNodeInThePath];
            Debug.Log(transform.position + "   ///   " + station);
            float distance = Vector3.Distance(station, transform.position);
            if ( distance < 1f)
            {
                station = pathNodes[currentNodeInThePath];
                currentNodeInThePath++;
            }
        }
        else
        {
            station = pathNodes[pathNodes.Count - 1];
        }
        return toRVOVector(station);
    }


    RVO.Vector2 toRVOVector(Vector3 param)
    {
        return new RVO.Vector2(param.x, param.y);
    }

    UnityEngine.Vector2 toUnityVector(RVO.Vector2 param)
    {
        return new UnityEngine.Vector2(param.x(), param.y());
    }

}
