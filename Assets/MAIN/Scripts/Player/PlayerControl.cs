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
    public bool canDash = true;
    public bool activated = true;
    public AnimationCurve dashCurve;
    public Obj interactableObject;
    public Vector2 moveVector;
    public int powerNb;
    [HideInInspector]
    public int nbAntiCam = 0;
    public float DistanceMinWallApresDash = 0.5f;
    float distanceDash = 1;
    public LayerMask layerDash;

    public List<Tresor> inventory = new List<Tresor>();

    NavMeshAgent agent;


    private void Start()
    {
        StartCoroutine(CalculeDistanceDash());
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
        if(stat.power== Power.CameraOff)
        {
            nbAntiCam = stat.nbAntiCam;
        }

    }

    private void Update()
    {
        if (activated)
        {
            if (canMove)
            {
                moveVector.x = player.GetAxis("Horizontal");
                moveVector.y = player.GetAxis("Vertical");
                if (moveVector.sqrMagnitude > 1)
                {
                    moveVector.Normalize();
                }
            }
            else

            {
                moveVector.Normalize();
            }

            if (player.GetButtonDown("Dash") && canDash)
            {
                canDash = false;
                StartCoroutine(Dash(moveVector));
            }
            if (canDash)
            {
                if (player.GetButtonDown("Interact") && canInteract)
                {
                    LockedDoor lockedDoor = interactableObject.gameObject.GetComponent<LockedDoor>();
                    if (stat.power == Power.AllKey && lockedDoor != null)
                    {
                        if (powerNb > 0)
                        {
                            //powerNb -= 1;
                            activated = false;
                            lockedDoor.Pick(this);
                        }
                    }
                    else
                        Action(interactableObject);
                }
                if (player.GetButtonDown("CapacitySpe"))
                {
                    Capacity();
                }
            }
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
        if (newObject != null)
        {
            interactableObject = newObject;
            newObject.playerControl = this;
            if (newObject.ToHightlight != null)
            {
                newObject.ToHightlight.material = newObject.Highlight;
                try
                {
                    newObject.canvas?.gameObject.SetActive(true);
                }
                catch { }
            }
            else if (newObject is Door)
            {
                newObject.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().material = newObject.Highlight;
                newObject.canvas.gameObject.SetActive(true);
            }

            canInteract = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        
        Obj newObject = collision.GetComponent<Obj>();
        if (newObject!= null&&newObject == interactableObject)
        {
            interactableObject = null;
            newObject.playerControl = null;
            if (newObject.ToHightlight != null)
            {
                newObject.ToHightlight.material = newObject.Default;
                try
                {
                    newObject.canvas.gameObject.SetActive(false);
                }
                catch { }
            }
            else if (newObject is Door)
            {
                newObject.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().material = newObject.Default;
                newObject.canvas.gameObject.SetActive(false);
            }

            canInteract = false;
        }
    }

    protected virtual void Capacity()
    {
        Debug.Log("capacitySpe");
        switch (stat.power)
        {
            case Power.CameraOff:
                if (nbAntiCam > 0)
                {
                    nbAntiCam--;
                    Instantiate(stat.ObjAntiCamera, this.transform.position, Quaternion.identity);
                }
                break;

        }
    }

    IEnumerator Dash(Vector2 vDash)
    {
        float curveTime = 0f;
        float curveAmount = dashCurve.Evaluate(curveTime/stat.timeDash);

        RaycastHit2D hit = Physics2D.Raycast(transform.position, vDash,distanceDash,layerDash);
        
        if (hit.collider != null&&hit.distance<distanceDash)
        {
            //Debug.Log(hit.distance);
            //Debug.Log(hit.collider);
            //Debug.Log(((Vector3)hit.point - transform.position).magnitude);
            while (((Vector3)hit.point-transform.position).magnitude>DistanceMinWallApresDash)
            {
               // Debug.Log(((Vector3)hit.point - transform.position).magnitude);
                curveTime += Time.deltaTime;
                curveAmount = dashCurve.Evaluate(curveTime / stat.timeDash);
                transform.position += (Vector3)vDash * stat.dashSpeed * curveAmount * Time.deltaTime;
                yield return null;
            }
        }
        else
        {
            while (curveTime < stat.timeDash)
            {
                curveTime += Time.deltaTime;
                curveAmount = dashCurve.Evaluate(curveTime / stat.timeDash);
                transform.position += (Vector3)vDash * stat.dashSpeed * curveAmount * Time.deltaTime;
                yield return null;
            }
        }
        canDash = true;
    }
    IEnumerator CalculeDistanceDash()
    {
        float curveTime = 0f;
        float curveAmount = dashCurve.Evaluate(curveTime / stat.timeDash);
        Vector3 v = Vector3.zero;
        
        while (curveTime < stat.timeDash)
        {
            curveTime += Time.deltaTime;
            curveAmount = dashCurve.Evaluate(curveTime / stat.timeDash);
            v += Vector3.up * stat.dashSpeed * curveAmount * Time.deltaTime;
            yield return null;
        }
        distanceDash = v.magnitude;

    }
}
