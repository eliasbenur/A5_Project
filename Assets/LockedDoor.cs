using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Rewired;

[RequireComponent(typeof(Door))]
public class LockedDoor : MonoBehaviour
{
    Player player;
    public float timer;
    public string[] sequence;
    public GameObject canvas;
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

    private void Start()
    {
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
        timerText.text = timer.ToString();
        canvas.transform.GetChild(2).gameObject.SetActive(true);
        Debug.Log(canvas.transform.GetChild(2));

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
        canvas.transform.GetChild(2).gameObject.SetActive(false);
        playerControl.Set_playerEnabled(true);
        yield return null;
    }
}
