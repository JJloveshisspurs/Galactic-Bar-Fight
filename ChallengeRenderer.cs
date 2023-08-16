using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ChallengeRenderer : MonoBehaviour
{
    public TextMeshProUGUI challengeTitle;
    public TextMeshProUGUI challengeDescription;

    private SavedChallengeData challengeData;



    public void SetChallenge(SavedChallengeData pChallengeData) {

        challengeTitle.text = pChallengeData.challengeName;
        challengeDescription.text = pChallengeData.challengeDescription;

    }
}


