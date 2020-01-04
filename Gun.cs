using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Gun : MonoBehaviour
{
    public Transform firingOrigin;
    public GameObject laserPrefab;

    public bool firedWeapon;

    private OVRGrabbable grabbbleWeapon;


    private const string  grabberLeftName = "AvatarGrabberLeft";
    private const string grabberRightName = "AvatarGrabberRight";

    private bool firstgrabbed;

    // Start is called before the first frame update
    void Start()
    {
        grabbbleWeapon = this.gameObject.GetComponent<OVRGrabbable>();
    }

    // Update is called once per frame
    void Update()
    {
        if (grabbbleWeapon != null)
        {
            if (grabbbleWeapon.isGrabbed)
            {
                grabbbleWeapon.transform.position = grabbbleWeapon.grabbedBy.gameObject.transform.position;
                grabbbleWeapon.transform.rotation = grabbbleWeapon.grabbedBy.gameObject.transform.rotation;
               
                EvaluateTriggerPull();
            }



        }
    }

    public void EvaluateTriggerPull()
    {
        //*** Left Trigger Pull
        //OVRInput.Get(OVRInput.RawButton.LHandTrigger)

        if (grabbbleWeapon.grabbedBy.name == grabberLeftName)
        {
            if (OVRInput.Get(OVRInput.RawButton.LIndexTrigger) == false)
            {
                firedWeapon = false;

            }

            if (OVRInput.Get(OVRInput.RawButton.LIndexTrigger) == true
             && firedWeapon == false)
            {
                firedWeapon = true;
                OVRInput.SetControllerVibration(1f, 1f, OVRInput.Controller.LTouch);
                StopControllerVibration(true, .2f);
                FireLaser();
            }
            /*if (OVRInput.Get(OVRInput.RawButton.LIndexTrigger) == true
                && firedWeapon == true)
            {
                firedWeapon = false;
                FireLaser();
            }*/

        }

        //*** Right Trigger Pull
        if (grabbbleWeapon.grabbedBy.name == grabberRightName)
        {

            if (OVRInput.Get(OVRInput.RawButton.RIndexTrigger) == false)
            {
                firedWeapon = false;
            }

            if (OVRInput.Get(OVRInput.RawButton.RIndexTrigger) == true
            && firedWeapon == false)
            {
                OVRInput.SetControllerVibration(1f, 1f, OVRInput.Controller.RTouch);
                StopControllerVibration(false, .2f);
                firedWeapon = true;
                FireLaser();
            }

            /*if (OVRInput.Get(OVRInput.RawButton.RIndexTrigger) == true && firedWeapon == true)
            {
                firedWeapon = false;
                FireLaser();
            }*/

        }
    }



    public void FireLaser()
    {
        Instantiate(laserPrefab, firingOrigin.position, this.gameObject.transform.rotation);
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
}
