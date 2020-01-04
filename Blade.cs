using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BzKovSoft.ObjectSlicerSamples;


public class Blade : MonoBehaviour
{
    public OVRGrabbable grabbableWeapon;
    public bool sliceInProgress;
    public BzKnife bladeScript;



    private const string grabberLeftName = "AvatarGrabberLeft";
    private const string grabberRightName = "AvatarGrabberRight";


    private float leftBladeVelocity = 0f;
    private float rightBladeVelocity = 0f;

    private float bladeVelocitiychecTimer;
    private float bladeVelocitycheckInterval = .2f;

    public GameObject activatedParticle;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (grabbableWeapon != null)
        {
            if (grabbableWeapon.isGrabbed )
            {
                grabbableWeapon.transform.position = grabbableWeapon.grabbedBy.gameObject.transform.position;
                grabbableWeapon.transform.rotation = grabbableWeapon.grabbedBy.gameObject.transform.rotation;

                EvaluateControllerSwing();

                SetActivationParticle(true);
            }
            else
            {
                SetActivationParticle(false);

            }
        }

        bladeVelocitiychecTimer += Time.deltaTime;


        if (bladeVelocitiychecTimer > bladeVelocitycheckInterval)
        {
            bladeVelocitiychecTimer = 0f;
            GameController.instance.bladeTextLabel.text = " left Blade == " + leftBladeVelocity.ToString() + " right blade == " + rightBladeVelocity.ToString();

        }
   }

    public void EvaluateControllerSwing()
    {
        //**** Left Controller
        Debug.Log("Checking controller Swing !!!!!");
        if (grabbableWeapon.grabbedBy.name == grabberLeftName)
        {
            leftBladeVelocity = OVRInput.GetLocalControllerAngularVelocity(OVRInput.Controller.LTouch).magnitude;


            if (leftBladeVelocity <= 1f && sliceInProgress == true)
            {
                sliceInProgress = false;

            }

            if (leftBladeVelocity >= 2f && sliceInProgress == false)
            {
                sliceInProgress = true;
                OVRInput.SetControllerVibration(1f, 1f, OVRInput.Controller.LTouch);
                StopControllerVibration(true, .05f);
                bladeScript.BeginNewSlice();

            }
           

        }

        //**** Right Controller
        if (grabbableWeapon.grabbedBy.name == grabberRightName)
        {
            rightBladeVelocity = OVRInput.GetLocalControllerAngularVelocity(OVRInput.Controller.RTouch).magnitude;

            if (rightBladeVelocity <= 1f && sliceInProgress == true)
            {
                sliceInProgress = false;

            }

            if (rightBladeVelocity >= 2f && sliceInProgress == false)
            {
                sliceInProgress = true;
                OVRInput.SetControllerVibration(1f, 1f, OVRInput.Controller.RTouch);
                StopControllerVibration(true, .05f);
                bladeScript.BeginNewSlice();

            }
        }


    }

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

    public void SetActivationParticle( bool pActivateParticle)
    {
        if (activatedParticle != null)
        {
            if (pActivateParticle)
            {
                if (activatedParticle.active == false)
                    activatedParticle.SetActive(true);

            }
            else
            {
                if (activatedParticle.active == true)
                    activatedParticle.SetActive(false);

            }
        }
    }
}
