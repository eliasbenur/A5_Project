using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class MapControl : MonoBehaviour
{
    Player player;

    public float minClamp = 5;
    public float maxClamp = 17.1f;
    public float zoomSpeedMod = 5;
    public float[] zoomLevels;
    public int level = 2;
    public bool returnZero = true;

    public float speedMod = 5;

    public BoxCollider2D Bounds;
    public Camera fullMapCamera;
    public GameObject player_GameObject;

    public Vector2 move_Vector;
    public float zoom_Vector;

    public void Start()
    {
        player = ReInput.players.GetPlayer(0);

        UpdateCameraLevel();
        Vector3 pos = player_GameObject.transform.position;
        fullMapCamera.transform.position = new Vector3(pos.x, pos.y, fullMapCamera.transform.position.z);

        zoomLevels = new float[5];
        zoomLevels[0] = minClamp;
        zoomLevels[1] = ((minClamp + maxClamp) / 3);
        zoomLevels[2] = ((minClamp + maxClamp) / 2);
        zoomLevels[3] = ((minClamp + maxClamp) / 3 * 2);
        zoomLevels[4] = maxClamp;

        fullMapCamera.orthographicSize = zoomLevels[level];

    }

    void Update()
    {
        move_Vector.x = player.GetAxis("Horizontal");
        move_Vector.y = player.GetAxis("Vertical");
        zoom_Vector = -player.GetAxis("Zoom");
        if (move_Vector.sqrMagnitude > 1)
        {
            move_Vector.Normalize();
        }

        if (move_Vector != Vector2.zero)
        {
            fullMapCamera.transform.position += (Vector3)move_Vector * (0.02f * speedMod);
        }

        if (zoom_Vector != 0 && returnZero)
        {
            if (zoom_Vector > 0 && level < 4)
            {
                level++;
                fullMapCamera.orthographicSize = zoomLevels[level];
            }
            if (zoom_Vector < 0 && level > 0)
            {
                level--;
                fullMapCamera.orthographicSize = zoomLevels[level];
            }
            //fullMapCamera.orthographicSize += zoom_Vector * zoomSpeedMod;
            //fullMapCamera.orthographicSize = Mathf.Clamp(fullMapCamera.orthographicSize, 5, 17.3f);
            returnZero = false;
        }
        else
        {
            if (zoom_Vector == 0)
                returnZero = true;
        }
        UpdateCameraLevel();

        /*if (player.GetButtonDown("ZoomOut") && fullMapCamera.GetComponent<Camera>().orthographicSize < maxDist)
        {
            fullMapCamera.GetComponent<Camera>().orthographicSize += zoomSpeed;
            UpdateCameraLevel();
        }
        if (player.GetButtonDown("ZoomIn"))
        {
            fullMapCamera.GetComponent<Camera>().orthographicSize -= zoomSpeed;
            UpdateCameraLevel();
        }*/
    }

    private void FixedUpdate()
    {
        
    }

    public void UpdateCameraLevel()
    {
        Vector2 cameraPoint = fullMapCamera.transform.position;
        float vertExtent = fullMapCamera.orthographicSize;
        float horzExtent = vertExtent * Screen.width / Screen.height;

        cameraPoint.x = Mathf.Clamp(cameraPoint.x, -Bounds.size.x / 2 + horzExtent + Bounds.offset.x, Bounds.size.x / 2 - horzExtent + Bounds.offset.x);
        cameraPoint.y = Mathf.Clamp(cameraPoint.y, -Bounds.size.y / 2 + vertExtent + Bounds.offset.y, Bounds.size.y / 2 - vertExtent + Bounds.offset.y);

        //float targetzoom = Mathf.Lerp(fullMapCamera.orthographicSize, zoom_Vector, Time.unscaledDeltaTime * 5);

        //Vector2 targetpos = Vector2.Lerp(transform.position, cameraPoint, Time.unscaledDeltaTime * 5);
        fullMapCamera.transform.position = new Vector3(cameraPoint.x, cameraPoint.y/*targetpos.x, targetpos.y*/, fullMapCamera.transform.position.z);

       
    }
}
