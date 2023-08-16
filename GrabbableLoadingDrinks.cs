using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabbableLoadingDrinks : MonoBehaviour
{
    public bool loadingDrinkConsumed;

    //*** Grabbable object properties
    private OVRGrabbable grabbableLoadingDrink;


    private const string grabberLeftName = "DistanceGrabHandLeft";
    private const string grabberRightName = "DistanceGrabHandRight";

    private bool firstgrabbed;








    private float firingRateTimer;
    private float firingRate = .1f;

    private float YRotation;
    private Vector3 YRotAngle;

    private bool rightHandGrab;

    private Vector3 LeftGrabRotation;

    public DrinkLoader loader;

    // Start is called before the first frame update
    void Start()
    {


        grabbableLoadingDrink = this.gameObject.GetComponent<OVRGrabbable>();

        //*** Set gun data with brief delay based on weapons database parameters
        Invoke("InitializePowerupType", .15f);

    }

    public void InitializePowerupType()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (grabbableLoadingDrink != null)
        {
            if (grabbableLoadingDrink.isGrabbed)
            {

                grabbableLoadingDrink.transform.position = grabbableLoadingDrink.grabbedBy.gameObject.transform.position;


                //grabbbleWeapon.transform.localEulerAngles = defaultGrabRotation;

                //*** Check if Right hand grabbed  blaster
                rightHandGrab = (grabbableLoadingDrink.grabbedBy.gameObject.name == grabberRightName);


                if (rightHandGrab)
                {


                    grabbableLoadingDrink.transform.rotation = grabbableLoadingDrink.grabbedBy.gameObject.transform.rotation;
                }
                else
                {
                    /*LeftGrabRotation = new Quaternion(
                    grabbbleWeapon.grabbedBy.gameObject.transform.rotation.x,
                     grabbbleWeapon.grabbedBy.gameObject.transform.rotation.y,
                      grabbbleWeapon.grabbedBy.gameObject.transform.rotation.z * -1,
                       grabbbleWeapon.grabbedBy.gameObject.transform.rotation.w);*/

                    LeftGrabRotation = new Vector3(
                    grabbableLoadingDrink.grabbedBy.gameObject.transform.eulerAngles.x,
                     grabbableLoadingDrink.grabbedBy.gameObject.transform.eulerAngles.y,
                     grabbableLoadingDrink.grabbedBy.gameObject.transform.eulerAngles.z + 180
                    );

                    grabbableLoadingDrink.transform.eulerAngles = LeftGrabRotation;


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
            if (grabbableLoadingDrink.grabbedBy.name == grabberLeftName)
            {
                if (OVRInput.Get(OVRInput.RawButton.LIndexTrigger) == false)
                {
                    //*** Weapon not fired
                    loadingDrinkConsumed = false;


                    StopAllCoroutines();
                }

                //*** Fire Left hand weapon
                if (OVRInput.Get(OVRInput.RawButton.LIndexTrigger) == true && loadingDrinkConsumed == false)
                {
                    loadingDrinkConsumed = true;
                    OVRInput.SetControllerVibration(1f, 1f, OVRInput.Controller.LTouch);
                    StopControllerVibration(true, .2f);
                    BeginFileLoad();
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
            if (grabbableLoadingDrink.grabbedBy.name == grabberRightName)
            {

                if (OVRInput.Get(OVRInput.RawButton.RIndexTrigger) == false)
                {
                    //*** Weapon not fired
                    loadingDrinkConsumed = false;

                    StopAllCoroutines();
                    //StopCoroutine(FireNextShell(0f));
                }

                //*** Fire right hand weapon 
                if (OVRInput.Get(OVRInput.RawButton.RIndexTrigger) == true && loadingDrinkConsumed == false)
                {
                    OVRInput.SetControllerVibration(1f, 1f, OVRInput.Controller.RTouch);
                    StopControllerVibration(false, .2f);
                    loadingDrinkConsumed = true;
                    BeginFileLoad();
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

    public void BeginFileLoad()
    {
       

        loader.SetSavefile();
        grabbableLoadingDrink.GrabEnd(Vector3.zero, Vector3.zero);

        grabbableLoadingDrink.enabled = false;
        Debug.Log("Drinking Powerup");

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
