using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;
using UnityEngine.AI;
using UnityEngine.UI;
using MyBox;

[RequireComponent(typeof(Collider2D), typeof(Rigidbody2D))]
public class PlayerControl : MonoBehaviour
{
    //Rewired
    Player player;

    [Separator("Player")]
    public Stat stat;
    List<Tresor> inventory = new List<Tresor>();
    //States
    bool canMove = true;
    bool canInteract;
    Obj interactableObject;
    bool canDash = true;
    bool playerEnabled = true;
    //Movement
    Vector2 moveVector;

    [Separator("Movement")]
    public float powerUp_SpeedMod;
    private float currentSpeedMod = 1;

    [Separator("Dash")]
    public AnimationCurve dashCurve;
    public float distanceMinWallApresDash = 0.5f;
    float distanceDash = 1;
    public LayerMask layerDash;

    [Separator("Hunter")]
    public GameObject traceDePas;//Pierro
    public List<GameObject> traceDePasActiv;


    // Objects Malus
    private bool noisyMalus;
    private bool shinyRockMalus;
    private float cursedObjectMalus;
    private bool cursedObjectMalusActivated;
    private float glassOfCrystalMalus;
    private bool glassOfCrystalMalysActivated;

    //Power Vars
    Slider powerSlider;
    bool powerActive = false;
    bool powerunavailable = false;
    float powerDelay_tmp;

    [Separator("Inertie")]
    public float valeurInertie = 0.01f;
    public float inertieDim = 0.999f;
    private bool inertie = false;
    Vector3 vectorInertie;


    [Separator("Power Ups")]
    public bool securityZone1 = false;
    public bool securityZone2 = false;

    [Separator("Cook")]
    public GameObject donutPrefab;




    public bool NoisyMalus { get => noisyMalus; set => noisyMalus = value; }
    public bool ShinyRockMalus { get => shinyRockMalus; set => shinyRockMalus = value; }
    public float CursedObjectMalus { get => cursedObjectMalus; set => cursedObjectMalus = value; }
    public bool CursedObjectMalusActivated { get => cursedObjectMalusActivated; set => cursedObjectMalusActivated = value; }
    public float GlassOfCrystalMalus { get => glassOfCrystalMalus; set => glassOfCrystalMalus = value; }
    public bool GlassOfCrystalMalysActivated { get => glassOfCrystalMalysActivated; set => glassOfCrystalMalysActivated = value; }
    public bool Inertie { get => inertie; set => inertie = value; }
    public float CurrentSpeedMod { get => currentSpeedMod; set => currentSpeedMod = value; }

    public List<Tresor> Get_inventory() { return inventory; }

    public bool Get_playerEnabled() { return playerEnabled; }
    public void Set_playerEnabled(bool val_) { playerEnabled = val_; }

    public void Set_interactableObject(Obj val_) { interactableObject = val_; }

    public Vector2 Get_moveVector() { return moveVector; }

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
        if (stat == null)
        {
            stat = new Stat(0, 0, Power.None);
        }

        powerSlider = ObjectRefs.Instance.powerSlider;
        powerDelay_tmp = stat.powerDelay;

        switch (stat.power)
        {
            case Power.Hunter:
                stat.powerDelay = 5;
                break;
            case Power.Cheater:
                stat.powerDelay = 5;
                break;
            case Power.Ninja:
                stat.powerDelay = 10;
                break;
            case Power.Cook:
                stat.powerDelay = 5;
                break;
            case Power.DejaVu:
                stat.powerDelay = 5;
                break;
        }

    }

    /*Player Sprite Flip */
    public void SpriteFlip()
    {
        if (moveVector.x > 0)
        {
            transform.GetChild(0).GetComponent<SpriteRenderer>().flipX = true;
        }
        else if (moveVector.x < 0)
        {
            transform.GetChild(0).GetComponent<SpriteRenderer>().flipX = false;
        }
    }

    private void Update()
    {
        if (playerEnabled)
        {
            // Player input
            if (canMove)
            {
                moveVector.x = player.GetAxis("Horizontal");
                moveVector.y = player.GetAxis("Vertical");
                if (moveVector.magnitude < 0.4f)
                    moveVector = Vector2.zero;
                if (moveVector.sqrMagnitude > 1)
                {
                    moveVector.Normalize();
                }

            }
            else
            {
                moveVector = Vector2.zero;
            }

            // --- DASH -- 
            //Cursed Object Malus
            float objectMalus = 0;
            if (CursedObjectMalusActivated)
            {
                objectMalus = CursedObjectMalus;
            }
            //Dash
            if (GlassOfCrystalMalysActivated)
            {
                if (player.GetButtonDown("Dash") && canDash && stat.power == Power.DejaVu && powerDelay_tmp >= (stat.powerDelay + objectMalus))
                {
                    powerDelay_tmp -= stat.powerDelay + objectMalus;

                    canDash = false;
                    StartCoroutine(Dash(moveVector));
                }

            }
            else
            {
                if (player.GetButtonDown("Dash") && canDash && stat.power == Power.DejaVu && powerDelay_tmp >= (stat.powerDelay + objectMalus) / 2)
                {
                    powerDelay_tmp -= (stat.powerDelay + objectMalus) / 2;

                    canDash = false;
                    StartCoroutine(Dash(moveVector));
                }
            }

            if (canDash) // Is not dashing already
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
            // ---- FULL MAP ---
            if (player.GetButtonDown("ShowMap") && ObjectRefs.Instance.fullMap != null)
            {
                if (ObjectRefs.Instance.fullMap.activeSelf == true)
                {
                    ActiveMap(false);
                }
                else
                {
                    ActiveMap(true);
                }
            }
            if (player.GetButtonDown("Back") && ObjectRefs.Instance.fullMap != null && ObjectRefs.Instance.fullMap.activeSelf == true)
            {
                ActiveMap(false);
            }

            PowerUpdate();
            SpriteFlip();
        }


    }

    public void ActiveMap(bool status)
    {
        ObjectRefs.Instance.fullMap.SetActive(status);
        ObjectRefs.Instance.fullMap.GetComponent<MapControl>().Start();
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
            if (powerDelay_tmp < stat.powerDelay + CursedObjectMalus)
            {
                powerDelay_tmp += Time.deltaTime;
            }
            else
            {
                powerunavailable = false;
            }
        }
        if (powerActive)
        {
            if (GlassOfCrystalMalysActivated)
            {
                powerSlider.value = powerDelay_tmp / (stat.powerDelay - GlassOfCrystalMalus);
            }
            else
            {
                powerSlider.value = powerDelay_tmp / stat.powerDelay;
            }

        }
        else
        {
            powerSlider.value = powerDelay_tmp / (stat.powerDelay + CursedObjectMalus);
        }

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
                CurrentSpeedMod -= powerUp_SpeedMod;
                break;
            case Power.Ninja:
                ObjectRefs.Instance.playerNoise.noiseRadius -= ObjectRefs.Instance.playerNoise.getBaseNoiseRadius();
                CurrentSpeedMod -= powerUp_SpeedMod;
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
                CurrentSpeedMod += powerUp_SpeedMod;
                break;
            case Power.Ninja:
                ObjectRefs.Instance.playerNoise.noiseRadius += ObjectRefs.Instance.playerNoise.getBaseNoiseRadius();
                CurrentSpeedMod += powerUp_SpeedMod;
                break;
            case Power.Hunter:
                CleanTraceDePasActiv();
                break;
        }
    }

    private void FixedUpdate()
    {
        if (canDash) // Is not already dashing
        {
            //Movement Raycast 
            RaycastHit2D hit = Physics2D.Raycast(transform.position, moveVector, 0.5f, layerDash);

            if (hit.collider != null && hit.distance < 0.5f)
            {
            
                if (moveVector != Vector2.zero)
                {
                    transform.position += (Vector3)moveVector * (stat.speed * CurrentSpeedMod * Time.fixedDeltaTime);
                }
            }
            else
            {
                if (moveVector != Vector2.zero&&!Inertie)
                {
                    transform.position += (Vector3)moveVector * (stat.speed * CurrentSpeedMod * Time.fixedDeltaTime);

                }
                else
                {
                    if (vectorInertie.magnitude == 0)
                    {
                        vectorInertie = moveVector; 
                    }
                    else
                    {
                        if (moveVector.magnitude == 0) { vectorInertie *= inertieDim; }
                        else
                        {
                            if (moveVector.x != 0)
                                vectorInertie.x = vectorInertie.x < moveVector.x ? vectorInertie.x + valeurInertie : vectorInertie.x - valeurInertie;
                            if (moveVector.y != 0)
                                vectorInertie.y = vectorInertie.y < moveVector.y ? vectorInertie.y + valeurInertie : vectorInertie.y - valeurInertie;
                            
                        }
                        Debug.DrawLine(transform.position, transform.position + vectorInertie, Color.red);
                        transform.position += vectorInertie * (stat.speed * CurrentSpeedMod * Time.fixedDeltaTime);
                    }

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
            case Power.Cook:
                if (powerDelay_tmp >= stat.powerDelay)
                {
                    powerDelay_tmp = 0;
                    //Power
                    Instantiate(donutPrefab, this.transform.position, Quaternion.identity);
                }
                break;
            case Power.DejaVu:
                break;
            default:
                if (!powerActive && !powerunavailable)
                {
                    powerActive = true;
                    float glass_tmp = stat.powerDelay;
                    if (GlassOfCrystalMalysActivated)
                    {
                        glass_tmp -= GlassOfCrystalMalus;
                    }
                    float cursed_tmp = stat.powerDelay;
                    if (CursedObjectMalusActivated)
                    {
                        cursed_tmp += CursedObjectMalus;
                    }
                    
                    powerDelay_tmp = (powerDelay_tmp * glass_tmp) / (cursed_tmp);

                    ActivePower();
                }
                else
                {
                    powerActive = false;
                    float glass_tmp = stat.powerDelay;
                    if (GlassOfCrystalMalysActivated)
                    {
                        glass_tmp -= GlassOfCrystalMalus;
                    }
                    float cursed_tmp = stat.powerDelay;
                    if (CursedObjectMalusActivated)
                    {
                        cursed_tmp += CursedObjectMalus;
                    }

                    powerDelay_tmp = (powerDelay_tmp * glass_tmp) / (cursed_tmp);

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
            while (((Vector3)hit.point-transform.position).magnitude>distanceMinWallApresDash)
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
        distanceDash = v.magnitude; // ???

    }

    void NinjaAct()//Pierro
    {
        foreach (GuardIAController_v2 g in ObjectRefs.Instance.GAIC)
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
