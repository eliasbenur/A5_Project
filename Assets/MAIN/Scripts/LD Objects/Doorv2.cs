﻿using MyBox;
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

    [Separator("Closed Door")]
    public bool closeKey = false;
    Player player;
    [ConditionalField(nameof(closeKey))] public float timer_miniGame;
    string[] sequence;
    [ConditionalField(nameof(closeKey))] public GameObject canvas_lockedkey;
    Text timerText;
    //Player can start to Make Inputs (for the miniGame)
    bool canSelect = false;
    //MiniGame Succes -> Door Open
    bool success = false;

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

        if (closeKey)
        {
            timerText = canvas_lockedkey.transform.GetChild(0).GetComponent<Text>();
        }

    }

    public void FillSequence()
    {
        sequence = new string[4];
        int random = Random.Range(0, 4);
        for (int i = 0; i < 4; i++)
        {
            sequence[i] = ((inputs)random).ToString();
            //ObjectRefs.Instance.inputContainer[i].sprite = ObjectRefs.Instance.inputSet[random];
            //ObjectRefs.Instance.inputContainer[i].color = Color.white;
            random = Random.Range(0, 4);
        }
    }

    public void Pick(PlayerControl playerControl)
    {
        StartCoroutine(PickLock(playerControl));
    }

    public IEnumerator PickLock(PlayerControl playerControl)
    {
        canSelect = false;
        bool inputFailed = false;

        playerControl.Set_playerEnabled(false);
        FillSequence();
        timerText.text = timer_miniGame.ToString();
        canvas_lockedkey.SetActive(true);

        float temp = timer_miniGame;
        int count = 0;
        SwitchColor(count);

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
                        count++;
                        if (count < 4)
                            SwitchColor(count);
                    }
                    else
                    {
                        //playerControl.SetpowerNb(playerControl.GetpowerNb() - 1);
                        break;
                    }
                }
            }
            temp = temp - 0.01f;
            timerText.text = ((float)((int)(temp * 10)) / 10).ToString();
            yield return new WaitForSecondsRealtime(0.0001f);
        }
        EndColor(success, count);
        yield return new WaitForSeconds(0.5f);
        if (success == true)
        {
            gameObject.GetComponent<Doorv2>().OpenDoor();
            Destroy(gameObject.GetComponent<LockedDoor>());
            gameObject.GetComponent<Doorv2>().closeKey = false;
        }
        else
        {
            ObjectRefs.Instance.inputContainer[count].color = Color.red;
        }
        canSelect = false;
        canvas_lockedkey.SetActive(false);
        playerControl.Set_playerEnabled(true);
        yield return null;
    }

    public override void ActiveEvent()
    {
        base.ActiveEvent();

        if (closeKey)
        {
            if(playerControl.securityZone1)
            {
                playerControl.Set_playerEnabled(true);
                Pick(playerControl);
            }
        }
        else
        {
            OpenDoor();
        }
 
    }

    public void OpenDoor()
    {
        open = !open;
        if (open) Open();
        else Close();
    }

    void Open()
    {
        //Left 
        transform.GetChild(0).transform.localPosition = posiPorteLeft + Vector3.right * decalage;
        transform.GetChild(0).transform.gameObject.GetComponent<Collider2D>().enabled = false;
        transform.GetChild(0).transform.gameObject.GetComponent<SpriteRenderer>().color = new Vector4(0,1,1,0.3f);
        //Right
        transform.GetChild(1).transform.localPosition = posiPorteRight - Vector3.right * decalage;
        transform.GetChild(1).transform.gameObject.GetComponent<Collider2D>().enabled = false;
        transform.GetChild(1).transform.gameObject.GetComponent<SpriteRenderer>().color = new Vector4(0, 1, 1, 0.3f);

        //NavMesh 
        transform.GetChild(0).GetComponent<NavMeshObstacle>().enabled = false;
        transform.GetChild(1).GetComponent<NavMeshObstacle>().enabled = false;
    }
    void Close()
    {
        //Left
        transform.GetChild(0).transform.localPosition = posiPorteLeft;
        transform.GetChild(0).transform.gameObject.GetComponent<Collider2D>().enabled = true;
        transform.GetChild(0).transform.gameObject.GetComponent<SpriteRenderer>().color = new Vector4(0, 1, 1, 1);
        //Right
        transform.GetChild(1).transform.localPosition = posiPorteRight;
        transform.GetChild(1).transform.gameObject.GetComponent<Collider2D>().enabled = true;
        transform.GetChild(1).transform.gameObject.GetComponent<SpriteRenderer>().color = new Vector4(0, 1, 1, 1);

        //NavMesh 
        transform.GetChild(0).GetComponent<NavMeshObstacle>().enabled = true;
        transform.GetChild(1).GetComponent<NavMeshObstacle>().enabled = true;
    }

    public void EndColor(bool success, int count)
    {
        Color color;
        if (success == true)
        {
            color = Color.green;
        }
        else
        {
            color = Color.red;
        }

        if (count == 4)
            count = 3;

        switch (sequence[count])
        {
            case "A":
                ObjectRefs.Instance.inputContainer[0].color = color;
                break;
            case "B":
                ObjectRefs.Instance.inputContainer[1].color = color;
                break;
            case "X":
                ObjectRefs.Instance.inputContainer[2].color = color;
                break;
            case "Y":
                ObjectRefs.Instance.inputContainer[3].color = color;
                break;
        }
    }

    public void SwitchColor(int count)
    {
        for (int i = 0; i < 4; i++)
        {
            ObjectRefs.Instance.inputContainer[i].color = Color.black;
        }
        StartCoroutine(Cooldown(count));
    }

    public IEnumerator Cooldown(int count)
    {
        if (count != 0)
            yield return new WaitForSeconds(0.13f);
        switch (sequence[count])
        {
            case "A":
                ObjectRefs.Instance.inputContainer[0].color = Color.white;
                break;
            case "B":
                ObjectRefs.Instance.inputContainer[1].color = Color.white;
                break;
            case "X":
                ObjectRefs.Instance.inputContainer[2].color = Color.white;
                break;
            case "Y":
                ObjectRefs.Instance.inputContainer[3].color = Color.white;
                break;
        }
        yield return null;
    }
}
