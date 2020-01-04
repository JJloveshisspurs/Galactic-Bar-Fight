using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour
{
     public enum grenadeState
    {
        Inactive,
        Active,
        Exploding
    }

    public grenadeState currentGrenadeState;

    public float explosionDelay;

    private OVRGrabbable grabbbleExplosive;


    private const string  grabberLeftName = "AvatarGrabberLeft";
    private const string grabberRightName = "AvatarGrabberRight";

    public GameObject explosionObject;

    public AudioClip GrenadeActivate;
    public AudioClip grenadeArming;

    public AudioClip grenadeExploding;

    public AudioSource grenadeSource;

    public Color grenadeInactiveColor;

    public Color grenadeActiveColor;

    public MeshRenderer grenademesh;

    public GameObject bombMesh;

    public Rigidbody bombRigidbody;

    public float explosionColliderActivationDelay;

    public GameObject explosionCollider;

    // Start is called before the first frame update
    void Start()
    {
        grabbbleExplosive = this.gameObject.GetComponent<OVRGrabbable>();

        grenademesh.materials[1].color = grenadeInactiveColor;
    }

    // Update is called once per frame
    void Update()
    {
        if (grabbbleExplosive != null)
        {
            if (grabbbleExplosive.isGrabbed)
            {
                grabbbleExplosive.transform.position = grabbbleExplosive.grabbedBy.gameObject.transform.position;
                grabbbleExplosive.transform.rotation = grabbbleExplosive.grabbedBy.gameObject.transform.rotation;

                if(currentGrenadeState == grenadeState.Inactive)
                ActivateGrenade();
            }

        }
    }

    public void ActivateGrenade()
    {
        //*** Left Trigger Pull
        //OVRInput.Get(OVRInput.RawButton.LHandTrigger)

        if (grabbbleExplosive.grabbedBy.name == grabberLeftName)
        {

            if (OVRInput.Get(OVRInput.RawButton.LIndexTrigger) == true && currentGrenadeState == grenadeState.Inactive)
            {
                currentGrenadeState = grenadeState.Active;
                Armgrenade();
                OVRInput.SetControllerVibration(1f, .7f, OVRInput.Controller.LTouch);
                Invoke("BeginExplosion", explosionDelay);
                StopControllerVibration(true, .1f);

            }
            /*if (OVRInput.Get(OVRInput.RawButton.LIndexTrigger) == true
                && firedWeapon == true)
            {
                firedWeapon = false;
                FireLaser();
            }*/

        }

        //*** Right Trigger Pull
        if (grabbbleExplosive.grabbedBy.name == grabberRightName)
        {
         
        if (OVRInput.Get(OVRInput.RawButton.RIndexTrigger) == true && currentGrenadeState == grenadeState.Inactive)
            {
                currentGrenadeState = grenadeState.Active;
                Armgrenade();
                OVRInput.SetControllerVibration(1f, .7f, OVRInput.Controller.RTouch);
                Invoke("BeginExplosion", explosionDelay);
                StopControllerVibration(false, .1f);
            }

            /*if (OVRInput.Get(OVRInput.RawButton.RIndexTrigger) == true && firedWeapon == true)
            {
                firedWeapon = false;
                FireLaser();
            }*/

        }
    }

    public void Armgrenade()
    {
        grenademesh.materials[1].color = grenadeActiveColor;
        grenadeSource.PlayOneShot(GrenadeActivate);
        Invoke("PlayGrenadeTimerSound", .5f);
    }

    public void PlayGrenadeTimerSound()
    {
        grenadeSource.PlayOneShot(grenadeArming);

    }

    public void BeginExplosion()
    {
        Invoke("ActivateExplosionCollider", explosionColliderActivationDelay);
        grenadeSource.Stop();
        currentGrenadeState = grenadeState.Exploding;
        explosionObject.SetActive(true);
        grenadeSource.PlayOneShot(grenadeExploding);
        bombMesh.SetActive(false);
        bombRigidbody.isKinematic = true;
        this.gameObject.transform.localEulerAngles = Vector3.zero;
       
        Destroy(this.gameObject, 2.5f);
    }

    public void ActivateExplosionCollider()
    {

        explosionCollider.SetActive(true);
    }

    public void StopControllerVibration(bool pIsLeftcontroller,float ptimeDelay = 0f)
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
