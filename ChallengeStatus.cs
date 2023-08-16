using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ChallengeStatus
{
    [SerializeField] Challenges challenge;
    [SerializeField] List<string> completedObjectives = new List<string>();

    public ChallengeStatus(Challenges challenge)
    {
        this.challenge = challenge;
    }

    public Challenges GetChallenge()
    {
        return challenge;
    }

    public bool IsComplete()
    {
        foreach(var objective in challenge.GetObjectives())
        {
            if(!completedObjectives.Contains(objective.reference))
            {
                return false;
            }
        }

        return true;
    }

    public int GetCompeletedCount()
    {
        return completedObjectives.Count;
    }

    public bool IsObjectiveComplete(string objective)
    {
        return completedObjectives.Contains(objective);
    }

    public void CompletObjective(string objective)
    {
        if(challenge.HasObjective(objective))
        {
            completedObjectives.Add(objective);
        }
    }
}
