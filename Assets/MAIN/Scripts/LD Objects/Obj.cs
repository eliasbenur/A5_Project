using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider2D))]
public class Obj : MonoBehaviour
{
    [HideInInspector] public PlayerControl playerControl;
    public Sprite SpriteImgCanvas;
    //public string StringAction;
    [HideInInspector]public SpriteRenderer ToHightlight;
    public Material Highlight;
    public Material Default;
    public Canvas canvas;
    

    protected virtual void Start()
    {
        if (SpriteImgCanvas == null&& GetComponent<SpriteRenderer>() != null)
            SpriteImgCanvas = GetComponent<SpriteRenderer>().sprite;
        if (canvas == null) canvas = new Canvas();
        InitialisationToHighlight();
    }

    public virtual void ActiveEvent()
    {
        Debug.Log("Event");
    }

    protected virtual void InitialisationToHighlight()
    {
        ToHightlight = GetComponent<SpriteRenderer>();
    }
}
