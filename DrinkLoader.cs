using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;



public class DrinkLoader : MonoBehaviour
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

    public MeshExploder[] meshExploders;

    public MeshRenderer[] meshRenderers;

    public GameObject highLightParticle;

    //*** PLAYER PREFS KEY
    private string FirstIntroKey = "FIRST_INTRO";

    private NewSaveDataTest saveDataManager;

    public void Update()
    {
        if (Input.GetKeyDown(inputTestKey) && sceneLoadInitialized == false)
        {
            sceneLoadInitialized = true;

            SetSavefile();
        }

        if (highLightParticle != null)
        {
            if (highLightParticle.active)
                highLightParticle.SetActive(false);
        }
    }

    public void SetSavefile()
    {
        //SaveDataManager.instance.SetSaveFileData(saveFileIndex);

        SetFirstSceneToLoad();

        PlayerFadeController.instance.ActivateFader();

        // Debug.Log("Activating powerups !!!!");
        for (int i = 0; i < meshExploders.Length; i++)
        {
            //Debug.Log("Powerup index == " + i.ToString());
            meshExploders[i].Explode();
            meshRenderers[i].enabled = false;
            //meshExploders[i].gameObject.SetActive(false);

        }

        AudioManager.instance.PlayBottleShatterAudio();


        Invoke("BeginAsyncSceneLoad", 3f);
    }

    public void SetFirstSceneToLoad()
    {
        //*** If Completed Intro Tour
        /*if (PlayerPrefs.HasKey(FirstIntroKey) == true)
        {

            SceneToLoad = HubSceneName;

        }
        else //*** Else if fresh save file show beginning intro area
        {

            SceneToLoad = IntroSceneName;

        }*/


        saveDataManager = GameObject.FindObjectOfType<NewSaveDataTest>();


        if (saveDataManager != null)
        {

            //*** Has nor entered the combat arena load intro scene
            if (saveDataManager.storedData.enteredFirstCombatArena == false)
            {
                SceneToLoad = IntroSceneName;

            }
            else //*** Has entered the combat arena before, loading into player bedroom.
            {

                SceneToLoad = HubSceneName;

            }
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
