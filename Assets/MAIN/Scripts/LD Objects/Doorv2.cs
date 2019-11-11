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

        timerText = canvas_lockedkey.transform.GetChild(0).GetComponent<Text>();
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
        canSelect = false;
        bool inputFailed = false;

        playerControl.activated = false;
        FillSequence();
        timerText.text = timer_miniGame.ToString();
        canvas_lockedkey.SetActive(true);

        float temp = timer_miniGame;
        int count = 0;
        while (temp >= 0 && !inputFailed)
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
                        inputFailed = true;
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
        else
        {
            playerControl.SetpowerNb(playerControl.GetpowerNb() - 1);
            ObjectRefs.Instance.inputContainer[count].color = Color.red;
        }
        canSelect = false;
        canvas_lockedkey.SetActive(false);
        playerControl.activated = true;
        yield return null;
    }

    public override void ActiveEvent()
    {
        base.ActiveEvent();

        if (closeKey)
        {
            //if(playerControl.stat.power == Power.AllKey)
            if(playerControl.securityZone1)
            {
                //if (playerControl.stat.nbKey_tmp > 0) {
                    playerControl.activated = true;
                    Pick(playerControl);
                //} 
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
}
