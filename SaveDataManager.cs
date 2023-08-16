using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class SaveDataManager : MonoBehaviour
{

    public static SaveDataManager instance;

    public const string SAVE_DATA_MODEL_PLAYERPREFS_KEY = "GBFSaveData";

    public SaveFilesContainer saveFilesContainer;

    public SaveFile currentSaveFile;


    /// <summary>
    /// *** Save data holding string values
    /// </summary>
    /// 
    /// 
    ///

    [SerializeField]
    public string  serializedDataString;

    public bool deletePlayerPrefs;

    // Start is called before the first frame update
    void Start()
    {
        //*** Initialize if this is the first instance and mark to not destroy
        if (instance == null)
        {
            instance = this;

            //*** Mark player data controller as do not destroy on load
           UnityEngine.Object.DontDestroyOnLoad(this);
            
        }
        else //*** Else destroy this gamebject and script
        {
            Destroy(this.gameObject);

        }

        if (deletePlayerPrefs)
        {
            Debug.Log("DELETING PLAYER PREFS!!!!");
            PlayerPrefs.DeleteAll();

        }
        else
        {
            Invoke("InitSaveDataManager", 2f);
        }

    }

    public void InitSaveDataManager() {

        Debug.Log("Initializing Save Data");


            ///*** Check if save data model exists, if so load it 
            if (PlayerPrefs.HasKey(SAVE_DATA_MODEL_PLAYERPREFS_KEY))
            {
                Debug.Log("Save Data Exists!!!");
                LoadSaveFiles();
            }
            else
            {
                Debug.Log("No Save Data Exists, creating new save file!!!");

                //*** If no save data exists create  it
                CreateNewSavefile();

            }

    }



    public void CreateNewSavefile()
    {

        //*** Grabbed Master challenge Data list
        MasterChallengeList oChallengeList = GameObject.FindObjectOfType<MasterChallengeList>();

        //*** Initialize save file ocntainer
        saveFilesContainer = new SaveFilesContainer();

        //*** Initialize save files list
        saveFilesContainer.saveFiles = new List<SaveFile>();


        //*** see if playerprefs string exists and holds any data
        string oString = PlayerPrefs.GetString(SAVE_DATA_MODEL_PLAYERPREFS_KEY, "");


        //*** Initialize base save file
        currentSaveFile = new SaveFile();


          

            //*** Create save 1 ----
            SaveFile oSaveFile = new SaveFile();

            //*** Create Save 1

            //*** Create new save entry
            oSaveFile = new SaveFile();

            //*** Name Default Save File
            oSaveFile.savefileName = "Save1";

            //*** Initialize challenge data list
            oSaveFile.challengeList = new List<SavedChallengeData>();

            //*** Set Default time
           // oSaveFile.lastUpdateTime = System.DateTime.Today.ToShortDateString() + " , " + System.DateTime.Today.ToShortTimeString();


            Debug.LogError("Created Save 1 !");

        /*
            //*** Create Save 2 ----
            SaveFile oSaveFile2 = new SaveFile();

            //*** Name Default Save File
            oSaveFile2.savefileName = "Save2";

            //*** Initialize challenge data list
            oSaveFile2.challengeList = new List<SavedChallengeData>();

            //*** Set Default time
            //oSaveFile2.lastUpdateTime = System.DateTime.Today.ToShortDateString() + " , " + System.DateTime.Today.ToShortTimeString();

          
          
            Debug.LogError("Created Save 2");

            //*** Create Save 3 ---
            SaveFile oSaveFile3 = new SaveFile();

            //*** Name Default Save File
            oSaveFile3.savefileName = "Save3";

            //*** Initialize challenge data list
            oSaveFile3.challengeList = new List<SavedChallengeData>();

            //*** Set Default time
            //oSaveFile3.lastUpdateTime = System.DateTime.Today.ToShortDateString() + " , " + System.DateTime.Today.ToShortTimeString();

            //*** Iterate through the list and add all existing challenges
            for (int i = 0; i < oChallengeList.challegeData.Count; i++)
            {
                //*** Add challenge list
                oSaveFile.challengeList.Add(oChallengeList.challegeData[i]);

                //*** Add challenge list
                oSaveFile2.challengeList.Add(oChallengeList.challegeData[i]);

                //*** Add challenge list
                oSaveFile3.challengeList.Add(oChallengeList.challegeData[i]);
            }

            //*** Add save files to save data object
            saveFilesContainer.saveFiles.Add(oSaveFile);

            //*** Add save files to save data object
            saveFilesContainer.saveFiles.Add(oSaveFile2);

            //*** Add save files to save data object
            saveFilesContainer.saveFiles.Add(oSaveFile3);

            Debug.LogError("Created Save 3");
            */

        oSaveFile.InitializeNewChallengeData(oChallengeList.challegeData);

        saveFilesContainer.saveFiles.Add(oSaveFile);
      
        //*** Serialize data into one large data string
        serializedDataString = JsonUtility.ToJson(saveFilesContainer);

        //*** Save to constant player prefs key
        PlayerPrefs.SetString(SAVE_DATA_MODEL_PLAYERPREFS_KEY, serializedDataString);

        //*** Render Existing save file data
        RenderFileInfo();
    }

    public void CreateNewTestSavefile()
    {


        saveFilesContainer  = new SaveFilesContainer();

        //*** Initialize save files list
        saveFilesContainer.saveFiles = new List<SaveFile>();

        SaveFile oSaveFile = new SaveFile();

        oSaveFile.savefileName = "Save1";

        oSaveFile.lifeTimeKills = UnityEngine.Random.Range(1, 3000);

        oSaveFile.lifeTimeScore = UnityEngine.Random.Range(1, 3000);

        oSaveFile.powerupDrinksConsumed = UnityEngine.Random.Range(1, 3000);

        oSaveFile.lastUpdateTime = System.DateTime.Today.ToShortDateString() + " , " + System.DateTime.Today.ToShortTimeString();

        //*** Initialize challenge data list
        oSaveFile.challengeList = new List<SavedChallengeData>();

        //*** Grabbed Master challenge Data list
        MasterChallengeList oChallengeList = GameObject.FindObjectOfType<MasterChallengeList>();

        //*** Iterate throug hthe list and add all existing challenges
        for(int i = 0; i < oChallengeList.challegeData.Count; i++) {

            //*** Add challenge list
            oSaveFile.challengeList.Add(oChallengeList.challegeData[i]);
        }

        //*** Add Save file to the list
        saveFilesContainer.saveFiles.Add(oSaveFile);


        //*** Serialize data into one large data string
        serializedDataString = JsonUtility.ToJson(saveFilesContainer);

        //*** Save to constant player prefs key
        PlayerPrefs.SetString(SAVE_DATA_MODEL_PLAYERPREFS_KEY, serializedDataString);
    }

    public void LoadSaveFiles(int pSetActiveSaveFile = 0)
    {

        string oString = PlayerPrefs.GetString(SAVE_DATA_MODEL_PLAYERPREFS_KEY, "");

        saveFilesContainer = JsonUtility.FromJson<SaveFilesContainer>(oString);


        SetSaveFileData(pSetActiveSaveFile);

        //*** Render Existing save file data
        RenderFileInfo();
    }

    public void SaveCompletedTutorial() 
    {
        saveFilesContainer.saveFiles[0].completedIntroTutorial = true;

        //*** Serialize data into one large data string
        serializedDataString = JsonUtility.ToJson(saveFilesContainer);

        //*** Save to constant player prefs key
        PlayerPrefs.SetString(SAVE_DATA_MODEL_PLAYERPREFS_KEY, serializedDataString);



    }




    public void SetSaveFileData( int pSaveFileIndex)
    {

        //*** Set Current Save File
        currentSaveFile = saveFilesContainer.saveFiles[pSaveFileIndex];

        PlayerDataController.instance.ActiveSaveFileIndex = pSaveFileIndex;


    }


    public void GetandUpdateSaveFileData(int pFileIndex, CombatMetrics pMetricsContainer, List<SavedChallengeData> pCompletedChalengeData)
    {

        Debug.Log("Getting and saving file data");

        string oString = PlayerPrefs.GetString(SAVE_DATA_MODEL_PLAYERPREFS_KEY, "");

        saveFilesContainer = JsonUtility.FromJson<SaveFilesContainer>(oString);


        currentSaveFile = saveFilesContainer.saveFiles[pFileIndex];

        UpdateNewSaveFile(pMetricsContainer, pCompletedChalengeData);
    }

    public void UpdateNewSaveFile(CombatMetrics pMetricsContainer, List<SavedChallengeData> pCompletedChalengeData)
    {
        currentSaveFile.lifeTimeKills  = currentSaveFile.lifeTimeKills + pMetricsContainer.roundTotalKills;
        currentSaveFile.lifeTimeScore = currentSaveFile.lifeTimeScore + pMetricsContainer.roundTotalScore;
        currentSaveFile.powerupDrinksConsumed = currentSaveFile.powerupDrinksConsumed + pMetricsContainer.roundTotalDrinksConsumed;

        //*** Create update string to detail save file  save time
        currentSaveFile.lastUpdateTime = System.DateTime.Today.ToShortDateString() + " , " + System.DateTime.Today.ToShortTimeString();

        //*** update most recent complete challenges
        for(int i = 0; i < pCompletedChalengeData.Count; i++)
        {
            for ( int x = 0; x < currentSaveFile.challengeList.Count; x++)
            {

                if(pCompletedChalengeData[i].challengeName == currentSaveFile.challengeList[x].challengeName)
                {
                    //*** Mark challenge as completed
                    currentSaveFile.challengeList[x].ChallengeComplete = true;

                }

            }

        }
       
        //*** Update existing save file data
        for (int i = 0; i < saveFilesContainer.saveFiles.Count; i++)
        {
            if (saveFilesContainer.saveFiles[i].savefileName == currentSaveFile.savefileName)
            {
                saveFilesContainer.saveFiles[i] = currentSaveFile;

            }
        }

        //*** Serialize data into one large data string
        serializedDataString = JsonUtility.ToJson(saveFilesContainer);

        //*** Save to constant player prefs key
        PlayerPrefs.SetString(SAVE_DATA_MODEL_PLAYERPREFS_KEY, serializedDataString);



    }

    public void RenderFileInfo() {

        SaveDataInfoRenderer saveInfoRenderer = GameObject.FindObjectOfType<SaveDataInfoRenderer>();

        if (saveInfoRenderer != null)
        {

            Debug.LogError("Rendering Save file info!!!");
            saveInfoRenderer.RenderSaveDataInfo();

        }

    }



}
