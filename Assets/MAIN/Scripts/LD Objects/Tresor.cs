using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tresor : Obj
{
    public MaterialObj materialObj;
    public float poid = 0;
    public EnumObjPlayer canTake= EnumObjPlayer.All;
    public int rangeRock = 5;
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
            switch (materialObj)
            {
                case MaterialObj.Soap:
                    Soap();
                    break;
                case MaterialObj.HeavyStone:
                    HeavyStone();
                    break;
                case MaterialObj.ShinoRock:
                    ShinoRock();
                    break;
                case MaterialObj.CursedObject:
                    CursedObject();
                    break;
                case MaterialObj.GlassOFCrystal:
                    GlassOFCrystal();
                    break;
            }
        }
    }
    void Soap()
    {
        if (playerControl != null)
            playerControl.inertie = true;
    }
    void HeavyStone()
    {
        if(playerControl!= null)
            playerControl.currentSpeedMod = 0.7f;
    }
    void ShinoRock()
    {
        foreach (GuardIAController_v2 g in ObjectRefs.Instance.GAIC)
        {
            FOV_vBT f = g.gameObject.GetComponent<FOV_vBT>();
            if (f != null)
                f.viewRadius += rangeRock;       
        }
    }
    void CursedObject()
    {
        if (playerControl != null)
            playerControl.malusCursedObject = 0.7f;
    }
    void GlassOFCrystal()
    {
        if (playerControl != null)
            playerControl.multiplicaterGlassOfCrystal = 1.5f;
    }
}
