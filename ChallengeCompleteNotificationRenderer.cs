using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

using DG.Tweening;

public class ChallengeCompleteNotificationRenderer : MonoBehaviour
{

    public TextMeshPro challengeCompleteTextMesh;

    public GameObject challengeCompleteContainer;

    // Start is called before the first frame update
    void Start()
    {

    }

   public void SetandRenderCompletedChallenge( string pCompletedChallengeText)
    {
        //*** Set the text for the ocmpleted challenge menu
        challengeCompleteTextMesh.text = pCompletedChallengeText;

        challengeCompleteContainer.SetActive(true);

        Invoke("DelayedHide",5f);
    }

    public void DelayedHide() {
        challengeCompleteContainer.transform.DOScale(0f, .3f).onComplete += DelayedDisable;
    }

    public void DelayedDisable() {
        challengeCompleteContainer.SetActive(false);
        challengeCompleteContainer.transform.localScale = Vector3.one;
    }
}



