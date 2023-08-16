using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChallengeCompletion : MonoBehaviour
{
    [SerializeField] Challenges challenge;
    [SerializeField] string objective;
    
    public void CompleteObjective()
    {
        ChallengeList challengeList = GameObject.FindGameObjectWithTag("Player").GetComponent<ChallengeList>();
        challengeList.CompleteObjective(challenge, objective);
    }
}
