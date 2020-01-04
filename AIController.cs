using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour
{
    public static AIController instance;

    public List<AIbehaviorData> SocialBehaviors;
    public List<AIbehaviorData> AgressiveBehaviors;

    public AIPositionMarker[] allMarkers = new AIPositionMarker[0];
    public List<AIPositionMarker> allLegalMarkers = new List<AIPositionMarker>();
    public List<SortedAIMarkerData> sortedMarkers = new List<SortedAIMarkerData>();

    public float legalMarkerUpdateTime = .5f;
    public float legalMarkerUpdateTimer = 0f;

    // Start is called before the first frame update
    void Start()
    {
        if (instance == null) 
        instance = this;

        GetAllAIMarkers();
    }

    private void Update()
    {
        legalMarkerUpdateTimer += Time.deltaTime;

        if(legalMarkerUpdateTimer > legalMarkerUpdateTime)
        {
            legalMarkerUpdateTimer = 0f;
            GetAllLegalMarkers();
        }
    }


    public AIbehaviorData.AiBehaviors GetRandomAISocialBehaviorGroup( AIbehaviorData.AiBehaviors pBehavior )
    {
        //*** We defaut to a spwaning behavior
        AIbehaviorData.AiBehaviors oBehavior = AIbehaviorData.AiBehaviors.Spawning;

        for (int i = 0; i < SocialBehaviors.Count; i++)
        {
            if (SocialBehaviors[i].beahviorGroup == pBehavior)
                oBehavior = pBehavior;


        }

        return oBehavior;

    }

    public AIbehaviorData.AiBehaviors GetRandomAICombatBehaviorGroup()
    {
        int oRandomBehaviorIndex = Random.Range(0, AgressiveBehaviors.Count);

        //*** We default to a spwaning behavior
        AIbehaviorData.AiBehaviors oBehavior = AIbehaviorData.AiBehaviors.Spawning;

        oBehavior = AgressiveBehaviors[oRandomBehaviorIndex].beahviorGroup;

        return oBehavior;

    }

   

    public AIbehaviorData.AiBehaviors GetSpecificAISocialBehaviorGroup(AIbehaviorData.AiBehaviors pBehavior )
    {
        //*** We defaut to a spwaning behavior
        AIbehaviorData.AiBehaviors oBehavior = AIbehaviorData.AiBehaviors.Spawning;

        for (int i = 0; i < SocialBehaviors.Count; i++)
        {
            if (SocialBehaviors[i].beahviorGroup == pBehavior)
                oBehavior = pBehavior;


        }

        return oBehavior;

    }

    public AIbehaviorData.AiBehaviors GetSpecificAICombatBehaviorGroup(AIbehaviorData.AiBehaviors pBehavior)
    {
        //*** We defaut to a spwaning behavior
        AIbehaviorData.AiBehaviors oBehavior = AIbehaviorData.AiBehaviors.Spawning;

        for (int i = 0; i < AgressiveBehaviors.Count; i++)
        {
            if (AgressiveBehaviors[i].beahviorGroup == pBehavior)
                oBehavior = pBehavior;


        }

        return oBehavior;

    }


    public AISubBehaviorData GetSocialAISubBehavior(AIbehaviorData.AiBehaviors pBehavior, int pSubBehaviorIndex = -1)
    {
        AISubBehaviorData oSubBehaviorData = null;




        for (int i = 0; i < SocialBehaviors.Count; i++)
        {
            if (SocialBehaviors[i].beahviorGroup == pBehavior)
            {
                ///*** Get Sub behavior index , if not already assigned
                if (pSubBehaviorIndex == -1)
                    pSubBehaviorIndex = Random.Range(0, SocialBehaviors[i].AISubBehaviors.Count);

                for (int x = 0; x < SocialBehaviors[i].AISubBehaviors.Count; x++)
                {

                    oSubBehaviorData = SocialBehaviors[i].AISubBehaviors[pSubBehaviorIndex];

                }

            }
        }


        return oSubBehaviorData;

    }

    public AISubBehaviorData GetCombatAISubBehavior(AIbehaviorData.AiBehaviors pBehavior, int pSubBehaviorIndex = -1)
    {
        AISubBehaviorData oSubBehaviorData = null;


        for (int i = 0; i < AgressiveBehaviors.Count; i++)
        {
            if (AgressiveBehaviors[i].beahviorGroup == pBehavior)
            {
                ///*** Get Sub behavior index , if not already assigned
                if (pSubBehaviorIndex == -1)
                    pSubBehaviorIndex = Random.Range(0, AgressiveBehaviors[i].AISubBehaviors.Count);

                for (int x = 0; x < AgressiveBehaviors[i].AISubBehaviors.Count; x++)
                {
                   
                    oSubBehaviorData = AgressiveBehaviors[i].AISubBehaviors[pSubBehaviorIndex];

                }

            }
        }


        return oSubBehaviorData;

    }

    public void GetAllAIMarkers()
    {
     allMarkers = GameObject.FindObjectsOfType<AIPositionMarker>();


    }

    public void GetAllLegalMarkers()
    {
        allLegalMarkers = new List<AIPositionMarker>();

        for ( int i = 0; i <allMarkers.Length; i++)
        {
            if (allMarkers[i].isActive)
                allLegalMarkers.Add(allMarkers[i]);

        }


    }


    public Transform GetIdealMovementPosition(Vector3 pVector)
    {
        int markerIndex = 0;

        sortedMarkers = new List<SortedAIMarkerData>(allLegalMarkers.Count);

        for (int i = 0; i < allLegalMarkers.Count; i++)
        {
            if (allLegalMarkers[i] != null)
            {
                SortedAIMarkerData oData = new SortedAIMarkerData();

                oData.markerObject = allLegalMarkers[i].gameObject;
                oData.distance = Vector3.Distance(pVector, allLegalMarkers[i].gameObject.transform.position);
                sortedMarkers.Add(oData);

                //Debug.Log(" Adding data to the list!!!!!");
            }

        }

        sortedMarkers.Sort(SortByDistance);

        markerIndex = Random.Range(0, Mathf.FloorToInt(sortedMarkers.Count * .5f) );


        if (sortedMarkers[markerIndex].markerObject == null)
            markerIndex = 0;


        return sortedMarkers[markerIndex].markerObject.transform;

    }


   


    //*** Sort thing classes by distance
    static int SortByDistance(SortedAIMarkerData p1, SortedAIMarkerData p2)
    {
        return p1.distance.CompareTo(p2.distance);
    }

    


}

[System.Serializable]
public class SortedAIMarkerData
{
    public GameObject markerObject;
    public float distance;


}