using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabbableMuteButton : MonoBehaviour
{
    public bool MuteButtonActivated;

    //*** Grabbable object properties
    private OVRGrabbable grabbablePowerup;

    private const string grabberLeftName = "DistanceGrabHandLeft";
    private const string grabberRightName = "DistanceGrabHandRight";

    private bool firstgrabbed;

    private float firingRateTimer;
    private float firingRate = .1f;

    private float YRotation;
    private Vector3 YRotAngle;

    private bool rightHandGrab;

    private Vector3 LeftGrabRotation;

   

    public GameObject highLightParticle;


    public List<AudioSource> additionalAudioSources;

    // Start is called before the first frame update
    void Start()
    {
        grabbablePowerup = this.gameObject.GetComponent<OVRGrabbable>();

        //*** Set gun data with brief delay based on weapons database parameters
        Invoke("InitializePowerupType", .15f);
    }

    public void InitializePowerupType()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (grabbablePowerup != null)
        {
            if (grabbablePowerup.isGrabbed)
            {

                if (highLightParticle != null)
                {
                    if (highLightParticle.active)
                        highLightParticle.SetActive(false);
                }

                grabbablePowerup.transform.position = grabbablePowerup.grabbedBy.gameObject.transform.position;

                // Change the scale of the game object grabbed to appear more to scale with the player.
                gameObject.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);


                //grabbbleWeapon.transform.localEulerAngles = defaultGrabRotation;

                //*** Check if Right hand grabbed  blaster
                rightHandGrab = (grabbablePowerup.grabbedBy.gameObject.name == grabberRightName);


                if (rightHandGrab)
                {
                    grabbablePowerup.transform.rotation = grabbablePowerup.grabbedBy.gameObject.transform.rotation;
                }
                else
                {
                    /*LeftGrabRotation = new Quaternion(
                    grabbbleWeapon.grabbedBy.gameObject.transform.rotation.x,
                     grabbbleWeapon.grabbedBy.gameObject.transform.rotation.y,
                      grabbbleWeapon.grabbedBy.gameObject.transform.rotation.z * -1,
                       grabbbleWeapon.grabbedBy.gameObject.transform.rotation.w);*/

                    LeftGrabRotation = new Vector3(
                            grabbablePowerup.grabbedBy.gameObject.transform.eulerAngles.x,
                            grabbablePowerup.grabbedBy.gameObject.transform.eulerAngles.y,
                            grabbablePowerup.grabbedBy.gameObject.transform.eulerAngles.z + 180);

                    grabbablePowerup.transform.eulerAngles = LeftGrabRotation;
                }

                /* grabbbleWeapon.transform.position = grabbbleWeapon.grabbedBy.gameObject.transform.position;
                 grabbbleWeapon.transform.rotation = grabbbleWeapon.grabbedBy.gameObject.transform.rotation;*/

                EvaluateTriggerPull(rightHandGrab);
            }
        }
    }

    public void EvaluateTriggerPull(bool pIsrightHand)
    {
        //*** Left Trigger Pull
        //OVRInput.Get(OVRInput.RawButton.LHandTrigger)

        if (pIsrightHand == false)
        {
            if (grabbablePowerup.grabbedBy.name == grabberLeftName)
            {
                if (OVRInput.Get(OVRInput.RawButton.LIndexTrigger) == false)
                {
                    //*** Weapon not fired
                    MuteButtonActivated = false;


                    StopAllCoroutines();
                }

                //*** Fire Left hand weapon
                if (OVRInput.Get(OVRInput.RawButton.LIndexTrigger) == true && MuteButtonActivated == false)
                {
                    MuteButtonActivated = true;
                    OVRInput.SetControllerVibration(1f, 1f, OVRInput.Controller.LTouch);
                    StopControllerVibration(true, .2f);
                    ActivateMutebutton();
                }
                /*if (OVRInput.Get(OVRInput.RawButton.LIndexTrigger) == true
                && firedWeapon == true)
            {
                firedWeapon = false;
                FireLaser();
            }*/

            }
        }
        else
        {

            //*** Right Trigger Pull
            if (grabbablePowerup.grabbedBy.name == grabberRightName)
            {

                if (OVRInput.Get(OVRInput.RawButton.RIndexTrigger) == false)
                {
                    //*** Weapon not fired
                    MuteButtonActivated = false;

                    StopAllCoroutines();
                    //StopCoroutine(FireNextShell(0f));
                }

                //*** Fire right hand weapon 
                if (OVRInput.Get(OVRInput.RawButton.RIndexTrigger) == true && MuteButtonActivated == false)
                {
                    OVRInput.SetControllerVibration(1f, 1f, OVRInput.Controller.RTouch);
                    StopControllerVibration(false, .2f);
                    MuteButtonActivated = true;
                    ActivateMutebutton();
                }

                /*if (OVRInput.Get(OVRInput.RawButton.RIndexTrigger) == true && firedWeapon == true)
            {
                firedWeapon = false;
                FireLaser();
            }*/

            }
        }
    }

    //*** Weapon fire Functions 

    public void ActivateMutebutton()
    {
        AudioManager.instance.PlayAudio(AudioData.audioClipType.Confirm_Click,this.gameObject.transform,0f,1f);

        //*** Increment Kill count
        AudioManager.instance.MuteAllMusicObjects();

        for(int i = 0; i < additionalAudioSources.Count; i++)
        {

            additionalAudioSources[i].volume = 0f;


        }

    }

    //**** Weapon Vibration functions 

    public void StopControllerVibration(bool pIsLeftcontroller, float ptimeDelay = 0f)
    {
        if (pIsLeftcontroller)
        {
            Invoke("StopControllerVibrationLeft", ptimeDelay);

        }
        else
        {
            Invoke("StopControllerVibrationRight", ptimeDelay);

        }


    }


    public void StopControllerVibrationLeft()
    {
        OVRInput.SetControllerVibration(0f, 0f, OVRInput.Controller.LTouch);

    }
    public void StopControllerVibrationRight()
    {
        OVRInput.SetControllerVibration(0f, 0f, OVRInput.Controller.RTouch);

    }
}
