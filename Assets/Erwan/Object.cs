using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object : MonoBehaviour
{
    public PlayerControl playerControl;

    public virtual void ActiveEvent()
    {
        Debug.Log("Event");
    }
}
