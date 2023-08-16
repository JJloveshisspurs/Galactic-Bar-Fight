using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChallengeList : MonoBehaviour
{
    [SerializeField] List<ChallengeStatus> statuses = new List<ChallengeStatus>();

    public event Action onUpdate;

    ChallengeStatus newChallenge;

    public IEnumerable<ChallengeStatus> GetStatuses()
    {
        return statuses;
    }

    public void AddChallenge(Challenges challenge)
    {
        if(HasChallenge(challenge))
        {
            return;
        }

        newChallenge = new ChallengeStatus(challenge);
        statuses.Add(newChallenge);

        if(onUpdate != null)
        {
            onUpdate();
        }
    }

    public bool HasChallenge(Challenges challenge)
    {
        return GetChallengeStatus(challenge) != null;
    }

    public void CompleteObjective(Challenges challenge, string objective)
    {
        ChallengeStatus status = GetChallengeStatus(challenge);

        status.CompletObjective(objective);

        if (status.IsComplete())
        {
            GiveReward(challenge);
            FindObjectOfType<DestroyChallenge>().wasComplete = true;
        }

        if (onUpdate != null)
        {
            onUpdate();
        }
    }

    private ChallengeStatus GetChallengeStatus(Challenges challenge)
    {
        foreach (ChallengeStatus status in statuses)
        {
            if (status.GetChallenge() == challenge)
            {
                return status;
            }
        }

        return null;
    }

    public void GiveReward(Challenges challenge)
    {
        //foreach(Challenges.Reward reward in challenge.GetRewards())
        //{
        //    //TODO Destroy gameobject and remove from list
        //}

        ChallengeStatus status = GetChallengeStatus(challenge);

        if (status.IsComplete())
        {
            statuses.Remove(newChallenge);
        }
    }

    public List<ChallengeStatus> GetListOfChallenges()
    {
        return statuses;
    }
}
