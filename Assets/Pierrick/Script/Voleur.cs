using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D), typeof(Rigidbody2D))]
public class Voleur : MonoBehaviour
{
    public float vitesse = 1;
    [HideInInspector]
    public Obj objCanInteract;
    public List<Obj> inventaire = new List<Obj>();


    protected virtual void Start()
    {
        GetComponent<Rigidbody2D>().gravityScale = 0;
    }
    protected virtual void  Update()
    {
        Move();
        if (Input.GetKeyDown("k")) Action(objCanInteract);
    }
    private void Move()
    {
        float valJoystickX = Input.GetAxisRaw("Horizontal");
        float valJoystickY = Input.GetAxisRaw("Vertical");
        if (Mathf.Abs(valJoystickX) > 0.3)
            transform.position += Vector3.right * vitesse * valJoystickX * Time.deltaTime;
        if (Mathf.Abs(valJoystickY) > 0.3)
            transform.position += Vector3.up * vitesse * valJoystickY * Time.deltaTime;
    }
    protected void Action(Obj obj)
    {
        if(obj!=null)obj.ActiveEvent();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Obj o = collision.GetComponent<Obj>();
        if (o != null)
        {
            objCanInteract = o;
            o.voleur = this;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        Obj o = collision.GetComponent<Obj>();
        if (o == objCanInteract)
        {
            objCanInteract = null;
            o.voleur = null;
        }
    }
}
