using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : Object
{
    public bool open = false;
    Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }
    public override void ActiveEvent()
    {
        base.ActiveEvent();
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
