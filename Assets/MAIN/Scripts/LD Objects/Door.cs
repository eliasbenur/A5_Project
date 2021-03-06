﻿using MyBox;
using Rewired;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Door : Obj
{
    [System.NonSerialized]
    public bool open = false;
    Animator anim;

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

        playerControl.Set_playerEnabled(false);
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
            gameObject.GetComponent<Door>().OpenDoor();
            Destroy(gameObject.GetComponent<LockedDoor>());
            gameObject.GetComponent<Door>().closeKey = false;
        }
        done = true;
        canvas_lockedkey.transform.GetChild(2).gameObject.SetActive(false);
        playerControl.Set_playerEnabled(true);
        yield return null;
    }

    public override void ActiveEvent()
    {
        base.ActiveEvent();

        if (closeKey)
        {
            if(playerControl.stat.power == Power.AllKey)
            {
                //if (ObjectRefs.Instance.player.GetComponent<PlayerControl>().stat.nbKey_tmp > 0) {
                    ObjectRefs.Instance.player.GetComponent<PlayerControl>().Set_playerEnabled(true);
                    Pick(ObjectRefs.Instance.player.GetComponent<PlayerControl>());
                //} 
            }
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
