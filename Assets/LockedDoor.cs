using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Rewired;

public class LockedDoor : MonoBehaviour
{
    Player player;
    public float timer;
    public string[] sequence;
    public GameObject canvas;
    public Text timerText;
    public Image[] inputContainer;
    public Sprite[] inputSet;

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
            inputContainer[i].sprite = inputSet[random];
            inputContainer[i].color = Color.white;
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
        canvas.transform.GetChild(2).gameObject.SetActive(true);
        Debug.Log(canvas.transform.GetChild(2));

        float temp = timer;
        int count = 0;
        while (temp >= 0)
        {
            if (player.GetButtonUp(RewiredConsts.Action.A) && canSelect == false)
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
                    if (player.GetButtonDown(RewiredConsts.Action.A) && sequence[count] == "A" ||
                    player.GetButtonDown(RewiredConsts.Action.B) && sequence[count] == "B" ||
                    player.GetButtonDown(RewiredConsts.Action.X) && sequence[count] == "X" ||
                    player.GetButtonDown(RewiredConsts.Action.Y) && sequence[count] == "Y")
                    {
                        inputContainer[count].color = Color.green;
                        count++;
                    }
                    else
                    {
                        Debug.Log("FAIL");
                        inputContainer[count].color = Color.red;
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
        }
        done = true;
        canvas.transform.GetChild(2).gameObject.SetActive(false);
        playerControl.activated = true;
        yield return null;
    }
}
