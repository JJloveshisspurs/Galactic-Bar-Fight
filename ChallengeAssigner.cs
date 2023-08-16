using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

public class ChallengeAssigner : MonoBehaviour
{
    public MasterChallengeList challengeList;


    public SavedChallengeData.ChallengeType challengegroup1Type;
    public SavedChallengeData.ChallengeType challengegroup2Type;
    public SavedChallengeData.ChallengeType challengegroup3Type;


    public List<SavedChallengeData> challengepool1;
    public List<SavedChallengeData> challengepool2;
    public List<SavedChallengeData> challengepool3;

    public SavedChallengeData activeChallenge1;
    public SavedChallengeData activeChallenge2;
    public SavedChallengeData activeChallenge3;

    public List<ChallengeRenderer> challengeRenderers;

    public string sceneToLoad;
    public bool beginSceneLoad;

    // Start is called before the first frame update
    void Start()
    {
        //SetChallenges();
    }

    private void Update()
    {
        if(Input.GetKey(KeyCode.D) && beginSceneLoad == false) {

            beginSceneLoad = true;

            StartCoroutine(LoadYourAsyncSceneTest());
        }
    }

    public void SetChallenges()
    {
        Invoke("SetChallenge1", 0f);

        Invoke("SetChallenge2", 0.3f);

        Invoke("SetChallenge3", 0.3f);

        Invoke("GetNewChallenges",2f);
    }


    public void GetChallenges()
    {
        GetChallenge1();
        GetChallenge2();
        GetChallenge3();
    }

    public void SetChallenge1()
    {
        var result = from s in challengeList.challegeData
                     where s.ChallengeComplete == false && s.challengeType == challengegroup1Type
                     select s;

        challengepool1 = result.ToList<SavedChallengeData>();

       

    }

    public void GetChallenge1()
    {
        int oRand = Random.Range(0, challengepool1.Count);

        activeChallenge1 = challengepool1[oRand];


    }

    public void SetChallenge2()
    {
        var result = from s in challengeList.challegeData
                     where s.ChallengeComplete == false && s.challengeType == challengegroup2Type
                     select s;

        challengepool2 = result.ToList<SavedChallengeData>();



    }

    public void GetChallenge2()
    {

        int oRand = Random.Range(0, challengepool2.Count);

        activeChallenge2 = challengepool2[oRand];


    }


    public void SetChallenge3()
    {
        var result = from s in challengeList.challegeData
                     where s.ChallengeComplete == false && s.challengeType == challengegroup3Type
                     select s;

        challengepool3 = result.ToList<SavedChallengeData>();

       

    }

    public void GetChallenge3()
    {
        int oRand = Random.Range(0, challengepool3.Count);

        activeChallenge3 = challengepool3[oRand];


    }

    public void GetNewChallenges()
    {
        GetChallenge1();
        GetChallenge2();
        GetChallenge3();


        challengeRenderers[0].SetChallenge(activeChallenge1);
        challengeRenderers[1].SetChallenge(activeChallenge2);
        challengeRenderers[2].SetChallenge(activeChallenge3);

        //*** Set random challenge data on layer data controller
        SetRandomChallengedata();
    }

    public void SetRandomChallengedata() {

        Debug.Log("Setting player challenge data");

        if(PlayerDataController.instance != null) {
            Debug.Log("Player Data controller is NOT NULL!!!!");

            PlayerDataController.instance.SetActiveChallengeData(activeChallenge1);
            PlayerDataController.instance.SetActiveChallengeData(activeChallenge2);
            PlayerDataController.instance.SetActiveChallengeData(activeChallenge3);

        }

    }

    IEnumerator LoadYourAsyncSceneTest()
    {
        // The Application loads the Scene in the background as the current Scene runs.
        // This is particularly good for creating loading screens.
        // You could also load the Scene by using sceneBuildIndex. In this case Scene2 has
        // a sceneBuildIndex of 1 as shown in Build Settings.

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneToLoad);

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
}


