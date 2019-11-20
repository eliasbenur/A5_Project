using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tresor : Obj
{
    public MaterialObj materialObj;
    public float poid = 0;
    public EnumObjPlayer canTake= EnumObjPlayer.All;
    // 0 if no Malus
    public float NoiseMalus;
    public override void ActiveEvent()
    {
        base.ActiveEvent();
        if (playerControl.inventory.Count < 3 &&
            (canTake==EnumObjPlayer.All||canTake.ToString()== "Player"+playerControl.intPlayer))
        {
            playerControl.interactableObject = null;
            ObjectRefs.Instance.playerInventory.transform.GetChild(2).GetChild(playerControl.inventory.Count).GetComponent<Image>().sprite = GetComponent<SpriteRenderer>().sprite;
            playerControl.inventory.Add(this);
            ObjectRefs.Instance.playerNoise.noiseRadius += NoiseMalus;
            GameManager.Instance.objectiveDone = true;
            gameObject.SetActive(false);
            transform.SetParent(playerControl.transform);

            ObjectRefs.Instance.soungManager.PlayitemTaken();
        }
    }
}
