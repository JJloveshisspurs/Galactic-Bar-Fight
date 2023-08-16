using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyChallenge : MonoBehaviour
{
    List<ChallengeStatus> currentStatuses = new List<ChallengeStatus>();

    public bool wasComplete = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(wasComplete)
        {
            Destroy(gameObject);
        }
    }

    private void RemoveChallenge()
    {

    }
}
