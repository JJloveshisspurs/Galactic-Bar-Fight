using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChallengeCompletionManager : MonoBehaviour
{
    public static ChallengeCompletionManager instance;

    public int killCount;

    public List<ChallengeData> basicKillChallenges;

    public ChallengeCompleteNotificationRenderer challengeNotificationRenderer;

    public void Start()
    {
        if (instance == null) 
        instance = this;
    }


    public void IncrementKillCount()
    {

        killCount++;

        CheckForChallengecompletion();
    }

    public void CheckForChallengecompletion()
    {
        for(int i = 0; i < basicKillChallenges.Count; i++)
        {

            //*** Check if challenge completion was shown
            if(basicKillChallenges[i].challengeDisplayedAlready == false)
            {
                //*** Check if we met completion criteria
                if( killCount >= basicKillChallenges[i].completionCriteria)
                {
                    basicKillChallenges[i].challengeDisplayedAlready = true;

                    challengeNotificationRenderer.SetandRenderCompletedChallenge(basicKillChallenges[i].challengeDescription);

                    return;

                }


            }



        }

    }
}
