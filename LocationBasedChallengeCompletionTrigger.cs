using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocationBasedChallengeCompletionTrigger : MonoBehaviour
{
    public bool LocationFound;

    private void OnTriggerEnter(Collider other)
    {

        if (GameController.instance.currenGameState == GameController.gameState.gameplay || GameController.instance.currenGameState == GameController.gameState.resetting)
        {
            if (other.gameObject.tag == "Player")
            {

                if (LocationFound = false)
                {
                    LocationFound = true;
                    GameController.instance.IncrementLocationChallengesDiscoveredCount();
                }
            }

        }

    }

   
}
