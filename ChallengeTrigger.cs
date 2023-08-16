using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ChallengeTrigger : MonoBehaviour
{
    [SerializeField] string action;
    [SerializeField] UnityEvent onTrigger;

    public void Trigger()
    {
        if(action == "UpdateChallenge")
        {
            onTrigger.Invoke();
        }
    }
}
