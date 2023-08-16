using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveFile
{

    [SerializeField]
    public string savefileName;

    [SerializeField]
    public string lastUpdateTime;

    [SerializeField]
    public int lifeTimeScore;

    [SerializeField]
    public int lifeTimeKills;

    [SerializeField]
    public int powerupDrinksConsumed;

    [SerializeField]
    public bool completedIntroTutorial;


    public List<SavedChallengeData> challengeList;

    public void UpdateFileData( SaveFile pFileData) {

        savefileName = pFileData.savefileName;

        lastUpdateTime = System.DateTime.Now.ToString();

        lifeTimeScore = pFileData.lifeTimeScore;

        lifeTimeKills = pFileData.lifeTimeKills;

        powerupDrinksConsumed = pFileData.powerupDrinksConsumed;

        completedIntroTutorial = pFileData.completedIntroTutorial;

        //challengeList = new List<SavedChallengeData>();

        for(int i = 0; i < pFileData.challengeList.Count; i++) {

            challengeList[i] = pFileData.challengeList[i];


        }

    }


    public void InitializeNewChallengeData(List<SavedChallengeData> pChallengedata) {

        challengeList = new List<SavedChallengeData>();


        for(int i = 0; i < pChallengedata.Count; i++) {

            SavedChallengeData oData = new SavedChallengeData();


            oData.challengeType = pChallengedata[i].challengeType;

            oData.challengeName = pChallengedata[i].challengeName;

            oData.challengeDescription = pChallengedata[i].challengeDescription;

            oData.ChallengeRendered = pChallengedata[i].ChallengeRendered;

            oData.challengeCompletionKey = pChallengedata[i].challengeCompletionKey;

            oData.interactionCount = pChallengedata[i].interactionCount;

            oData.ChallengeComplete = pChallengedata[i].ChallengeComplete;

            challengeList.Add(oData);
        }




    }
}






