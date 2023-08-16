using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ChallengeViewer : MonoBehaviour
{

    public NewSaveDataTest newSaveDataModel;

    public int challengeIndex;

    public TextMeshPro challengeName;
    public TextMeshPro challengeDescription;
    public GameObject incompleteChallengeTextLabel;
    public GameObject completedChallengeTextLabel;
    public TextMeshPro ChallengeCountTextLabel;
    public TextMeshPro ChallengeTypeTextLabel;


    public Color TargetPracticeGalleryColor;
    public Color DroidDestructionGalleryColor;
    public Color ClubAxillaChallengeColor;
    public Color DesolateDunesChallengeColor;
    public Color PegasusChallengeColor;
    public Color LychRuinsChallengeColor;

    // Start is called before the first frame update
    void Start()
    {
        Invoke("UpdateChallengeViewer", 3f);
    }

   

    // Update is called once per frame
   public void UpdateChallengeViewer()
    {
        newSaveDataModel = GameObject.FindObjectOfType<NewSaveDataTest>();

        RenderChallenge();
    }

    public void RenderChallenge()
    {
        challengeName.text = newSaveDataModel.availableChallengesList[challengeIndex].challengeName;
        challengeDescription.text = newSaveDataModel.availableChallengesList[challengeIndex].challengeDescription;
        ChallengeTypeTextLabel.text = GetChallengeTypeLabelText(newSaveDataModel.availableChallengesList[challengeIndex].challengeStyle);

        if (newSaveDataModel.availableChallengesList[challengeIndex].ChallengeComplete)
        {
            completedChallengeTextLabel.gameObject.SetActive(true);
            incompleteChallengeTextLabel.gameObject.SetActive(false);

        }
        else
        {
            incompleteChallengeTextLabel.gameObject.SetActive(true);
            completedChallengeTextLabel.gameObject.SetActive(false);

        }


        UpdateChallengeIndexLabel();
    }


    public void ViewNextChallenge()
    {
        challengeIndex = challengeIndex + 1;

        if (challengeIndex >= newSaveDataModel.availableChallengesList.Count)
            challengeIndex = 0;


        RenderChallenge();
    }

    public void ViewPReviousChallenge()
    {
        challengeIndex = challengeIndex - 1;

        if (challengeIndex < 0)
            challengeIndex = newSaveDataModel.availableChallengesList.Count - 1;


        RenderChallenge();
    }

    public void ViewNext10Challenge()
    {

        int oCalculatedIndex = 0;

        oCalculatedIndex = challengeIndex + 10;

        if (oCalculatedIndex >= newSaveDataModel.availableChallengesList.Count)
        {

            challengeIndex = oCalculatedIndex - newSaveDataModel.availableChallengesList.Count;

        }
        else
        {

            challengeIndex = oCalculatedIndex;


        }
            


        RenderChallenge();
    }

    public void ViewPRevious10Challenge()
    {
        int oCalculatedIndex = 0;

        oCalculatedIndex = challengeIndex - 10;

        if (oCalculatedIndex < 0)
        {
            challengeIndex = newSaveDataModel.availableChallengesList.Count - oCalculatedIndex;

        }
        else
        {

            challengeIndex = oCalculatedIndex;


        }


        RenderChallenge();
    }


    public void UpdateChallengeIndexLabel()
    {

        ChallengeCountTextLabel.text = @"#"+ challengeIndex.ToString() + "  out of " + newSaveDataModel.availableChallengesList.Count.ToString();


    }

    public string GetChallengeTypeLabelText(SavedChallengeData.ChallengeStyle pStyle)
    {

        string oString = "";

        switch (pStyle)
        {

            case SavedChallengeData.ChallengeStyle.Hub:

                oString = "Pegasus Hub";
                ChallengeTypeTextLabel.color = PegasusChallengeColor;
                break;


            case SavedChallengeData.ChallengeStyle.ShootingGallery:
                oString = "Target Practice Gallery";
                ChallengeTypeTextLabel.color = TargetPracticeGalleryColor;

                break;


            case SavedChallengeData.ChallengeStyle.TrainingRoom:
                oString = "Droid Training Room";
                ChallengeTypeTextLabel.color = DroidDestructionGalleryColor;

                break;

            case SavedChallengeData.ChallengeStyle.level1:
                oString = "Club Axilla (Combat)";
                ChallengeTypeTextLabel.color = ClubAxillaChallengeColor;

                break;

            case SavedChallengeData.ChallengeStyle.level2:
                oString = "Desolate Dunes (Combat)";
                ChallengeTypeTextLabel.color = DesolateDunesChallengeColor;

                break;

            case SavedChallengeData.ChallengeStyle.level3:
                oString = "Lych Ruins (Combat)";
                ChallengeTypeTextLabel.color = LychRuinsChallengeColor;

                break;

            default:
                oString = "None";
                ChallengeTypeTextLabel.color = Color.white;

                break;


        }

        return oString;

    }
}
