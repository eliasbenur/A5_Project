using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider2D))]
public class Obj : MonoBehaviour
{
    [HideInInspector] public PlayerControl playerControl;
    //public string StringAction;
    [HideInInspector]public SpriteRenderer ToHightlight;
    public Material Highlight;
    public Material Default;
    public Canvas Interaction_Canvas;
    

    protected virtual void Start()
    {
        if (Interaction_Canvas == null) Interaction_Canvas = new Canvas();
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
