using Rewired;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Door : Obj
{
    [System.NonSerialized]
    public bool open = false;
    Animator anim;
    public bool closeKey = false;

    Player player;
    public float timer;
    public string[] sequence;
    public GameObject canvas_lockedkey;
    public Text timerText;

    public bool canSelect = false;
    public bool done = false;
    public bool success = false;

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
        timerText.text = timer.ToString();
        canvas_lockedkey.transform.GetChild(2).gameObject.SetActive(true);
        Debug.Log(canvas_lockedkey.transform.GetChild(2));

        float temp = timer;
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
            gameObject.GetComponent<Door>().OpenDoor();
            Destroy(gameObject.GetComponent<LockedDoor>());
            gameObject.GetComponent<Door>().closeKey = false;
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
            //closeKey = false;
        }
        else
        {
            OpenDoor();
        }
 
    }

    IEnumerator DelayBake()
    {
        yield return new WaitForSeconds(2);
        ObjectRefs.Instance.NavMesh.GetComponent<NavMeshSurface2d>().BuildNavMesh();
    }

    public void OpenDoor()
    {
        open = !open;
        if (open) Open();
        else Close();
    }

    void Open()
    {
        Debug.Log("open");
        anim.SetBool("open", true);
        StartCoroutine(DelayBake());
    }
    void Close()
    {
        Debug.Log("close");
        anim.SetBool("open", false);
        StartCoroutine(DelayBake());
    }
}
