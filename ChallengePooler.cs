using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ChallengePooler : MonoBehaviour
{
    public NewSaveDataTest SaveDataSystem;
    public LevelData.ChallengeSection challengeSection;
    public List<SavedChallengeData> incompleteChallengePool;
    public List<SavedChallengeData> incompleteFilteredChallengePool;

    public CurrentChallengeRenderer currentChallengeRender;

    public SavedChallengeData currentAssignedChallenge;

    public SavedChallengeData.ChallengeType filteredChallengeTypes;

    public bool sortChallengesByAscendingValue;

    // Start is called before the first frame update
    void Start()
    {


       Invoke("GetChallengePoolData",4f);
    }

    public void GetChallengePoolData()
    {
        SaveDataSystem = GameObject.FindObjectOfType<NewSaveDataTest>(false);

        if (SaveDataSystem != null)
        {

            //*** Set Incomplete challenge data for challenge pool

            if (challengeSection == LevelData.ChallengeSection.Hub)
            {

                incompleteChallengePool = SaveDataSystem.hubChallengesList;

                FilterChallenge();
            }

            if (challengeSection == LevelData.ChallengeSection.TrainingDroids)
            {

                incompleteChallengePool = SaveDataSystem.targetPracticeChallengeList;

                AssignRandomChallenge();
            }

            if (challengeSection == LevelData.ChallengeSection.ShootingGallery)
            {

                incompleteChallengePool = SaveDataSystem.shootingGalleryChallengesList;

                AssignRandomChallenge();
            }

        }

    }

    public void AssignRandomChallenge()
    {
        if (incompleteChallengePool.Count > 0)
        {
            
            SavedChallengeData oChallenges;

            int oChallengeIndex = Random.Range(0, incompleteChallengePool.Count);


            currentAssignedChallenge = incompleteChallengePool[oChallengeIndex];

            if (currentChallengeRender != null)
                currentChallengeRender.PopulateChallengeVisual(incompleteChallengePool[oChallengeIndex]);

         }
    }


    public void FilterChallenge()
    {
        if (incompleteChallengePool.Count > 0)
        {

            if (sortChallengesByAscendingValue)
            {

                //*** Create list of hub challenges where the challenges are incomplete AND they are set as hub challenges AND the challenges are set to active!
                var sortedChallenges = incompleteChallengePool.Where(c => c.ChallengeComplete == false
                && c.challengeType == filteredChallengeTypes
                && c.challengeAvailability == SavedChallengeData.ChallengeAvailability.Active).OrderBy(o => o.interactionCount).ToList();


                incompleteFilteredChallengePool = sortedChallenges.ToList<SavedChallengeData>();


                //*** Set first Sorted Challenge as immediate hcallenge to complete

                currentAssignedChallenge = incompleteFilteredChallengePool[0];
            }
            else
            {


                //*** Set Challenge

            }
        }
    }
}
