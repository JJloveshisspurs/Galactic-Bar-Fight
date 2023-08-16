using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomDanceRoutine : MonoBehaviour
{

    public Animator characterDanceAnimator;
    public Animator characterDanceAnimatorTwin;

    AnimatorStateInfo animationState;

    AnimatorClipInfo[] myAnimatorClip;

    float animationTime = 0f;

    public const string danceNamePrefix = "Base Layer.Dance";


    public int maxDanceNum = 9;

    public int currentDanceNum;

    string currentDanceName = "";

    public bool isTheTwins;

    // Start is called before the first frame update
    void Start()
    {
        InitializeDance();
    }

    public void InitializeDance()
    {
        //Get randomized dance number 
        currentDanceNum = Random.Range(0, maxDanceNum);

        //Create randomized dance name
        currentDanceName = danceNamePrefix + currentDanceNum.ToString();

        characterDanceAnimator.CrossFade(currentDanceName, .2f);

        if(isTheTwins)
            characterDanceAnimatorTwin.CrossFade(currentDanceName, .2f);

       

        Debug.Log("animatin time == " + characterDanceAnimator.playbackTime.ToString());
        Invoke("InitializeDance", 5f);
    }


}
