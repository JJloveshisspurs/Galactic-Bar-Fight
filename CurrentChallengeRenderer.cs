using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CurrentChallengeRenderer : MonoBehaviour
{
    public TextMeshPro challengeName;
    public TextMeshPro challengeDescription;
    public SavedChallengeData currentChallenge;

   public void PopulateChallengeVisual(SavedChallengeData pChallengeData)
    {
        currentChallenge = pChallengeData;

        RenderChallengeData();
    }

    public void RenderChallengeData()
    {

        challengeName.text = currentChallenge.challengeName;
        challengeDescription.text = currentChallenge.challengeDescription;
    }
}
