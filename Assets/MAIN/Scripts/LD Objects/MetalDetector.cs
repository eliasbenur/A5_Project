using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Collider2D))]
public class MetalDetector : MonoBehaviour
{
    public Transform IASpawn;
    public GameObject IAPrefab;
    public int IASpawnNum;
    public float delaySpawn = 5;
    float delaySpawn_tmp;

    private void Update()
    {
        if (delaySpawn_tmp > 0)
        {
            delaySpawn_tmp -= Time.deltaTime;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerControl voleur = collision.gameObject.GetComponent<PlayerControl>();
        if (voleur!= null)
        {
            if ((voleur.stat.power == Power.Cheater && !voleur.isPowerActive()) || (voleur.stat.power != Power.Cheater))
            {
                bool metal = false;
                foreach (Tresor tre in voleur.Get_inventory())
                {
                    if (tre.materialObj == MaterialObj.Metal) metal = true;
                }
                if (metal) {
                    //GAME OVER
                    //GameManager.Instance.DetectorMetal();
                    if (IASpawn != null && delaySpawn_tmp <= 0)
                    {
                        for (int x = 0; x < IASpawnNum; ++x)
                        {
                            Vector3 whereToSpawn = Outils.RandomPointInBounds(IASpawn.gameObject.GetComponent<BoxCollider2D>().bounds);
                            GameObject IA_tmp =  Instantiate(IAPrefab, whereToSpawn, Quaternion.identity);
                            IA_tmp.GetComponent<GuardIAController>().behaviourType = ia_BehaviourType.SpawnedIA;
                            IA_tmp.GetComponent<GuardIAController>().StartChasingPlayerVarInis();
                            IA_tmp.GetComponent<NavMeshAgent>().SetDestination(ObjectRefs.Instance.player.transform.position);

                            delaySpawn_tmp = delaySpawn;
                        }
                    }
                }
            }
        }
    }
}
