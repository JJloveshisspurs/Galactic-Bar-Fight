using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_Enemy_TestAIM : MonoBehaviour
{
    //***Animation States
    private const string RUN_ANIMATION = "Base Layer.RifleRun";
    private const string RIFLE_SHOOT__ANIMATION = "Base Layer.ShootTwoHands";
    private const string ENEMY_HIDE__ANIMATION = "Base Layer.TakeCover";
    private const string ENEMY_SHOOT_HIDE__ANIMATION = "Base Layer.RifleShot";


    public Vector3 attackYRotation;

    public AI_Blaster blaster;

    public Animator animator;

    public Vector3 offsetNumber;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("ShootPlayer", 3f, 3f);
    }

    private void Update()
    {
        if (GameController.instance != null) {

            //this.gameObject.transform.LookAt(GameController.instance.GetPlayerTargetPosition());

            /* Vector3 direction = GameController.instance.GetPlayerTargetPosition() - transform.position;
             direction.Normalize();
             float rotation = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

             transform.LookAt( new Vector3(0f, 0f, rotation - 90));*/

            Vector3 relativePos = GameController.instance.GetPlayerTargetPosition() - transform.position;

            // the second argument, upwards, defaults to Vector3.up
            Quaternion rotation = Quaternion.LookRotation(relativePos, Vector3.up);
            transform.rotation = rotation;
        }
    }

    public void ShootPlayer()
    {
        /*attackYRotation = new Vector3(0f,
              GameController.instance.player.position.y,
            0f);

        this.gameObject.transform.LookAt(attackYRotation);*/


        animator.StopPlayback();
        animator.CrossFade(ENEMY_SHOOT_HIDE__ANIMATION, 0f);

       
       /* Vector3 relativePos = GameController.instance.GetPlayerTargetPosition() - transform.position;

        // the second argument, upwards, defaults to Vector3.up
        Quaternion rotation = Quaternion.LookRotation(relativePos, Vector3.up);
        transform.rotation = rotation;*/


        if (GameController.instance != null)
        {
            attackYRotation = new Vector3(GameController.instance.player.position.x,
                   GameController.instance.player.position.y,
                  GameController.instance.player.position.z);

           
        }

        blaster.ShootAtPlayer();

    }
}
