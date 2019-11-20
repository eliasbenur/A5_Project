using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;
using UnityEngine.AI;
using UnityEngine.UI;

[RequireComponent(typeof(Collider2D), typeof(Rigidbody2D))]
public class PlayerControl : MonoBehaviour
{
    Player player;
    [Range(1, 4)]
    public int intPlayer = 1;
    Rigidbody2D rb;
    public Stat stat;
    //public float speedMod = 1;
    public float sprintMod;
    public float noiseMod;
    public bool canMove;
    public bool canInteract;
    public bool canDash = true;
    public bool activated = true;
    public GameObject fullMap;
    public LevelMenu_Manager menuManager;
    public AnimationCurve dashCurve;
    public Obj interactableObject;
    public Vector2 moveVector;
    public Text text_PowerNb;
    [HideInInspector]
    public int nbAntiCam = 0;
    public float DistanceMinWallApresDash = 0.5f;
    float distanceDash = 1;
    public LayerMask layerDash;
    public Sprite cameraManSprite, KeyManSprite;
    public GameObject traceDePas;//Pierro
    public List<GameObject> traceDePasActiv;

    public float sprint;

    public List<Tresor> inventory = new List<Tresor>();

    //Power Vars
    Slider powerSlider;
    bool powerActive = false;
    bool powerunavailable = false;
    float powerDelay_tmp;

    //PowersUps
    public bool securityZone1 = false;
    public bool securityZone2 = false;
    public GameObject donutPrefab;

    NavMeshAgent agent;

    public bool isPowerActive()
    {
        if (powerActive)
        {
            return true;
        }
        return false;
    }


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

        SetpowerNb(stat.nbKey);

        switch (stat.power)
        {
            case Power.AllKey:
                transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = KeyManSprite;
                break;
            case Power.CameraOff:
                transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = cameraManSprite;
                text_PowerNb.transform.parent.gameObject.SetActive(false);
                break;
        }

        powerSlider = ObjectRefs.Instance.powerSlider;
        powerDelay_tmp = stat.powerDelay;

    }

    public void SetpowerNb(int value_)
    {
        stat.nbKey_tmp = value_;
        text_PowerNb.text = "x " + stat.nbKey_tmp;
    }

    public int GetpowerNb()
    {
        return stat.nbKey_tmp;
    }

    private void Update()
    {
        if (activated)
        {
            menuManager.canShowObjective = true;

            if (player.GetButtonDown("Sprint"))
            {
                ObjectRefs.Instance.playerNoise.noiseRadius = noiseMod;
                ParticleSystem.MainModule newMain = transform.GetChild(1).GetComponent<PlayerNoise>().particleSystemPrefab.GetComponent<ParticleSystem>().main;
                newMain.duration = noiseMod;

            }
            if (player.GetButtonUp("Sprint"))
            {
                ObjectRefs.Instance.playerNoise.noiseRadius = 1.5f;
                ParticleSystem.MainModule newMain = transform.GetChild(1).GetComponent<PlayerNoise>().particleSystemPrefab.GetComponent<ParticleSystem>().main;
                newMain.startLifetime = 1;
            }

            if (canMove)
            {
                moveVector.x = player.GetAxis("Horizontal");
                moveVector.y = player.GetAxis("Vertical");
                if (moveVector.sqrMagnitude > 1)
                {
                    moveVector.Normalize();
                }
                //Flip Sprite
                if (moveVector.x > 0)
                {
                    transform.GetChild(0).GetComponent<SpriteRenderer>().flipX = true;
                }
                else if(moveVector.x < 0)
                {
                    transform.GetChild(0).GetComponent<SpriteRenderer>().flipX = false;
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
                    Action(interactableObject);
                }
                if (player.GetButtonDown("CapacitySpe"))
                {
                    Capacity();
                }
            }
            if (player.GetButtonDown("ShowMap") && fullMap != null)
            {
                if (fullMap.activeSelf == true)
                {
                    ActiveMap(false);
                }
                else
                {
                    ActiveMap(true);
                }
            }
            if (player.GetButtonDown("Back") && fullMap != null && fullMap.activeSelf == true)
            {
                ActiveMap(false);
            }
        }
        else
        {
            menuManager.canShowObjective = false;
        }

        PowerUpdate();
    }

    public void ActiveMap(bool status)
    {
        fullMap.SetActive(status);
        fullMap.GetComponent<MapControl>().Start();
        if (status)
            Time.timeScale = 0f;
        else
            Time.timeScale = 1f;
    }

    public void PowerUpdate()
    {
        if (powerActive)
        {
            if (powerDelay_tmp > 0)
            {
                powerDelay_tmp -= Time.deltaTime;
            }
            else
            {
                powerActive = false;
                powerunavailable = true;
                DisablePower();
            }

        }
        else
        {
            if (powerDelay_tmp < stat.powerDelay)
            {
                powerDelay_tmp += Time.deltaTime;
            }
            else
            {
                powerunavailable = false;
            }
        }

        powerSlider.value = powerDelay_tmp / stat.powerDelay;
    }

    public void ActivePower()
    {
        switch (stat.power)
        {
            case Power.Cheater:
                if (inventory.Count > 0)
                {
                    ObjectRefs.Instance.playerNoise.noiseRadius -= inventory[0].NoiseMalus;
                }
                break;
            case Power.Ninja:
                ObjectRefs.Instance.playerNoise.noiseRadius -= ObjectRefs.Instance.playerNoise.getBaseNoiseRadius();
                break;
            case Power.Hunter:
                NinjaAct();
                break;
        }
    }

    public void DisablePower()
    {
        switch (stat.power)
        {
            case Power.Cheater:
                if (inventory.Count > 0)
                {
                    ObjectRefs.Instance.playerNoise.noiseRadius += inventory[0].NoiseMalus;
                }
                break;
            case Power.Ninja:
                ObjectRefs.Instance.playerNoise.noiseRadius += ObjectRefs.Instance.playerNoise.getBaseNoiseRadius();
                break;
            case Power.Hunter:
                CleanTraceDePasActiv();
                break;
        }
    }

    private void FixedUpdate()
    {
        if (canDash)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, moveVector, 0.5f, layerDash);

            if (hit.collider != null && hit.distance < 0.5f)
            {
                if (moveVector != Vector2.zero)
                {
                    transform.position += (Vector3)moveVector * (stat.speed * Time.fixedDeltaTime);
                }
            }
            else
            {
                if (moveVector != Vector2.zero)
                {
                    //agent.velocity = new Vector2(moveVector.x * stat.speed, moveVector.y * stat.speed);
                    //rb.velocity = new Vector2(moveVector.x * stat.speed, moveVector.y * stat.speed);
                    if (player.GetButton("Sprint"))
                    {
                        transform.position += (Vector3)moveVector * (stat.speed * Time.fixedDeltaTime * sprintMod);
                    }
                    else
                    {
                        transform.position += (Vector3)moveVector * (stat.speed * Time.fixedDeltaTime);
                    }
                }
                else
                {
                    //agent.velocity -= agent.velocity * 0.25f;
                    //rb.velocity -= rb.velocity * 0.25f;
                }
            }
        }
    }

    protected void Action(Obj newObject)
    {
        if (newObject != null) newObject.ActiveEvent();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "EndZone" && GameManager.Instance.objectiveDone)
        {
            GameManager.Instance.RestartGame();
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
                    newObject.Interaction_Canvas?.gameObject.SetActive(true);
                }
                catch { }
            }
            else if (newObject is Door)
            {
                newObject.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().material = newObject.Highlight;
                newObject.Interaction_Canvas.gameObject.SetActive(true);
            }
            else if (newObject is Doorv2)
            {
                newObject.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().material = newObject.Highlight;
                newObject.transform.GetChild(1).gameObject.GetComponent<SpriteRenderer>().material = newObject.Highlight;
                newObject.Interaction_Canvas.gameObject.SetActive(true);
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
                    newObject.Interaction_Canvas.gameObject.SetActive(false);
                }
                catch { }
            }
            else if (newObject is Door)
            {
                newObject.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().material = newObject.Default;
                newObject.Interaction_Canvas.gameObject.SetActive(false);
            }
            else if (newObject is Doorv2)
            {
                newObject.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().material = newObject.Default;
                newObject.transform.GetChild(1).gameObject.GetComponent<SpriteRenderer>().material = newObject.Default;
                newObject.Interaction_Canvas.gameObject.SetActive(false);
            }

            canInteract = false;
        }
    }

    public void StopVibrations()
    {
        player.SetVibration(0, 0);
        player.SetVibration(1, 0);
        player.SetVibration(2, 0);
        player.SetVibration(3, 0);
    }

    protected virtual void Capacity()
    {
        switch (stat.power)
        {
            case Power.CameraOff:
                if (nbAntiCam > 0)
                {
                    nbAntiCam--;
                    Instantiate(stat.ObjAntiCamera, this.transform.position, Quaternion.identity);
                }
                break;
            case Power.Cook:
                if (powerDelay_tmp >= stat.powerDelay)
                {
                    powerDelay_tmp = 0;
                    //Power
                    Instantiate(donutPrefab, this.transform.position, Quaternion.identity);
                }
                break;
            default:
                if (!powerActive && !powerunavailable)
                {
                    powerActive = true;
                    ActivePower();
                }
                else
                {
                    powerActive = false;
                    DisablePower();
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

    void NinjaAct()//Pierro
    {
        foreach (GuardIAController g in ObjectRefs.Instance.GAIC)
        {
            for (int i = 0; i < g.patrollPoints_List.Count; i++)
            {
                if (i != g.patrollPoints_List.Count - 1)
                {
                    Vector3 v = g.patrollPoints_List[i].transform.position - g.patrollPoints_List[i + 1].transform.position;
                    float dist = v.magnitude;
                    v.Normalize();
                    for (int j = 0; j < dist; j += 2)
                    {
                        GameObject go = Instantiate(traceDePas, g.patrollPoints_List[i + 1].transform.position + v * j, Quaternion.identity);
                        traceDePasActiv.Add(go);
                    }
                }
                else
                {
                    Vector3 v = g.patrollPoints_List[i].transform.position - g.patrollPoints_List[0].transform.position;
                    float dist = v.magnitude;
                    v.Normalize();
                    for (int j = 0; j < dist; j += 2)
                    {
                        GameObject go = Instantiate(traceDePas, g.patrollPoints_List[0].transform.position + v * j, Quaternion.identity);
                        traceDePasActiv.Add(go);
                    }
                }
            }
        }

    }
    void CleanTraceDePasActiv()
    {
        for (int i = 0; i < traceDePasActiv.Count; i++)
        {
            Destroy(traceDePasActiv[i]);
        }
        traceDePasActiv.Clear();
    }
}
