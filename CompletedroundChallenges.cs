using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CompletedroundChallenges : MonoBehaviour
{
    public TextMeshPro challengeName;
    public TextMeshPro challengeDescription;



    public void SetCompletedChallengeData(SavedChallengeData pSavedData)
    {

        challengeName.text = pSavedData.challengeName;
        challengeDescription.text = pSavedData.challengeDescription;

    }
}
