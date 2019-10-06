﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : Obj
{
    [System.NonSerialized]
    public bool open = false;
    Animator anim;
    public bool closeKey = false;


    protected override void Start()
    {
        base.Start();
        anim = GetComponent<Animator>();
    }
    public override void ActiveEvent()
    {
        base.ActiveEvent();


        if (closeKey)
        {
            if(playerControl.stat.power != Power.AllKey)
            {
                Tresor key=null;
                foreach (Tresor obj in playerControl.inventory)
                {
                    if (obj is Key)
                    {
                        key = obj;
                        break;
                    }
                }
                if (key != null)
                {
                    playerControl.inventory.Remove(key);
                    Destroy(key.gameObject);
                }
                else return;
            }
            closeKey = false;
        }
            OpenDoor();   
    }

    void OpenDoor()
    {
        open = !open;
        if (open) Open();
        else Close();
    }

    void Open()
    {
        Debug.Log("open");
        anim.SetBool("open", true);
    }
    void Close()
    {
        Debug.Log("close");
        anim.SetBool("open", false);
    }
}