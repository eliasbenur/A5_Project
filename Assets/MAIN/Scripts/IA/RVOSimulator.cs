using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using RVO;

public class RVOSimulator : MonoBehaviour
{
    List<RVO.Vector2> agentPositions;
    List<GameObject> rvoGameObj;

    public float maxSpeed;

    // Start is called before the first frame update
    void Start()
    {
        agentPositions = new List<RVO.Vector2>();
        rvoGameObj = new List<GameObject>();

        Simulator.Instance.setTimeStep(0.01f);
        Simulator.Instance.setAgentDefaults(15.0f, 10, 5.0f, 5.0f, 0.5f, 1, new RVO.Vector2(0.0f, 0.0f));
    }

    // Update is called once per frame
    void Update()
    {
        int agentNumber = Simulator.Instance.getNumAgents();

        try
        {
            for (int i = 0; i < agentNumber; i++)
            {
                RVO.Vector2 agentLoc = Simulator.Instance.getAgentPosition(i);
                RVO.Vector2 station = rvoGameObj[i].GetComponent<RVOAgent>().calculateNextStation() - agentLoc;

                if (RVOMath.absSq(station) > 1.0f)
                {
                    station = RVOMath.normalize(station);
                }

                Simulator.Instance.setAgentPrefVelocity(i, station);
                agentPositions[i] = Simulator.Instance.getAgentPosition(i);
                //Debug.Log(agentLoc + "  ///  " + agentPositions[i]);
            }
            Simulator.Instance.doStep();
        }
        catch(System.Exception ex){
            Debug.Log("Exeption: " + ex.Message);
        }
    }

    public int addAgentToSim(Vector3 pos, GameObject ag, List<Vector3> paths)
    {
        if (paths != null && paths.Count > 0)
            paths.Remove(paths[0]);

        Simulator.Instance.Clear();

        Simulator.Instance.setTimeStep(0.10f);
        /* setAgentDefaults(15.0f, 10, 5.0f, 5.0f, 0.5f, maxSpeed_, new RVO.Vector2(0.0f, 0.0f)); */
        Simulator.Instance.setAgentDefaults(15.0f, 10, 5.0f, 5.0f, 0.5f, maxSpeed, new RVO.Vector2(0.0f, 0.0f));

        int agentCount = agentPositions.Count;
        for (int i = 0; i < agentCount; i++)
        {
            Simulator.Instance.addAgent(agentPositions[i]);
        }

        rvoGameObj.Add(ag);

        agentPositions.Add(toRVOVector(pos));
        int retVal = Simulator.Instance.addAgent(toRVOVector(pos));

        return retVal;
    }

    Vector3 toUnityVector(RVO.Vector2 param)
    {
        return new Vector3(param.x(), param.y(), 0);
    }

    RVO.Vector2 toRVOVector(Vector3 param)
    {
        return new RVO.Vector2(param.x, param.y);
    }

    public RVO.Vector2 getAgentPosition(int index)
    {
        return agentPositions[index];
    }
}
