﻿using System.Collections;
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


    void Start()
    {
        rotationInitial = transform.eulerAngles.z;
    }

    // Update is called once per frame
    void Update()
    {
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
        //Debug.Log("att " + (angle + 360));
        //Debug.Log(angleRotation / 2);
    }

#if UNITY_EDITOR


#endif
}