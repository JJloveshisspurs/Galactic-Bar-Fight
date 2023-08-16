using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChallengeGiver : MonoBehaviour
{
    [SerializeField] Challenges challenge;


    public void GiveChallenge()
    {
        ChallengeList challengeList = GameObject.FindGameObjectWithTag("Player").GetComponent<ChallengeList>();
        challengeList.AddChallenge(challenge);
    }
}
