using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tresor : Obj
{
    public MaterialObj materialObj;
    public float poid = 0;
    public int malusShinyRock = 5;
    public float malusCursedObject = 5;
    // 0 if no Malus
    public float NoiseMalus;
    public ParticleSystem particle;
    public Material[] material;

    private void Awake()
    {
        ParticleSystem.MainModule newParticle = particle.main;
        ParticleSystemRenderer renderer = particle.GetComponent<ParticleSystemRenderer>();
        switch (materialObj)
        {
            case MaterialObj.CursedObject:
                renderer.material = material[0];
                newParticle.startColor = new Color(200, 0, 255);
                break;
            case MaterialObj.GlassOFCrystal:
                renderer.material = material[1];
                newParticle.startColor = new Color(0, 255, 20);
                break;
            case MaterialObj.HeavyStone:
                renderer.material = material[2];
                newParticle.startColor = new Color(255, 0, 0);
                break;
            case MaterialObj.ShinoRock:
                renderer.material = material[3];
                newParticle.startColor = new Color(255, 100, 240);
                break;
            case MaterialObj.Soap:
                renderer.material = material[4];
                newParticle.startColor = new Color(110, 255, 255);
                break;
            case MaterialObj.Noisy:
                renderer.material = material[5];
                newParticle.startColor = new Color(250, 255, 0);
                break;
        }
    }

    public override void ActiveEvent()
    {
        base.ActiveEvent();
        if (playerControl.Get_inventory().Count < 3)
        {
            playerControl.Set_interactableObject(null);
            ObjectRefs.Instance.playerInventory.transform.GetChild(2).GetChild(playerControl.Get_inventory().Count).GetComponent<Image>().sprite = GetComponent<SpriteRenderer>().sprite;
            playerControl.Get_inventory().Add(this);
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
                case MaterialObj.Noisy:
                    Noisy();
                    break;
            }
        }
    }

    void Noisy()
    {
        if (!playerControl.NoisyMalus)
        {
            ObjectRefs.Instance.playerNoise.noiseRadius += NoiseMalus;
            playerControl.NoisyMalus = true;
        }
    }

    void Soap()
    {
        if (playerControl != null)
            playerControl.Inertie = true;
    }
    void HeavyStone()
    {
        if(playerControl!= null)
            playerControl.CurrentSpeedMod = 0.7f;
    }
    void ShinoRock()
    {
        if (!playerControl.ShinyRockMalus)
        {
            foreach (GuardIAController_v2 g in ObjectRefs.Instance.GAIC)
            {
                FOV_vBT f = g.gameObject.GetComponent<FOV_vBT>();
                if (f != null)
                    f.viewRadius += malusShinyRock;
            }
            playerControl.ShinyRockMalus = true;
        }
    }
    void CursedObject()
    {
        if (playerControl != null)
        {
            if (!playerControl.CursedObjectMalusActivated)
            {
                playerControl.CursedObjectMalusActivated = true;
                playerControl.CursedObjectMalus = malusCursedObject;
            }
        }
    }
    void GlassOFCrystal()
    {
        if (playerControl != null)
        {
            if (!playerControl.GlassOfCrystalMalysActivated)
            {
                playerControl.GlassOfCrystalMalysActivated = true;
                switch (playerControl.stat.power)
                {
                    case Power.Hunter:
                        playerControl.GlassOfCrystalMalus = 3;
                        break;
                    case Power.Cheater:
                        playerControl.GlassOfCrystalMalus = 3;
                        break;
                    case Power.Ninja:
                        playerControl.GlassOfCrystalMalus = 5;
                        break;
                }
            }
        }
    }
}
