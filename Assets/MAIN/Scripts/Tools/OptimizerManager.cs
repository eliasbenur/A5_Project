using BehaviorDesigner.Runtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptimizerManager : MonoBehaviour
{
    Transform toFollow;
    public List<GameObject> guardList;
    // Start is called before the first frame update
    void Start()
    {
        toFollow = ObjectRefs.Instance.player.transform;
        Bounds CameraBound = Outils.OrthographicBounds(Camera.main);
        GetComponent<BoxCollider2D>().size = CameraBound.size + new Vector3(20,20,0);

    }

    // Update is called once per frame
    void Update()
    {
        transform.position = toFollow.position;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 11) // Guard
        {
            guardList.Add(collision.gameObject);
            collision.gameObject.GetComponent<FOV_vBT>().enabled = true;
            collision.gameObject.GetComponent<BehaviorTree>().enabled = true;
            collision.gameObject.GetComponent<GuardIAController_v2>().enabled = true;
        }   
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 11) // Guard
        {
            guardList.Remove(collision.gameObject);
            collision.gameObject.GetComponent<FOV_vBT>().enabled = false;
            collision.gameObject.GetComponent<BehaviorTree>().enabled = false;
            collision.gameObject.GetComponent<GuardIAController_v2>().enabled = false;
        }
    }
}
