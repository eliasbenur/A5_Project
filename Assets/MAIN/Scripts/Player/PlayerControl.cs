using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;
using UnityEngine.AI;

[RequireComponent(typeof(Collider2D), typeof(Rigidbody2D))]
public class PlayerControl : MonoBehaviour
{
    Player player;
    [Range(1, 4)]
    public int intPlayer = 1;
    Rigidbody2D rb;
    public Stat stat;
    //public float speedMod = 1;
    public bool canMove;
    public bool canInteract;
    bool canDash = true;
    public AnimationCurve dashCurve;
    public Obj interactableObject;
    public Vector2 moveVector;

    public List<Tresor> inventory = new List<Tresor>();

    NavMeshAgent agent;


    private void Start()
    {
        player = ReInput.players.GetPlayer(0);
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;
        if (stat == null)
        {
            stat = new Stat(0, 0, Power.None);
        }
        if (GetComponent<NavMeshAgent>() != null)
        {
            agent = GetComponent<NavMeshAgent>();
            agent.updateRotation = false;
            agent.updateUpAxis = false;
        }

    }

    private void Update()
    {
        if (canMove)
        {
            moveVector.x = player.GetAxis("Horizontal");
            moveVector.y = player.GetAxis("Vertical");
        }
        else
        {
            moveVector.Normalize();
        }

        if(player.GetButtonDown("Dash")&& canDash)
        {
            StartCoroutine(Dash(moveVector));
        }

        if (player.GetButtonDown("Interact") && canInteract)
        {
            Action(interactableObject);
        }
    }

    private void FixedUpdate()
    {
        if (canDash)
        {
            if (moveVector != Vector2.zero)
            {
                //agent.velocity = new Vector2(moveVector.x * stat.speed, moveVector.y * stat.speed);
                //rb.velocity = new Vector2(moveVector.x * stat.speed, moveVector.y * stat.speed);
                transform.position += (Vector3)moveVector * stat.speed * Time.deltaTime;
            }
            else
            {
                //agent.velocity -= agent.velocity * 0.25f;
                //rb.velocity -= rb.velocity * 0.25f;
            }
        }
    }

    protected void Action(Obj newObject)
    {
        if (newObject != null) newObject.ActiveEvent();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "EndZone" && GameManager.Instance.done)
        {
            GameManager.Instance.Restart();
        }
        
        Obj newObject = collision.gameObject.GetComponent<Obj>();
        Debug.Log(collision.name);
        if (newObject != null)
        {
            Debug.Log("triggerOn");
            interactableObject = newObject;
            newObject.playerControl = this;
            canInteract = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        
        Obj newObject = collision.GetComponent<Obj>();
        if (newObject!= null&&newObject == interactableObject)
        {
            Debug.Log("triggerOff");
            interactableObject = null;
            newObject.playerControl = null;
            canInteract = false;
        }
    }

    IEnumerator Dash(Vector2 vDash)
    {
        canDash = false;
        float curveTime = 0f;
        float curveAmount = dashCurve.Evaluate(curveTime/stat.timeDash);
        while(curveTime < stat.timeDash)
        {
            curveTime += Time.deltaTime;
            curveAmount = dashCurve.Evaluate(curveTime / stat.timeDash);
            transform.position += (Vector3)vDash * stat.dashSpeed* curveAmount * Time.deltaTime;
            yield return null;
        }
        canDash = true;
    }
}
