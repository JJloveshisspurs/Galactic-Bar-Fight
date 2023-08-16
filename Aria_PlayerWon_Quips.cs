using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aria_PlayerWon_Quips : MonoBehaviour
{
    public float VO_Delay;


    public AudioSource playerWonRoundGameover;


    public List<AudioClip> winVO_Clips;



    // Start is called before the first frame update
    void OnEnable()
    {
        Invoke("PlayRandomizedWinAudio", VO_Delay);
    }

    public void PlayRandomizedWinAudio()
    {
        int oRandomQuip = Random.Range(0, winVO_Clips.Count);


        playerWonRoundGameover.clip = winVO_Clips[oRandomQuip];

        playerWonRoundGameover.Play();

    }
}
