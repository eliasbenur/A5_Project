using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoiseParticleTrigger : MonoBehaviour
{
    public LayerMask guardLayer;
    void OnParticleCollision(GameObject other)
    {
        
        if (((1 << other.layer) & guardLayer) != 0)
        {
            other.GetComponent<GuardIAController>().PlayerNoiseDetected(ObjectRefs.Instance.player.transform.position);
        }
    }

}
