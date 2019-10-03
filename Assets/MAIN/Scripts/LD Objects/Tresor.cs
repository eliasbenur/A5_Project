using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tresor : Obj
{
    public MaterialObj materialObj;
    public float poid = 0;
    public GameObject imageInInventory = null;
    public override void ActiveEvent()
    {
        base.ActiveEvent();
        playerControl.interactableObject = null;
        playerControl.inventory.Add(this);
        imageInInventory = GameManager2.Instance.AddObjInInvetory(SpriteImgCanvas);
        gameObject.SetActive(false);
        transform.SetParent(playerControl.transform);
    }
}
