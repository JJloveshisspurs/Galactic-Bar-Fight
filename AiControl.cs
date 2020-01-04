using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using BeautifulDissolves;

public class AiControl : MonoBehaviour
{
    public float walkSpeed;
    public float runSpeed;
    public RichAI aiNavigation;
    public AIDestinationSetter aiTarget;

    public AIbehaviorData.AiBehaviors behaviorGroup;
    public AISubBehaviorData activeSubBehavior;
    public AnimationControl animations;

    public bool isMeleeEnemy;
    public bool isMoving;

    public bool isAttacking;

    public bool isTakingCover;


    public GameObject alienLaser;

    public GameObject laserFirePosition;

    private bool attacked;

    private float attackDelay = 3f;

    private GameObject projectile;

    private bool isDead;

    public Dissolve alienDissolve;



    private bool firstCheck = true;

    private float updateTimer;

    private float  updaterTime = .25f;

    private float meleeAttackDistance = 1.75f;

    //*** Animation stings all on the combat layer.  
    private string walkAnimation = "Walking";
    private string runAnimation = "Run";
    private string coverAnimation = "Ducking";
    private string ShootingAnimation = "Shooting3";
    private string meleeAttackAnimation = "Bayonetta_Stab";
    private string crouchIdle = "Crouch_Idle";
    private string crouchAttack = "Crouch_Shot";



    // Start is called before the first frame update
    void Start()
    {
        InitializeAIAgent();
    }

    private void Update()
    {
        updateTimer += Time.deltaTime;

        if (updateTimer > updaterTime)
        {
            updateTimer = 0f;

            UpdateAgent();

        }
    }

    void InitializeAIAgent()
    {


        Invoke("GetPositionTarget", Random.Range(1f, 3f));


    }

    void UpdateAgent()
    {
        if (aiNavigation != null && aiTarget != null)
        {

            if (isAttacking == false)
            {
                if (isMeleeEnemy)
                    Debug.Log(" vector3.distance ==" + Vector3.Distance(aiTarget.target.transform.position, aiNavigation.gameObject.transform.position).ToString());

                float distanceFromTarget = Vector3.Distance(aiTarget.target.transform.position, aiNavigation.gameObject.transform.position);

                if (isMeleeEnemy)
                {

                    if (distanceFromTarget <= meleeAttackDistance)
                    {
                        isAttacking = true;
                        Attack();
                    }
                }
                else
                {

                    if (distanceFromTarget <= 1f)
                    {
                        isAttacking = true;
                        Attack();
                    }
                }
            }

        }
        else {


            Debug.LogWarning("<><>AI Naviagtion Contoller or I target  is null !!!!!");

    }
    }


    public void GetAIBehavior()
    {
        //*** This is incorrect, we should be assigning here
        AIController.instance.GetSpecificAISocialBehaviorGroup(behaviorGroup);


    }

    public void SetSubBehaviorData()
    {
        //*** There is an option for adign a specific sub behavior by supplying the sub behavior index
        activeSubBehavior = AIController.instance.GetCombatAISubBehavior(behaviorGroup);

    }

    public void GetPositionTarget()
    {
        ResetAttack();

        if (isMeleeEnemy)
        {
            aiTarget.target = GameController.instance.player;

        }
        else
        {
            if (aiTarget != null)
            {

                //Debug.Log("Getting position target !!!");
                aiTarget.target = AIController.instance.GetIdealMovementPosition(this.gameObject.transform.position);

            }
            else
            {


                Debug.LogWarning("<><>AItarget is null !!!!!");

            }
        }

        AttackOrMove(firstCheck);


            firstCheck = false;
    }

    public void AttackOrMove(bool pInitializing = false)
    {
        Debug.Log("Atacking or moving!!!!");
        if (pInitializing)
        {
            Move();

        }
        else
        {
            int pActionNum = Random.Range(0, 20);

            Debug.Log("Rolling for next action , roll num == " + pActionNum.ToString());
            if( pActionNum  < 8)
            {
                Attack();
               

            }else if (pActionNum >= 8 && pActionNum <= 14)
            {

                 Move();

            }else  if(pActionNum >= 15)
            {
                if (isMeleeEnemy  == false)
                {
                    TakeCover();
                }
                else
                {
                    Attack();
                }

            }

        }

    }


    public void Attack()
    {
        CancelInvoke();

        isAttacking = true;

        transform.parent.LookAt(GameController.instance.player);

        //*** Disable movment ;
        aiNavigation.canMove = false;

        if (isMeleeEnemy)
        {

            activeSubBehavior = AIController.instance.GetCombatAISubBehavior(AIbehaviorData.AiBehaviors.Attacking_Melee, 0);
           animations.SetSpecificAnimation(2,meleeAttackAnimation,false);
        }
        else
        {
            activeSubBehavior = AIController.instance.GetCombatAISubBehavior(AIbehaviorData.AiBehaviors.Attacking_Shooting, 0);
            animations.SetSpecificAnimation(2, ShootingAnimation, false);

            if (laserFirePosition != null)
            {
                Invoke("OnProjectileAttack", 1.01f);
            }
        }

      
        Invoke("GetPositionTarget", Random.Range(5f, 8f));
    }

    public void Move()
    {
        if (aiNavigation != null)
        {

            CancelInvoke();

            if (isMeleeEnemy)
            {
                aiNavigation.maxSpeed = walkSpeed;

                animations.SetSpecificAnimation(2, walkAnimation, true);
            }
            else
            {
                aiNavigation.maxSpeed = runSpeed;

                animations.SetSpecificAnimation(2, runAnimation, true);

            }


            isMoving = true;

            aiNavigation.canMove = true;

            activeSubBehavior = AIController.instance.GetCombatAISubBehavior(AIbehaviorData.AiBehaviors.Moving, 0);


            Invoke("GetPositionTarget", Random.Range(10f, 13f));

        }
        else
        {


            Debug.LogWarning("<><>AI Naviagtion  is null !!!!!");

        }
    }

    public void TakeCover()
    {
        CancelInvoke();

        isTakingCover = true;

        transform.parent.LookAt(GameController.instance.player);

        //*** Disable movment ;
        aiNavigation.canMove = false;

        animations.SetSpecificAnimation(2, crouchIdle, true);
       

       
        Invoke("GetPositionTarget", Random.Range(5f, 8f));


    }

    public void HandleSubBehavior()
    {
        //*** Begin setting behaviors based on assigned behavior groups
        switch (behaviorGroup)
        {
            case AIbehaviorData.AiBehaviors.Spawning:


                break;

            case AIbehaviorData.AiBehaviors.Socializing:


                break;


            case AIbehaviorData.AiBehaviors.Moving:


                break;


            case AIbehaviorData.AiBehaviors.Attacking_Melee:


                break;


            case AIbehaviorData.AiBehaviors.Attacking_Shooting:


                break;


            case AIbehaviorData.AiBehaviors.Taking_Cover:


                break;


            default:


                break;


        }


    }

    public void OnSpawn()
    {


    }

    public void OnMove()
    {


    }

    public void OnMeleeAttack()
    {


    }

    public void OnProjectileAttack()
    {
        projectile = Instantiate(alienLaser, laserFirePosition.transform.position, Quaternion.identity, laserFirePosition.transform);

        projectile.transform.rotation = laserFirePosition.transform.rotation;

        projectile.transform.parent = null;

    }

    public void OnDisabled()
    {


    }

    public void OnTakeCover()
    {


    }

    public void ResetAttack()
    {
        isAttacking = false;
        isTakingCover = false;
    }


    public void SetAnimation()
    {
        animations.assignSpecicifcAnimationFromGroup = true;
        animations.specificAnimationLayer = activeSubBehavior.animationLayerIndex;
        animations.specificAnimationIndex = activeSubBehavior.animationIndex;
        animations.GetAnimationSpecificFromGroup();

    }

}
