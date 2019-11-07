using MyBox;
using Rewired;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Doorv2 : Obj
{
    [System.NonSerialized]
    public bool open = false;
    Animator anim;
    public float decalage = 0.7f;
    Vector3 posiPorteLeft, posiPorteRight;

    [Separator("Bool")]
    public bool closeKey = false;
    Player player;
    [ConditionalField(nameof(closeKey))] public float timer_miniGame;
    string[] sequence;
    [ConditionalField(nameof(closeKey))] public GameObject canvas_lockedkey;
    [ConditionalField(nameof(closeKey))] public Text timerText;

    [ConditionalField(nameof(closeKey))] public bool canSelect = false;
    [ConditionalField(nameof(closeKey))] public bool done = false;
    [ConditionalField(nameof(closeKey))] public bool success = false;

    public enum inputs
    {
        A,
        B,
        X,
        Y
    }

    protected override void Start()
    {
        base.Start();
        anim = GetComponent<Animator>();

        player = ReInput.players.GetPlayer(0);

        posiPorteLeft = transform.GetChild(0).transform.localPosition;
        posiPorteRight = transform.GetChild(1).transform.localPosition;
    }

    public void FillSequence()
    {
        sequence = new string[4];
        int random = Random.Range(0, 4);
        for (int i = 0; i < 4; i++)
        {
            sequence[i] = ((inputs)random).ToString();
            ObjectRefs.Instance.inputContainer[i].sprite = ObjectRefs.Instance.inputSet[random];
            ObjectRefs.Instance.inputContainer[i].color = Color.white;
            random = Random.Range(0, 4);
        }
    }

    public void Pick(PlayerControl playerControl)
    {
        StartCoroutine(PickLock(playerControl));
    }

    public IEnumerator PickLock(PlayerControl playerControl)
    {
        done = false;
        canSelect = false;

        playerControl.activated = false;
        FillSequence();
        timerText.text = timer_miniGame.ToString();
        canvas_lockedkey.transform.GetChild(2).gameObject.SetActive(true);
        Debug.Log(canvas_lockedkey.transform.GetChild(2));

        float temp = timer_miniGame;
        int count = 0;
        while (temp >= 0)
        {
            if ((player.GetButtonUp(RewiredConsts.Action.A) || player.GetButtonUp("Interact")) && canSelect == false)
                canSelect = true;

            if (count == 4)
            {
                success = true;
                break;
            }
            if (canSelect)
            {
                if (player.GetAnyButtonDown())
                {
                    if ((player.GetButtonDown(RewiredConsts.Action.A) && sequence[count] == "A") ||
                    (player.GetButtonDown(RewiredConsts.Action.B) && sequence[count] == "B") ||
                    (player.GetButtonDown(RewiredConsts.Action.X) && sequence[count] == "X") ||
                    (player.GetButtonDown(RewiredConsts.Action.Y) && sequence[count] == "Y") || player.GetButtonDown("KeyBoard_LockedDoor"))
                    {
                        ObjectRefs.Instance.inputContainer[count].color = Color.green;
                        count++;
                    }
                    else
                    {
                        playerControl.SetpowerNb(playerControl.GetpowerNb() - 1);
                        ObjectRefs.Instance.inputContainer[count].color = Color.red;
                        break;
                    }
                }
            }
            temp = temp - 0.01f;
            timerText.text = ((float)((int)(temp * 10)) / 10).ToString();
            yield return new WaitForSecondsRealtime(0.0001f);
        }
        if (success == true)
        {
            playerControl.SetpowerNb(playerControl.GetpowerNb() - 1);
            gameObject.GetComponent<Doorv2>().OpenDoor();
            Destroy(gameObject.GetComponent<LockedDoor>());
            gameObject.GetComponent<Doorv2>().closeKey = false;
        }
        done = true;
        canvas_lockedkey.transform.GetChild(2).gameObject.SetActive(false);
        playerControl.activated = true;
        yield return null;
    }

    public override void ActiveEvent()
    {
        base.ActiveEvent();

        if (closeKey)
        {
            if(playerControl.stat.power == Power.AllKey)
            {
                if (ObjectRefs.Instance.player.GetComponent<PlayerControl>().stat.nbKey_tmp > 0) {
                    ObjectRefs.Instance.player.GetComponent<PlayerControl>().activated = true;
                    Pick(ObjectRefs.Instance.player.GetComponent<PlayerControl>());
                } 
            }
        }
        else
        {
            OpenDoor();
        }
 
    }

    IEnumerator DelayBake()
    {
        yield return new WaitForSeconds(0.2f);
        //ObjectRefs.Instance.NavMesh.GetComponent<NavMeshSurface2d>().BuildNavMesh();
        //ObjectRefs.Instance.NavMesh.GetComponent<NavMeshSurface2d>().UpdateNavMesh(ObjectRefs.Instance.NavMesh.GetComponent<NavMeshSurface2d>().navMeshData);
    }

    public void OpenDoor()
    {
        open = !open;
        if (open) Open();
        else Close();
    }

    void Open()
    {
        //anim.SetBool("open", true);
        transform.GetChild(0).transform.localPosition = posiPorteLeft + Vector3.right * decalage;
        transform.GetChild(0).transform.gameObject.GetComponent<Collider2D>().enabled = false;
        //transform.GetChild(0).transform.gameObject.SetActive(false);
        transform.GetChild(0).transform.gameObject.GetComponent<SpriteRenderer>().color = new Vector4(0,1,1,0.3f);
        transform.GetChild(1).transform.localPosition = posiPorteRight - Vector3.right * decalage;
        transform.GetChild(1).transform.gameObject.GetComponent<Collider2D>().enabled = false;
        //transform.GetChild(1).transform.gameObject.SetActive(false);
        transform.GetChild(1).transform.gameObject.GetComponent<SpriteRenderer>().color = new Vector4(0, 1, 1, 0.3f);
        Destroy(transform.GetChild(0).GetComponent<NavMeshModifier>());
        Destroy(transform.GetChild(1).GetComponent<NavMeshModifier>());
        StartCoroutine(DelayBake());
    }
    void Close()
    {
        //anim.SetBool("open", false);
        transform.GetChild(0).transform.localPosition = posiPorteLeft;
        transform.GetChild(0).transform.gameObject.GetComponent<Collider2D>().enabled = true;
        transform.GetChild(0).transform.gameObject.SetActive(true);
        transform.GetChild(0).transform.gameObject.GetComponent<SpriteRenderer>().color = new Vector4(0, 1, 1, 1);
        transform.GetChild(1).transform.localPosition = posiPorteRight;
        transform.GetChild(1).transform.gameObject.GetComponent<Collider2D>().enabled = true;
        transform.GetChild(1).transform.gameObject.SetActive(true);
        transform.GetChild(1).transform.gameObject.GetComponent<SpriteRenderer>().color = new Vector4(0, 1, 1, 1);
        NavMeshModifier nmm_tmp =  transform.GetChild(0).gameObject.AddComponent<NavMeshModifier>();
        nmm_tmp.overrideArea = true;
        nmm_tmp.area = 1;
        nmm_tmp = transform.GetChild(1).gameObject.AddComponent<NavMeshModifier>();
        nmm_tmp.overrideArea = true;
        nmm_tmp.area = 1;
        StartCoroutine(DelayBake());
    }
}
