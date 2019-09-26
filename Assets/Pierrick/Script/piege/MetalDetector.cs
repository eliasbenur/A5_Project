using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class MetalDetector : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerControl voleur = collision.gameObject.GetComponent<PlayerControl>();
        if (voleur!= null)
        {
            bool metal = false;
            foreach(Tresor tre in voleur.inventory)
            {
                if (tre.materialObj == MaterialObj.Metal) metal = true;
            }
            if (metal) { GameManager.Instance.DetectorMetal(); }

        }
    }
}
