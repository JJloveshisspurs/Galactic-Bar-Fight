using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompleteRoundChallengesRenderer : MonoBehaviour
{

    public float challengeRevealInitialDelay;
    public float breakBetweenChallengeReveals;

    public List<CompletedroundChallenges> completedChallenges;


    public int completedChallengeCount;

    public AudioSource revealSFX;

    public GameObject Aria_Victory_Quips;

    public GameObject highScoreObject;

    public  bool newHighScoreDetected; 

    public void SetCompletedChallengesList(List<SavedChallengeData> pCompletedChallenges, bool pNewHighScoreDetected)
    {

        completedChallengeCount = pCompletedChallenges.Count;

        for(int i = 0; i < pCompletedChallenges.Count; i++)
        {

            completedChallenges[i].SetCompletedChallengeData(pCompletedChallenges[i]);

        }

        newHighScoreDetected = pNewHighScoreDetected;

        if(completedChallengeCount > 0)
        StartCoroutine(RenderCompletedChallenges());

    }

    IEnumerator RenderCompletedChallenges()
    {
        yield return new WaitForSeconds(challengeRevealInitialDelay);

        Aria_Victory_Quips.SetActive(true);

        yield return new WaitForSeconds(challengeRevealInitialDelay);

        if (completedChallengeCount >= 1)
        {
            completedChallenges[0].gameObject.SetActive(true);

            revealSFX.pitch = 1f;

            revealSFX.Play();

        }
        yield return new WaitForSeconds(breakBetweenChallengeReveals);

        if (completedChallengeCount >= 2)
        {
            completedChallenges[1].gameObject.SetActive(true);

            revealSFX.pitch = 1.1f;

            revealSFX.Play();
        }

        yield return new WaitForSeconds(breakBetweenChallengeReveals);

        if (completedChallengeCount >= 3)
        {
            completedChallenges[2].gameObject.SetActive(true);

            revealSFX.pitch = 1.2f;

            revealSFX.Play();
        }

        yield return new WaitForSeconds(breakBetweenChallengeReveals);

        if(highScoreObject != null && newHighScoreDetected)
        highScoreObject.SetActive(true);
    }
}
 