using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
[RequireComponent(typeof(PolygonCollider2D))]
public class CameraRotate : MonoBehaviour
{
    public float Speed = 5;
    public float angleRotation = 180;
    int multiplicateur = 1;
    float rotationInitial = 0;
    public LayerMask playerMask;
    public bool hackeable = false;
    public float timeHacked;
    public float timeUnHacked;
    public float timeH_tmp;
    public bool isHacked;
    PlayerControl playerControl;


    void Start()
    {
        rotationInitial = transform.eulerAngles.z;
        playerControl = ObjectRefs.Instance.player.GetComponent<PlayerControl>();
        timeH_tmp = timeHacked;
    }

    // Update is called once per frame
    void Update()
    {
        if (playerControl.securityZone2 && hackeable)
        {
            if (isHacked)
            {
                timeH_tmp -= Time.deltaTime;
                if (timeH_tmp < 0)
                {
                    isHacked = false;
                    timeH_tmp = timeUnHacked;
                    //
                    GetComponent<Collider2D>().enabled = true;
                    if (transform.childCount > 0)
                        transform.GetChild(0).gameObject.SetActive(true);
                }
            }
            else
            {
                timeH_tmp -= Time.deltaTime;
                if (timeH_tmp < 0)
                {
                    isHacked = true;
                    timeH_tmp = timeHacked;
                    //
                    gameObject.GetComponent<Collider2D>().enabled = false;
                    if (transform.childCount > 0)
                        transform.GetChild(0).gameObject.SetActive(false);
                }
            }
        }
        MoveCamera();
    }

    void MoveCamera()
    {
        float angle = transform.eulerAngles.z - rotationInitial;
        if (angle > 180) angle = angle - 360;
        if (angle < -180) angle = angle + 360;
        if (angle > angleRotation / 2) multiplicateur = -1;
        if (angle < -angleRotation / 2) multiplicateur = 1;
        transform.eulerAngles += Vector3.forward * multiplicateur * Speed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (((1 << collision.gameObject.layer) & playerMask) != 0)
        {
            ObjectRefs.Instance.menuCanvas.GetComponent<LevelMenu_Manager>().Active_LosePanel();
        }
    }

#if UNITY_EDITOR


#endif
}
