using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ChallengeItemUI : MonoBehaviour
{
    [SerializeField] TextMeshPro title;
    [SerializeField] TextMeshPro progress;

    ChallengeStatus status;

    public void Setup(ChallengeStatus status)
    {
        this.status = status;

        title.text = status.GetChallenge().GetTitle();
        progress.text = status.GetCompeletedCount() + "/" + status.GetChallenge().GetObjectiveCount();
    }

    public ChallengeStatus GetChallengeStatus()
    {
        return status;
    }
}
