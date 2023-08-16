using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class TestLoadingDrinks : MonoBehaviour
{
    //***Name of Save File drink
    public string drinkName;

    //*** Key for input test key values
    public KeyCode inputTestKey;

    //*** Name of Scene to load
    public string SceneToLoad;

    public bool sceneLoadInitialized;

    public string HubSceneName;
    public string IntroSceneName;

    public int saveFileIndex;

    public void Update()
    {
        if (Input.GetKeyDown(inputTestKey) && sceneLoadInitialized == false)
        {
            sceneLoadInitialized = true;

            SetSavefile();
        }


    }

    public void SetSavefile()
    {
        SaveDataManager.instance.SetSaveFileData(saveFileIndex);

        SetFirstSceneToLoad();

        Invoke("BeginAsyncSceneLoad", .5f);
    }

    public void SetFirstSceneToLoad()
    {
        //*** If Completed Intro Tour
        if (SaveDataManager.instance.currentSaveFile.completedIntroTutorial)
        {

            SceneToLoad = HubSceneName;

        }
        else //*** Else if fresh save file show beginning intro area
        {

            SceneToLoad = IntroSceneName;

        }


    }

    public void BeginAsyncSceneLoad()
    {


        StartCoroutine(LoadAsyncScene());
    }


    IEnumerator LoadAsyncScene()
    {
        // The Application loads the Scene in the background as the current Scene runs.
        // This is particularly good for creating loading screens.
        // You could also load the Scene by using sceneBuildIndex. In this case Scene2 has
        // a sceneBuildIndex of 1 as shown in Build Settings.

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(SceneToLoad);

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
}
