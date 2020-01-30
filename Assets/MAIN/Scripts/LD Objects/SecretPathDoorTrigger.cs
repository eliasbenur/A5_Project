using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SecretPathDoorTrigger : MonoBehaviour
{
    public LayerMask playerMask;

    public GameObject Door;

    public BoxCollider2D DisabledCollider;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (((1 << collision.gameObject.layer) & playerMask) != 0)
        {
            SecretPathTrigger go_tmp = gameObject.transform.parent.GetChild(2).GetComponent<SecretPathTrigger>();
            go_tmp.Tiles.GetComponent<Tilemap>().color = new Color(1, 1, 1, 1);
            Tilemap[] tilesMaps_tmp = go_tmp.TileMap.GetComponentsInChildren<Tilemap>();
            for (int x = 0; x < tilesMaps_tmp.Length; ++x)
            {
                tilesMaps_tmp[x].color = new Color(1, 1, 1, 0.4f);
            }
            Physics2D.IgnoreLayerCollision(8, 9, true);
            Physics2D.IgnoreLayerCollision(9, 17, false);

            DisabledCollider.enabled = true;

            Door.SetActive(false);
        }
    }

}
