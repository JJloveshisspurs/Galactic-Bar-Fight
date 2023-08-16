using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavigationDirector : MonoBehaviour
{

    public NavMeshAgent navMeshAgent;
    public Transform positionTarget;

    public float startDelay;
    public float repetitionRate;

    public float stoppedAgentCheckInterval = .3f;

    public float stoppedAgentCheckTimer = 0f;


    public float stopDistance = 2f;

    private NewSaveDataTest saveDataManager;


    private bool initialized;

    // Start is called before the first frame update
    void Start()
    {
        Invoke("SetUpNavMeshAgent", startDelay);
    }


    public void SetUpNavMeshAgent()
    {
        Debug.Log("Setting up Nav Director Nav Mesh Agent !!! ");

        saveDataManager = GameObject.FindObjectOfType<NewSaveDataTest>();

        if (saveDataManager != null)
        {
            Debug.Log("Found Save Data Manager !!!! ");

            if (saveDataManager.storedData.enteredFirstCombatArena == false)
            {

                Debug.Log(" Has not Entered first combat arena!!!");
                CenterAndFollow();


            }
            else
            {

                Debug.Log("Entered first combat arena, disabling!!!");
                this.gameObject.SetActive(false);
            }
        }
        else
        {
            Debug.Log("Save data Manager is null!!!");

        }
    }

    // Update is called once per frame
    void Update()
    {

        if (initialized)
        {

            //if(positionTarget != null)
            //navMeshAgent.SetDestination(positionTarget.transform.position);


            if (positionTarget == null)
                this.gameObject.SetActive(false);

            stoppedAgentCheckTimer += Time.deltaTime;

            //Debug.Log("Nav mesh stopping distance ==" + navMeshAgent.remainingDistance.ToString());

            if (stoppedAgentCheckInterval <= stoppedAgentCheckTimer)
            {
                stoppedAgentCheckTimer = 0f;

                CheckStopDistance();
            }
        }
    }

    public void CenterAndFollow()
    {

        Debug.Log("CENTERING Nav Director Nav Mesh Agent !!! ");
        //navMeshAgent.transform.localPosition = Vector3.zero;
        //arrowrenderer.start = this.gameObject.transform.position;
        navMeshAgent.gameObject.SetActive(false);

        navMeshAgent.transform.localPosition = Vector3.zero;


        navMeshAgent.gameObject.SetActive(true);


        if (positionTarget != null)
        {
            Debug.Log("Position Target != null , setting  Nav Mesh Agent target !!! ");
            navMeshAgent.transform.parent = null;

            navMeshAgent.enabled = true;
            navMeshAgent.SetDestination(positionTarget.transform.position);

            initialized = true;

            Invoke("ResetNavAgent", repetitionRate);
        }

    }

    public void ResetNavAgent()
    {
        Debug.Log("Resetting up Nav Director Nav Mesh Agent !!! ");
       
            navMeshAgent.gameObject.SetActive(false);
            navMeshAgent.transform.parent = this.gameObject.transform;
            navMeshAgent.transform.localPosition = Vector3.zero;

            Invoke("SetUpNavMeshAgent", 1f);
        

    }


    public void CheckStopDistance()
    {
        float oDistance = Vector3.Distance(navMeshAgent.transform.position, positionTarget.position);

        Debug.Log("distance from target == " + oDistance.ToString());

        if (oDistance <= stopDistance)
            navMeshAgent.enabled = false; 

    }
}
