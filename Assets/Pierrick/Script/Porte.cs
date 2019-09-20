using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[RequireComponent(typeof(Animator))]
public class Porte :Obj
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
        //anim.SetBool("open", true);
        transform.localEulerAngles = new Vector3(0, 0, transform.localEulerAngles.z + 90);
    }
    void Close()
    {
        Debug.Log("close");
        //anim.SetBool("open", false);
        transform.localEulerAngles = new Vector3(0, 0, transform.localEulerAngles.z - 90);
    }
}
