using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class NewSaveDataTest : MonoBehaviour
{
    public string saveDataKey = "GBF_SavedData";


    public SaveFileUpdated saveDataModel;


    public SaveFileUpdated storedData;

    public List<SavedChallengeData> completedChallengesList;
    public List<SavedChallengeData> availableChallengesList;
    public List<SavedChallengeData> hubChallengesList;
    public List<SavedChallengeData> targetPracticeChallengeList;
    public List<SavedChallengeData> shootingGalleryChallengesList;
    public List<SavedChallengeData> EasyLevel1IncompleteChallengeList;
    public List<SavedChallengeData> MediumLevel1IncompleteChallengeList;
    public List<SavedChallengeData> HardLevel1IncompleteChallengeList;

    public List<SavedChallengeData> EasyLevel2IncompleteChallengeList;
    public List<SavedChallengeData> MediumLevel2IncompleteChallengeList;
    public List<SavedChallengeData> HardLevel2IncompleteChallengeList;

    public List<SavedChallengeData> EasyLevel3IncompleteChallengeList;
    public List<SavedChallengeData> MediumLevel3IncompleteChallengeList;
    public List<SavedChallengeData> HardLevel3IncompleteChallengeList;

    // Start is called before the first frame update
    void Start()
    {
        
        //***  Mark game object as not to be destroyed on load
        DontDestroyOnLoad(this.gameObject);
        LoadSaveData();
    }

    public void LoadSaveData()
    {
        string oData = "";



        if (PlayerPrefs.HasKey(saveDataKey))
        {
            Debug.Log("Loading DATA!!!!");

            oData = PlayerPrefs.GetString(saveDataKey);

            storedData = JsonUtility.FromJson<SaveFileUpdated>(oData);


            //storedData = PlayerPrefs.lo
        }
        else
        {
            Debug.Log("CREATING SAVE DATA!!!!");

            SaveNewData();
        }



        //*** Validate data
        ValidateData();

    }

    public void SaveNewData()
    {

        string oData = "";


        Debug.Log("CRATING SAVE DATA!!!!");

        //*** Serialize  central save data model
        oData = JsonUtility.ToJson(saveDataModel);

        //*** Save central save data model to playerprefs
        PlayerPrefs.SetString(saveDataKey, oData);

        //*** deserialize stored data as beginning  save data model
        storedData = JsonUtility.FromJson<SaveFileUpdated>(oData);


    }


    public void SaveData()
    {

        string oData = "";


        Debug.Log("CRATING SAVE DATA!!!!");

        //*** Serialize Stored data
        oData = JsonUtility.ToJson(storedData);

        //*** Saved Stored data to player prefs
        PlayerPrefs.SetString(saveDataKey, oData);

        //*** Reload stored data
        storedData = JsonUtility.FromJson<SaveFileUpdated>(oData);


    }

    public void ValidateData()
    {

        int oDifferenceInChallengeCount = 0;


        //*** Check for and update data model based on any new changes

        for (int i = 0; i < storedData.levels.Count; i++)
        {

            //*** Update challenge availability based on central data model
            storedData.levels[i].levelAvailability = saveDataModel.levels[i].levelAvailability;
            storedData.levels[i].levelName = saveDataModel.levels[i].levelName;
            storedData.levels[i].challengeSection = saveDataModel.levels[i].challengeSection;


            for (int x = 0; x < storedData.levels[i].challengeList.Count; x++)
            {

                //*** Check if the loaded data model and the data model prefab have the same amount of challenges , if prefab has more , add new challenge data slots
                oDifferenceInChallengeCount = (saveDataModel.levels[i].challengeList.Count - storedData.levels[i].challengeList.Count);


                if (oDifferenceInChallengeCount > 0)
                {
                    Debug.Log("New Challenge data detected!!!! adding new challenges : " + oDifferenceInChallengeCount.ToString());


                    for (int z = 0; z < oDifferenceInChallengeCount; z++)
                    {

                        SavedChallengeData oData = new SavedChallengeData();


                        storedData.levels[i].challengeList.Add(oData);


                        Debug.Log("New Challenge Added!!!! : " + oDifferenceInChallengeCount.ToString());
                    }


                }


                //*** Update challenge availability based on central data model
                storedData.levels[i].challengeList[x].challengeAvailability = saveDataModel.levels[i].challengeList[x].challengeAvailability;

                //*** Update challenge description based on central data model
                storedData.levels[i].challengeList[x].challengeDescription = saveDataModel.levels[i].challengeList[x].challengeDescription;

                //*** Update challenge completion key based on central data model
                storedData.levels[i].challengeList[x].challengeCompletionKey = saveDataModel.levels[i].challengeList[x].challengeCompletionKey;

                //*** Update challenge difficulty mode based on central data model
                storedData.levels[i].challengeList[x].challengeDifficultyMode = saveDataModel.levels[i].challengeList[x].challengeDifficultyMode;

                //*** Update challenge name based on central data model
                storedData.levels[i].challengeList[x].challengeName = saveDataModel.levels[i].challengeList[x].challengeName;

                //*** Update challenge style based on central data model
                storedData.levels[i].challengeList[x].challengeStyle = saveDataModel.levels[i].challengeList[x].challengeStyle;

                //*** Update challenge type based on central data model
                storedData.levels[i].challengeList[x].challengeType = saveDataModel.levels[i].challengeList[x].challengeType;

                storedData.levels[i].challengeList[x].interactionCount = saveDataModel.levels[i].challengeList[x].interactionCount;
            }

        }



        SaveData();


        OrganizeData();
    }




    public void OrganizeData()
    {
        ////*** Create list of challenges in the game
        CreateAvailableChallengeList();

        ////*** Create list of completed challenges in the game
        CreateCompletedChallengeList();

        ////*** Create list of incomplete hub challenges in the game
        CreateHubChallengeList();

        ////*** Create list of Target PRacticechallenges in the game
        CreateTargetPracticeChallengeList();

        ////*** Create list of Target PRacticechallenges in the game
        CreateShootingGalleryChallengeList();

        // Level 1 Challenges
        ////*** Create list of easy challenges in the game
        CreateLevel1EasyChallengeList();

        ////*** Create list of medium challenges in the game
        CreateLevel1MediumChallengeList();

        ////*** Create list of hard challenges in the game
        CreateLevel1HardChallengeList();

        // Level 2 Challenges
        ////*** Create list of easy challenges in the game
        CreateLevel2EasyChallengeList();

        ////*** Create list of medium challenges in the game
        CreateLeve21MediumChallengeList();

        ////*** Create list of hard challenges in the game
        CreateLevel2HardChallengeList();

        // Level 3 Challenges
        ////*** Create list of easy challenges in the game
        CreateLevel3EasyChallengeList();

        ////*** Create list of medium challenges in the game
        CreateLevel3MediumChallengeList();

        ////*** Create list of hard challenges in the game
        CreateLevel3HardChallengeList();

        //*** Refresh stat trackers
        UpdatePlayerStatTrackers();

        PlayerDataController.instance.ClearCompletedChallengeData();

        LevelUnlockChecker oLevelUnlockCheck = GameObject.FindObjectOfType<LevelUnlockChecker>();

        if (oLevelUnlockCheck != null)
        {
            //*** Check if new level was Unlocked
            oLevelUnlockCheck.EvaluateLevelUnlocks();
        }

    }


    public void CreateAvailableChallengeList()
    {
        //*** Created available challenge list
        availableChallengesList = new List<SavedChallengeData>();

        //*** Iterate through level data
        for(int i = 0; i < storedData.levels.Count; i++)
        {

            //*** Check if this level is even available yet
            if (storedData.levels[i].levelAvailability == SavedChallengeData.ChallengeAvailability.Active)
            {

                //*** If level available iterate through challenges for level
                for (int x = 0; x < storedData.levels[i].challengeList.Count; x++)
                {

                    //*** Check if current challenge is even available yet
                    if (storedData.levels[i].challengeList[x].challengeAvailability == SavedChallengeData.ChallengeAvailability.Active)
                    {
                        //*** Create master list of available challenges
                        SavedChallengeData oChallengeData = new SavedChallengeData();

                        oChallengeData.challengeName = storedData.levels[i].challengeList[x].challengeName;
                        oChallengeData.challengeDescription = storedData.levels[i].challengeList[x].challengeDescription;
                        oChallengeData.challengeAvailability = storedData.levels[i].challengeList[x].challengeAvailability;
                        oChallengeData.ChallengeComplete = storedData.levels[i].challengeList[x].ChallengeComplete;
                        oChallengeData.challengeCompletionKey = storedData.levels[i].challengeList[x].challengeCompletionKey;
                        oChallengeData.interactionCount = storedData.levels[i].challengeList[x].interactionCount;
                        oChallengeData.challengeStyle = storedData.levels[i].challengeList[x].challengeStyle;
                        oChallengeData.challengeType = storedData.levels[i].challengeList[x].challengeType;
                        oChallengeData.challengeLifetime = storedData.levels[i].challengeList[x].challengeLifetime;
                        oChallengeData.challengeDifficultyMode = storedData.levels[i].challengeList[x].challengeDifficultyMode;

                        availableChallengesList.Add(oChallengeData);


                    }

                }
            }

        }

    }

    public void CreateCompletedChallengeList()
    {
        //*** Create list of hub challenges where the challenges are incomplete AND they are set as hub challenges AND the challenges are set to active!
        var completedHubChallenges = availableChallengesList.Where(c => c.ChallengeComplete == true
        && c.challengeAvailability == SavedChallengeData.ChallengeAvailability.Active);

        completedChallengesList = completedHubChallenges.ToList<SavedChallengeData>();


    }

    public void CreateHubChallengeList()
    {

        //var smallNumbers = numbers.Where(n => n > 1 && n != 4 &&  n < 10);

        //*** Create list of hub challenges where the challenges are incomplete AND they are set as hub challenges AND the challenges are set to active!
        var hubChallenges = availableChallengesList.Where(c => c.ChallengeComplete == false
        && c.challengeStyle == SavedChallengeData.ChallengeStyle.Hub
        && c.challengeAvailability == SavedChallengeData.ChallengeAvailability.Active);

        hubChallengesList = hubChallenges.ToList<SavedChallengeData>();

    }

    public void CreateTargetPracticeChallengeList()
    {
        //*** Create list of hub challenges where the challenges are incomplete AND they are set as hub challenges AND the challenges are set to active!
        var targetPracticechallenges = availableChallengesList.Where(c => c.ChallengeComplete == false
        && c.challengeStyle == SavedChallengeData.ChallengeStyle.TrainingRoom
        && c.challengeAvailability == SavedChallengeData.ChallengeAvailability.Active);

        targetPracticeChallengeList = targetPracticechallenges.ToList<SavedChallengeData>();

    }

    public void CreateShootingGalleryChallengeList()
    {
        //*** Create list of hub challenges where the challenges are incomplete AND they are set as hub challenges AND the challenges are set to active!
        var shootingGalleryChallenges = availableChallengesList.Where(c => c.ChallengeComplete == false
        && c.challengeStyle == SavedChallengeData.ChallengeStyle.ShootingGallery
        && c.challengeAvailability == SavedChallengeData.ChallengeAvailability.Active);

        shootingGalleryChallengesList = shootingGalleryChallenges.ToList<SavedChallengeData>();

    }

    public void CreateLevel1EasyChallengeList()
    {
        //*** Create list of hub challenges where the challenges are incomplete AND they are set as hub challenges AND the challenges are set to active!
        var Level1EasyChallenges = availableChallengesList.Where(c => c.ChallengeComplete == false
        && c.challengeStyle == SavedChallengeData.ChallengeStyle.level1
        && c.challengeDifficultyMode ==   DifficultySettings.difficulty.easy
        && c.challengeAvailability == SavedChallengeData.ChallengeAvailability.Active);

        EasyLevel1IncompleteChallengeList = Level1EasyChallenges.ToList<SavedChallengeData>();

    }

    public void CreateLevel1MediumChallengeList()
    {
        var Level1MediumChallenges = availableChallengesList.Where(c => c.ChallengeComplete == false
         && c.challengeStyle == SavedChallengeData.ChallengeStyle.level1
         && c.challengeDifficultyMode == DifficultySettings.difficulty.medium
         && c.challengeAvailability == SavedChallengeData.ChallengeAvailability.Active);

        MediumLevel1IncompleteChallengeList = Level1MediumChallenges.ToList<SavedChallengeData>();

    }

    public void CreateLevel1HardChallengeList()
    {
        var Level1HardChallenges = availableChallengesList.Where(c => c.ChallengeComplete == false
          && c.challengeStyle == SavedChallengeData.ChallengeStyle.level1
          && c.challengeDifficultyMode == DifficultySettings.difficulty.hard
          && c.challengeAvailability == SavedChallengeData.ChallengeAvailability.Active);

        HardLevel1IncompleteChallengeList = Level1HardChallenges.ToList<SavedChallengeData>();

    }

    public void CreateLevel2EasyChallengeList()
    {
        //*** Create list of hub challenges where the challenges are incomplete AND they are set as hub challenges AND the challenges are set to active!
        var Leve21EasyChallenges = availableChallengesList.Where(c => c.ChallengeComplete == false
        && c.challengeStyle == SavedChallengeData.ChallengeStyle.level2
        && c.challengeDifficultyMode == DifficultySettings.difficulty.easy
        && c.challengeAvailability == SavedChallengeData.ChallengeAvailability.Active);

        EasyLevel2IncompleteChallengeList = Leve21EasyChallenges.ToList<SavedChallengeData>();

    }

    public void CreateLeve21MediumChallengeList()
    {
        var Level2MediumChallenges = availableChallengesList.Where(c => c.ChallengeComplete == false
         && c.challengeStyle == SavedChallengeData.ChallengeStyle.level2
         && c.challengeDifficultyMode == DifficultySettings.difficulty.medium
         && c.challengeAvailability == SavedChallengeData.ChallengeAvailability.Active);

        MediumLevel2IncompleteChallengeList = Level2MediumChallenges.ToList<SavedChallengeData>();

    }

    public void CreateLevel2HardChallengeList()
    {
        var Level3HardChallenges = availableChallengesList.Where(c => c.ChallengeComplete == false
          && c.challengeStyle == SavedChallengeData.ChallengeStyle.level2
          && c.challengeDifficultyMode == DifficultySettings.difficulty.hard
          && c.challengeAvailability == SavedChallengeData.ChallengeAvailability.Active);

        HardLevel2IncompleteChallengeList = Level3HardChallenges.ToList<SavedChallengeData>();

    }

    public void CreateLevel3EasyChallengeList()
    {
        //*** Create list of hub challenges where the challenges are incomplete AND they are set as hub challenges AND the challenges are set to active!
        var Level3EasyChallenges = availableChallengesList.Where(c => c.ChallengeComplete == false
        && c.challengeStyle == SavedChallengeData.ChallengeStyle.level3
        && c.challengeDifficultyMode == DifficultySettings.difficulty.easy
        && c.challengeAvailability == SavedChallengeData.ChallengeAvailability.Active);

        EasyLevel3IncompleteChallengeList = Level3EasyChallenges.ToList<SavedChallengeData>();

    }

    public void CreateLevel3MediumChallengeList()
    {
        var Level3MediumChallenges = availableChallengesList.Where(c => c.ChallengeComplete == false
         && c.challengeStyle == SavedChallengeData.ChallengeStyle.level3
         && c.challengeDifficultyMode == DifficultySettings.difficulty.medium
         && c.challengeAvailability == SavedChallengeData.ChallengeAvailability.Active);

        MediumLevel3IncompleteChallengeList = Level3MediumChallenges.ToList<SavedChallengeData>();

    }

    public void CreateLevel3HardChallengeList()
    {
        var Level3HardChallenges = availableChallengesList.Where(c => c.ChallengeComplete == false
          && c.challengeStyle == SavedChallengeData.ChallengeStyle.level3
          && c.challengeDifficultyMode == DifficultySettings.difficulty.hard
          && c.challengeAvailability == SavedChallengeData.ChallengeAvailability.Active);

        HardLevel3IncompleteChallengeList = Level3HardChallenges.ToList<SavedChallengeData>();

    }

    public void SaveNewCompletedChallenges(List<SavedChallengeData> pCompletedTargetChallenges, bool pReturnedFromArenaVictorious = false, bool pReturnedFromArenaWithLoss = false)
    {

        if (pReturnedFromArenaWithLoss == false)
        {

            //*** Iterate through completed target challenge lists
            for (int z = 0; z < pCompletedTargetChallenges.Count; z++)
            {
                //*** Iterate through stored data levels
                for (int i = 0; i < storedData.levels.Count; i++)
                {
                    //*** Iterate through levels challenge lists
                    for (int x = 0; x < storedData.levels[i].challengeList.Count; x++)
                    {
                        //*** Does the challengecompletion key match this saved challenge object in the data model ? If so , challenge was completed and set completed flag to true.
                        if (storedData.levels[i].challengeList[x].challengeCompletionKey == pCompletedTargetChallenges[z].challengeCompletionKey)
                        {
                            storedData.levels[i].challengeList[x].ChallengeComplete = true;


                        }

                    }
                }
            }
        }

        if (pReturnedFromArenaVictorious == true || pReturnedFromArenaWithLoss == true)
        {
           
                storedData.enteredFirstCombatArena = true;

            
        }

        SaveData();

        OrganizeData();

    }

    public void SaveCompletedTutorial()
    {
        /*if (storedData.enteredFirstCombatArena == false)
        {
            storedData.enteredFirstCombatArena = true;



            SaveData();

            OrganizeData();
        }*/
    }




    public void AssignRandomChallenges()
    {

        List<int> listNumbers = new List<int>();
        int number;
        for (int i = 0; i < 6; i++)
        {
            do
            {
                number = Random.Range(1, 49);
            } while (listNumbers.Contains(number));
            listNumbers.Add(number);
        }

    }



    public void UpdatePlayerStatTrackers()
    {
        ChallengeViewer oChallengeViewer = GameObject.FindObjectOfType<ChallengeViewer>();

        if (oChallengeViewer != null)
            oChallengeViewer.UpdateChallengeViewer();


       StatsConsole oStatsConsole = GameObject.FindObjectOfType<StatsConsole>();

        if (oStatsConsole != null)
            oStatsConsole.UpdatePlayerStats();



    }



}
