using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ChallengesListUI : MonoBehaviour
{
    [SerializeField] ChallengeItemUI challengePrefab;
    ChallengeList challengeList;

    // Start is called before the first frame update
    void Start()
    {
        challengeList = GameObject.FindGameObjectWithTag("Player").GetComponent<ChallengeList>();
        challengeList.onUpdate += Redraw;

        Redraw();
    }

    private void Redraw()
    {
        //transform.DetachChildren();

        foreach (ChallengeStatus status in challengeList.GetStatuses())
        {
            ChallengeItemUI uiInstance = Instantiate<ChallengeItemUI>(challengePrefab, transform);
            float tempFloat = uiInstance.transform.position.y;

            //int numberOfClassObjects = FindObjectsOfType(typeof(ChallengeItemUI)).Length;

            GameObject[] tempChallengeObject = GameObject.FindGameObjectsWithTag("Challenge");
            
            if(tempChallengeObject.Length > 1)
            {
                // Checks to see if there are 3 challenges present then turns all the additional ones off
                if(tempChallengeObject.Length > 3)
                {
                    for (int i = 0; i < tempChallengeObject.Length; i++)
                    {
                        tempChallengeObject[3].SetActive(false);
                    }
                }

                // Assigns the location of the challenges
                for(int i = 1; i < tempChallengeObject.Length; i++)
                {
                    tempChallengeObject[i].transform.position = new Vector3(uiInstance.transform.position.x, tempFloat - 1.0f, uiInstance.transform.position.z);

                    tempFloat = tempChallengeObject[i].transform.position.y;
                }

                //uiInstance.transform.position = new Vector3(uiInstance.transform.position.x, uiInstance.transform.position.y - 1.0f, uiInstance.transform.position.z);
            }

            uiInstance.Setup(status);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
