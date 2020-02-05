using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{

    public AudioClip movementSnd, guardesAttention, itemTaken, winSnd, normalMusic, chaseMusic, searchMusic;
    AudioSource movementSnd_src, guardesAttention_src, itemTaken_src, winSnd_src, normalMusic_src;
    // Start is called before the first frame update
    void Start()
    {
        movementSnd_src = gameObject.AddComponent<AudioSource>();
        movementSnd_src.clip = movementSnd;
        movementSnd_src.volume = 0.5f;

        guardesAttention_src = gameObject.AddComponent<AudioSource>();
        guardesAttention_src.clip = guardesAttention;
        guardesAttention_src.volume = 0.1f;

        itemTaken_src = gameObject.AddComponent<AudioSource>();
        itemTaken_src.clip = itemTaken;
        itemTaken_src.volume = 0.5f;

        winSnd_src = gameObject.AddComponent<AudioSource>();
        winSnd_src.clip = winSnd;
        winSnd_src.volume = 0.3f;

        normalMusic_src = gameObject.AddComponent<AudioSource>();
        normalMusic_src.clip = normalMusic;
        normalMusic_src.volume = 0.5f;
        normalMusic_src.loop = true;
        normalMusic_src.Play();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void PlaymovementSnd()
    {
        movementSnd_src.Play();

    }

    public void PlayguardesAttention()
    {

        guardesAttention_src.Play();

    }

    public void PlayitemTaken()
    {

        itemTaken_src.Play();


    }

    public void PlaywinSnd()
    {

        winSnd_src.Play();

    }

    public void playNormalMusic()
    {
        normalMusic_src.Stop();
        normalMusic_src.clip = normalMusic;
        normalMusic_src.Play();
    }

    public void playChaseMusic()
    {
        normalMusic_src.Stop();
        normalMusic_src.clip = chaseMusic;
        normalMusic_src.Play();
    }

    public void playSearchMusic()
    {
        normalMusic_src.Stop();
        normalMusic_src.clip = searchMusic;
        normalMusic_src.Play();
    }
}