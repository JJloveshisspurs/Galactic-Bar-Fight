using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChallengeViewerButton : MonoBehaviour
{

    public bool previousButton;

    public bool previous10Button;

    public bool Next10Button;


    public ChallengeViewer challengeViewer;

    public AudioSource buttonPressSound;


    public void SelectButton()
    {

        if (previousButton)
        {

            challengeViewer.ViewPReviousChallenge();

        }
        else
        {


            challengeViewer.ViewNextChallenge();
        }

        buttonPressSound.Play();


    }

    public void SelectByTensButton()
    {

        if (previous10Button)
        {

            challengeViewer.ViewPRevious10Challenge();

        }


        if(Next10Button)
        {


            challengeViewer.ViewNext10Challenge();
        }

        buttonPressSound.Play();


    }
}
