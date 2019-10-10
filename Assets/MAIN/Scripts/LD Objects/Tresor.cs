﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tresor : Obj
{
    public MaterialObj materialObj;
    public float poid = 0;
    public GameObject imageInInventory = null;
    public EnumObjPlayer canTake= EnumObjPlayer.All;
    // 0 if no Malus
    public float NoiseMalus;
    public override void ActiveEvent()
    {
        base.ActiveEvent();
        if (playerControl.inventory.Count == 0&&
            (canTake==EnumObjPlayer.All||canTake.ToString()== "Player"+playerControl.intPlayer))
        {
            playerControl.interactableObject = null;
            playerControl.inventory.Add(this);
            playerControl.transform.Find("Noise").GetComponent<PlayerNoise>().noiseRadius += NoiseMalus;
            //imageInInventory = GameManager2.Instance.AddObjInInvetory(SpriteImgCanvas);
            GameManager.Instance.CheckObjective(gameObject);
            gameObject.SetActive(false);
            transform.SetParent(playerControl.transform);
        }
    }
}
