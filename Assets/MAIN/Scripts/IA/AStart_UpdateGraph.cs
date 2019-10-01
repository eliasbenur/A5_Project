using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStart_UpdateGraph : MonoBehaviour
{
    public float updateTime;
    float updateTime_tmp;
    // Start is called before the first frame update
    void Start()
    {
        if (updateTime == 0)
        {
            Debug.Log("Update Time cant be set to 0, setted to 1s");
            updateTime = 1;
        }
        updateTime_tmp = updateTime;
    }

    // Update is called once per frame
    void Update()
    {

        if (updateTime_tmp > 0)
        {
            updateTime_tmp -= Time.deltaTime;
        }
        else
        {
            Bounds bounds = GetComponent<BoxCollider2D>().bounds;
            AstarPath.active.UpdateGraphs(bounds);
            updateTime_tmp = updateTime;
        }
    }
}
