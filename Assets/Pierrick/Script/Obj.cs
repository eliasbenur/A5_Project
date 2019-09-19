using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider2D))]
public class Obj : MonoBehaviour
{
    [HideInInspector] public Voleur voleur;
    public virtual void ActiveEvent()
    {
        Debug.Log("Event");
    }
}
