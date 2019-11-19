using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class MapControl : MonoBehaviour
{
    Player player;

    public float minDist = 10;
    public float maxDist = 30;

    public float minX;
    public float maxX;

    public float minY;
    public float maxY;

    public float[] levels;
    public int currentlevels = 2;

    public GameObject fullMapCamera;
    public GameObject player_GameObject;

    void Start()
    {
        ReInput.players.GetPlayer(0);
        levels = new float[5];
        levels[0] = -minDist;
        levels[1] = -((minDist + maxDist) / 4);
        levels[2] = -((minDist + maxDist) / 2);
        levels[3] = -((minDist + maxDist) / 4 * 3);
        levels[4] = -maxDist;

        Vector3 pos = player_GameObject.transform.position;
        fullMapCamera.transform.position = new Vector3(Mathf.Clamp(pos.x, minX, maxX), Mathf.Clamp(pos.y, minY, maxY), levels[currentlevels]);

    }

    void Update()
    {
        
    }
}
