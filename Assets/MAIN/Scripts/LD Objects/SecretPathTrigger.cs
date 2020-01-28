using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SecretPathTrigger : MonoBehaviour
{
    public LayerMask playerMask;

    public GameObject Tiles, TilesColl;
    public GameObject TileMap;
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
            Tiles.GetComponent<Tilemap>().color = new Color(1, 1, 1, 1);
            Tilemap[] tilesMaps_tmp = TileMap.GetComponentsInChildren<Tilemap>();
            for (int x = 0; x < tilesMaps_tmp.Length; ++x)
            {
                tilesMaps_tmp[x].color = new Color(1,1,1, 0.4f);
            }


        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (((1 << collision.gameObject.layer) & playerMask) != 0)
        {
            Tiles.GetComponent<Tilemap>().color = new Color(1, 1, 1, 0.4f);
            Tilemap[] tilesMaps_tmp = TileMap.GetComponentsInChildren<Tilemap>();
            for (int x = 0; x < tilesMaps_tmp.Length; ++x)
            {
                tilesMaps_tmp[x].color = new Color(1, 1, 1, 1);
            }
        }
    }
}
