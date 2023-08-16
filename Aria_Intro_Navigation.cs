using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Aria_Intro_Navigation : MonoBehaviour
{

//*** AR Character moving around the world
    public NavMeshAgent ariaNavigation;

    //*** List of points the AI character will move to in desired moement order
    public List<Transform> Navigationpoints;

    //*** Made this a singleton to iterate move position
    public static Aria_Intro_Navigation instance;

    //*** Index of active target node / point to move to
    public int ariaNodeIndex;

    

    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
            instance = this;


        Invoke("CheckForTutorialCompletion",1f);

    }

    public void CheckForTutorialCompletion()
    {
        //Checks if player has completed tutorial before
        
                Invoke("InititalizeAriaFlightpath", 3f);
        

    }


    public void InititalizeAriaFlightpath() {
        //*** Tell AI to target first position
        ariaNavigation.SetDestination(Navigationpoints[0].position);

        //*** this is redundant it should be 0 anyway
        ariaNavigation.SetDestination(Navigationpoints[ariaNodeIndex].position);

    }




    public void GoToNextariaNode() {

        //increment index of target position
        ariaNodeIndex = ariaNodeIndex + 1;

        //*** Mote to next position
        ariaNavigation.SetDestination(Navigationpoints[ariaNodeIndex].position);

    }

    public void GoToFinalAriaTutorialNode()
    {

        //increment index of target position
        ariaNodeIndex = Navigationpoints.Count - 1;

        //*** Mote to next position
        ariaNavigation.SetDestination(Navigationpoints[ariaNodeIndex].position);

    }

    public void AriaArrived() {

        //*** Tell AI Character to stop reached position
        ariaNavigation.isStopped = true;


    }

    public void AriaGetMoving() {

        //*** Tell AI Character to continue movement to next position
        ariaNavigation.isStopped = false;
    }
}



