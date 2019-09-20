using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider2D))]
public class Obj : MonoBehaviour
{
    [HideInInspector] public Voleur voleur;
    //public Sprite SpriteImgCanvas;
    //public string StringAction;
    public Material Highlight;
    public Material Default;
    public Canvas canvas;

    private void Start()
    {
        if (canvas == null) canvas = new Canvas();
    }

    public virtual void ActiveEvent()
    {
        Debug.Log("Event");
    }
}
