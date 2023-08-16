using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AriaNavigationNode : MonoBehaviour
{

    //*** check if node is final tutorial position
    public bool FinalNavigationNode;

    //*** Has AI gotten to current node ?
    public bool AriaHasArrived;

    //*** Has player reached current node?
    public bool playerHasArrived;

    //*** Has AI Begun talking ?
    public bool beganNarration;

    //*** Has AI finished talking ?
    public bool finishedNarration;

    //*** List o clips rleated to this node position
    public List<AudioClip> ArioVoiceOverLines;

    //public List<AriaTutorialData> AriaIntroVoiceOverLines;

    public AudioClip AriaConfirmationVoiceOverLines;

    public AudioClip AriaCancellationVoiceOverLines;

    public GameObject alternativeTarget;

    //*** How long to wait in between delivering audio lines
    public float delayBetweenLines = 1.5f;

    //*** what index of lines are the AI on?
    public int narrationIndex;

    //*** Audio source for AI speech
    public AudioSource narrationAudioSource;

    //*** Still needs work but controls looking at player
    public bool lookAtPlayer;

    //*** AI character tranform to look at player
    public Transform ariaTransform;

    //**** Player transfrorm
    public Transform playerTransform;

    //*** AI intro class holding master node list
    public Aria_Intro_Navigation ariaIntro;

    public List<SavedChallengeData> tutorialCompleteChallengeData;

    public Aria_Interactions aria_Interactions;

    public AriaConfirmationTimer[] interactionTimers;

    public HubChallengeDisplay hubChallengeDisplay;

    public bool decisionWasMade;

    public AriaAlertExclamationPoint exclamationPointManager;

    public HubController hubController;

    // Start is called before the first frame update
    void Start()
    {
        //*** Intro is static this is redundant
        ariaIntro = GameObject.FindObjectOfType<Aria_Intro_Navigation>();

        interactionTimers = GameObject.FindObjectsOfType<AriaConfirmationTimer>();

        //*** Delay before getting player transform to prevent nulls if instantiating
        Invoke("SetPlayerTransformAndHubChallengeMenu", 1.5f);

        
    }

    private void Update()
    {

        //**** Needs work
        if (lookAtPlayer)
         {

            if (playerTransform != null)
                ariaTransform.LookAt(playerTransform);
        }
        else {

        }


    }

    public void SetPlayerTransformAndHubChallengeMenu()
    {
        //GetPlayerTransform();

       // GetHubChallengeRenderer();
    }


    //** Get player transform
    void GetPlayerTransform() {
        playerTransform = GameObject.FindObjectOfType<PlayerDamageController>().gameObject.transform;

        lookAtPlayer = true;
    }

    public void GetHubChallengeRenderer()
    {
        hubChallengeDisplay = GameObject.FindObjectOfType<HubChallengeDisplay>(false);
    }

    //*** Checks for player and AI reaching Node
    private void OnTriggerEnter(Collider other)
    {
        if (beganNarration == false)
        {

            if (other.gameObject.tag == "Aria")
            {
                exclamationPointManager.RevealExclamationPoint();

                GetPlayerTransform();

                AriaHasArrived = true;

                aria_Interactions = other.GetComponentInChildren<Aria_Interactions>();

                AssignInteractionTimersCurrentNode();

                CheckAriaNarration();
            }


            if (other.gameObject.tag == "Player")
            {
                playerHasArrived = true;

               

                CheckAriaNarration();
            }

        }

    }

    private void OnTriggerExit(Collider other)
    {



        if (other.gameObject.tag == "Player")
        {
            hubController.ResetVolumeForAria();

        }

        

    }

    //*** Function to check if / when lines are completed
    public void CheckAriaNarration() {

        //Both player and AI have reached Node
        if (playerHasArrived && AriaHasArrived )
        {
            hubController.LowerVolumeForAria();


            exclamationPointManager.HideExclamationPoint();


            beganNarration = true;

            //*** Look at player while talking
            lookAtPlayer = true;

            //*** If have not complayered voice over
            if (narrationIndex < ArioVoiceOverLines.Count)
            {
                //*** Tell AI intro that AI has reached this node and to stop movement
                if (ariaIntro != null)
                    ariaIntro.AriaArrived();

                //*** Set tutorial audio clip
                narrationAudioSource.clip = ArioVoiceOverLines[narrationIndex];

                //*** Play Audio
                narrationAudioSource.Play();

                //*** Check next line in audio length + delay between lines) seconds
                Invoke("CheckAriaNarration", ArioVoiceOverLines[narrationIndex].length + delayBetweenLines);

                //*** Increment narration index
                narrationIndex++;
            }
            else
            {
                //*** Completed audio
                Debug.Log("Completed Narration!!");
                CompletedTutorialVO_At_Node();

            }

        }
    }


    //*** This saves if tutorial is complete
    public void MarkTutorialComplete() {

        //***If final tutorial node  save tutorial complete
        if (FinalNavigationNode)
        {

            Debug.Log("Tutorial Completed!!!!");

            //*** Mark that Tutorial was completed
            SaveDataManager.instance.currentSaveFile.completedIntroTutorial = true;

            //*** Save file data
            SaveDataManager.instance.SaveCompletedTutorial();
        }
    
    }

    //*** This checks if character has finished talking
    public void CompletedTutorialVO_At_Node() {
        //***Finished lines stp looking at player and move
        lookAtPlayer = false;

        //*** Save completed tutrial if final navigation point
        if (FinalNavigationNode)
        {
            PlayerDataController.instance.newSaveDataModel.SaveNewCompletedChallenges(tutorialCompleteChallengeData);

            hubChallengeDisplay.RenderNewSompletedChallenge(tutorialCompleteChallengeData[0]);

            hubController.ResetVolumeForAria();
        }
        else
        {
            if (aria_Interactions != null)
                aria_Interactions.RenderDecisionMenu();

        }
    }

    public void ConfirmationSelection()
    {
        if (decisionWasMade == false)
        {
            decisionWasMade = true;

            //*** If not final node keep moving along path
            if (ariaIntro != null)
                ariaIntro.GoToNextariaNode();

            if (aria_Interactions != null)
                aria_Interactions.ConfirmInteraction();

            //*** Set tutorial audio clip
            narrationAudioSource.clip = AriaConfirmationVoiceOverLines;

            //*** Play Audio
            narrationAudioSource.Play();

            Invoke("MoveToNextNode", 3f);
        }
    }

    public void CancelSelection()
    {
        if (decisionWasMade == false)
        {

            decisionWasMade = true;

            //*** If not final node keep moving along path
            if (ariaIntro != null)
                ariaIntro.GoToFinalAriaTutorialNode();

            if (aria_Interactions != null)
                aria_Interactions.DeclineInteraction();

            //*** Set tutorial audio clip
            narrationAudioSource.clip = AriaCancellationVoiceOverLines;

            //*** Play Audio
            narrationAudioSource.Play();

            Invoke("MoveToNextNode", 3f);
        }
    }


    public void MoveToNextNode()
    {
        lookAtPlayer = false;
        //*** If not final node keep moving along path
        if (ariaIntro != null)
            ariaIntro.AriaGetMoving();
    }

    public void AssignInteractionTimersCurrentNode()
    {

        for(int i = 0; i < interactionTimers.Length; i++)
        {

            interactionTimers[i].currentNavigationNode = this;


        }


    }
}




