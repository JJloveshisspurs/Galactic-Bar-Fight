using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aria_PlayerLost_Quips : MonoBehaviour
{


    public float VO_Delay;


    public AudioSource gameOver_Quip_source;


    public List<AudioClip> lossVO_Clips;


    // Start is called before the first frame update
    void Start()
    {
        Invoke("PlayRandomizedLossAudio", VO_Delay);
    }

    public void PlayRandomizedLossAudio()
    {
        int oRandomQuip = Random.Range(0, lossVO_Clips.Count);


        gameOver_Quip_source.clip = lossVO_Clips[oRandomQuip];

        gameOver_Quip_source.Play();

    }
}
