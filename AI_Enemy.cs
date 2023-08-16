using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class AI_Enemy : MonoBehaviour
{
    public bool isMeleeEnemy;

    //***Animation States - Projectile AI
    private const string RUN_ANIMATION = "Base Layer.RifleRun";
    private const string RIFLE_SHOOT__ANIMATION = "Base Layer.RifleShot";
    private const string ENEMY_HIDE__ANIMATION = "Base Layer.TakeCover";


    //***Animation States - Melee AI
    private const string MELEE_SPAWN_ANIMATION = "Base Layer.Roar";
    private const string MELEE_RUN_ANIMATION = "Base Layer.Sprint";
    private const string MELEE_ATTACK_ANIMATION = "Base Layer.Attack";
    private const string MELEE_PARRIED_ANIMATION = "Base Layer.StunnedFall";

    //*** Universal states
    private const string STUMBLE_ANIMATION = "Base Layer.Stumble";

    public enum AIMovementState {

        RunToPosition,
        Disable,
        ShootAtPlayer,
        affected,
        attackingMelee

    }

    public AIMovementState currentAiState;



    public AIBehaviorStates initialBehavior;
    public AIBehaviorStates currentBehavior;

    public NavMeshAgent navMeshAgent;

    public Animator animator;


    public GameObject positionTarget;

    private float runSpeed = 4f;
    private float meleeRunSpeed = 5f;

    public MeshExploder meshExploderHead;

    public MeshExploder meshExploderBody;


    public Rigidbody enemyRigidbody;

    private bool beginShooting = false;

    public float minimumAttackDistance = 1.3f;

    private float movementUpdateTimer;
    private float movementupdateTime = .1f;

    private float distanceFromTarget = 0f;

    bool shootingStarted;

    [SerializeField]
    bool isDead = false;

    bool isDisabled = false;

    public AI_Blaster blaster;


    public Vector3 attackYRotation;

    public int scoreForEnemyKill = 100;

    public float distance;

    public BoxCollider longPoleArmCollider;

    public GameObject DisabledParticle;

    
    public float enemyUpdateInterval = .1f;
    public float enemyUpdateTime = .1f;


    private float enemyDeathCheckInterval = 1.5f;
    public float enemyDeathCheckTimer;

    bool addedToEnmyCount = false;

    public bool testAutoKill;

    public GameObject growlAudio;

    public bool shootingDisabled;


    public List<SkinnedMeshRenderer> skinMeshRnderers;

    // Start is called before the first frame update
    void Start()
    {
        if (testAutoKill)
        {

            Invoke("EnemyIsDead", 5f);

        }
       

            InitializeEnemy();

        
    }

    // Update is called once per frame
    void Update()
    {
        enemyUpdateTime += Time.deltaTime;

        movementUpdateTimer += Time.deltaTime;

        enemyDeathCheckTimer += Time.deltaTime;

        if(GameController.instance != null && enemyDeathCheckTimer >= enemyDeathCheckInterval)
        {
            enemyDeathCheckTimer = 0f;

            //*** Make sure we elimintae weird cases of invincible enemies or enemies fighting after round is over
            if (GameController.instance.currenGameState == GameController.gameState.gameOver || isDead)
                Destroy(this.gameObject,3f);

        }


        if(isDisabled)
            return;

        //*** Update a few times  second
        if (enemyUpdateTime >= enemyUpdateInterval)
        {
            enemyUpdateTime = 0f;

            if (GameController.instance.currenGameState == GameController.gameState.resetting && isDisabled == false)
            {
                isDisabled = true;

                ForceEnemyStumble();
            }

            //if (GameController.instance.currenGameState == GameController.gameState.resetting)
               // return;


            if (GameController.instance.currenGameState == GameController.gameState.gameplay)
            {

                if (currentAiState == AIMovementState.RunToPosition)
                    MoveTowardsAttackPosition();

            }

            if (GameController.instance != null)
            {
                //*** If Game round is over , kill remaining enemies
                if (GameController.instance.currenGameState == GameController.gameState.gameOver && isDead == false)
                {

                    ForceEnemyDead();

                }
            }
        }

    }

    private void OnTriggerEnter(Collider pOther)
    {
       

            //*** If Enemy touches shield, strun them
            if (pOther.gameObject.layer == 25)
            {
                DelyedForceStumble();

                if (AudioManager.instance != null)
                    AudioManager.instance.PlayAudio(AudioData.audioClipType.BubbleShieldHit, pOther.transform, 0f,-1f,true);

                return;
            }


            if (pOther.gameObject.tag == "Projectile" && isDead == false)
            {
                isDead = true;
                Debug.Log("Hit with projectile!!!");
                Invoke("DeathByProjectile", .2f);

            AudioManager.instance.PlayAudio(AudioData.audioClipType.alienOuch, this.gameObject.transform, 0f, .7f, true, true);
        }

        if (pOther.gameObject.tag == "ExplodingProjectile" && isDead == false)
            { 
                isDead = true;
            
                Debug.Log("Hit with  exploding projectile!!!");
                Invoke("DeathByExplosiveProjectile", .2f);

            AudioManager.instance.PlayAudio(AudioData.audioClipType.alienOuch, this.gameObject.transform, 0f, .7f, true, true);
        }

           /* if ((pOther.gameObject.tag == "LethalMelee"  && isDead == false))
            {
                isDead = true;
                Debug.Log("Hit with Melee weapon!!!");
                Invoke("DeathByMelee", .2f);

            
               // pOther.gameObject.SendMessage("MeleeJolt");
        }*/
        

    }


    public void InitializeEnemy() {


        isDisabled = false;

        if (addedToEnmyCount == false)
        {
            addedToEnmyCount = true;
            //if (GameController.instance != null)
                //GameController.instance.IncrementEnemyCount();
        }

        if (animator != null)
            animator.StopPlayback();


        //*** A Forcible reset to make sure AI breaks out of their Disabled naimation state
        if (animator != null)
        {

            if (isMeleeEnemy == false)
            {

                animator.StopPlayback();
                animator.CrossFade(RUN_ANIMATION, 0);
                //navMeshAgent.isStopped = false;

            }
            else
            {
                animator.StopPlayback();
                animator.CrossFade(MELEE_RUN_ANIMATION, 0);
                //navMeshAgent.isStopped = false;

            }
        }


        if (animator!= null)
        animator.applyRootMotion = false;

        if (DisabledParticle != null)
            DisabledParticle.SetActive(false);


        if (isMeleeEnemy == false)
        {
            if (navMeshAgent != null)
                navMeshAgent.isStopped = false;

            if (animator != null)
                animator.speed = 1f;

            if (navMeshAgent != null)
                navMeshAgent.speed = runSpeed;

            Invoke("InitializeFirstEnemyPosition", 1f);


            //InvokeRepeating("ShootAtPlayer", 4f,4f);

            float oAttackInterval = UnityEngine.Random.Range(4f, 7f);

            InvokeRepeating("MakeADecision", oAttackInterval, oAttackInterval);

        }
        else
        {
            if (AudioManager.instance != null)
                AudioManager.instance.PlayAudio(AudioData.audioClipType.EnemyMelee_Screech,this.gameObject.transform,0.2f,.6f,true,true);
           
           Invoke("InitializeMeleeEnemy",3f);
        }
    }

    public void InitializeMeleeEnemy() {
        Debug.Log("Initializing Melee Enemy position");

        if (navMeshAgent != null)
            navMeshAgent.isStopped = false;

        if (animator != null)
            animator.speed = 1f;

        if (navMeshAgent != null)
            navMeshAgent.speed = meleeRunSpeed;

        Invoke("InitializeFirstEnemyMeleePosition", .1f);

    }





    public void MakeADecision() {

        shootingStarted = false;
        int oDecisionNum = Random.Range(0, 100);


        if(oDecisionNum  >= 0 && oDecisionNum <= 30) {
            //*** Move positions
            GetEnemyPosition();

        }else if (oDecisionNum > 30 && oDecisionNum < 90)
            {
            //*** Shoot player
            ShootAtPlayer();
            }
        else if (oDecisionNum >= 90  )
        {
            //***  DoSpecial reaction
            SpecialReaction();

        }





    }




    public void InitializeFirstEnemyPosition() {

        currentBehavior = initialBehavior;

        positionTarget = AI_Position_Controller.instance.GetAITargetPosition(currentBehavior.currentBehavior).gameObject;



    }

    public void InitializeFirstEnemyMeleePosition()
    {

        currentBehavior = initialBehavior;

        positionTarget = AI_Position_Controller.instance.GetAITargetPosition(currentBehavior.currentBehavior).gameObject;

        currentAiState = AIMovementState.RunToPosition;

    }

    public void GetEnemyPosition()
    {

        currentBehavior = initialBehavior;

        positionTarget = AI_Position_Controller.instance.GetAITargetPosition(currentBehavior.currentBehavior).gameObject;

        currentAiState = AIMovementState.RunToPosition;

    }



   

    public void MoveTowardsAttackPosition() {

        if (positionTarget == null)
            return;

        if (isMeleeEnemy)
        {
           
            //*** Check melee attack positions


            CheckMeleeAttackPosition();


            if (growlAudio != null)
                growlAudio.SetActive(true);
        }
        else
        {



            //*** Set Destination position
            if (navMeshAgent != null)
                navMeshAgent.SetDestination(positionTarget.transform.position);

            if (movementUpdateTimer >= movementupdateTime)
            {
                distanceFromTarget = Mathf.Abs(Vector3.Distance(this.gameObject.transform.position, positionTarget.transform.position));

                //Debug.Log("distance from target" + distanceFromTarget.ToString());

                if (distanceFromTarget <= minimumAttackDistance)
                {
                    //*** Reached destination waypoint
                    if (shootingStarted == false)
                    {

                        ShootAtPlayer();
                    }
                }
                else
                {
                    //*** Haven't reached destination keep moving !
                    if (navMeshAgent.isStopped == false)
                    {
                        if (animator != null)
                        {

                            if (isMeleeEnemy == false)
                            {

                                animator.StopPlayback();
                                animator.CrossFade(RUN_ANIMATION, 0);
                                navMeshAgent.isStopped = false;

                                navMeshAgent.speed = runSpeed;
                            }
                            else
                            {
                                animator.StopPlayback();
                                animator.CrossFade(MELEE_RUN_ANIMATION, 0);
                                navMeshAgent.isStopped = false;

                                navMeshAgent.speed = runSpeed;

                            }
                        }
                    }
                }

                /*if (navMeshAgent.isStopped == true && beginShooting == true && shootingStarted == false)
                {
                    beginShooting = false;
                    ShootAtPlayer();

                }*/
            }
        }
    }

    public void CheckMeleeAttackPosition()
    {
        if (currentAiState != AIMovementState.attackingMelee)
        {
            if (navMeshAgent != null)
                navMeshAgent.isStopped = false;

            //*** Get absolute value of distance
            distance = Mathf.Abs(Vector3.Distance(this.gameObject.transform.position, positionTarget.transform.position));

            //*** Set Destination position
            if (navMeshAgent != null)
                navMeshAgent.SetDestination(positionTarget.transform.position);

            if(animator != null)
            animator.CrossFade(MELEE_RUN_ANIMATION, 0);
             //Debug.Log("Distance = " + Mathf.Abs(distance));

            if (distance <= 2.4f)
            {
                navMeshAgent.isStopped = true;
                AttackPlayer();
            }

        }
        // }
    }



    public void ShootAtPlayer()
    {
        if (navMeshAgent != null)
            navMeshAgent.isStopped = true;

        navMeshAgent.speed = 0f;

        currentAiState = AIMovementState.ShootAtPlayer;

        shootingStarted = true;
        //animator.SetTrigger("Shooting");
        //animator.
        Debug.Log("Shooting at position : " + GameController.instance.player.position.ToString());



        Vector3 relativePos = GameController.instance.GetPlayerTargetPosition() - transform.position;

        // the second argument, upwards, defaults to Vector3.up
        Quaternion rotation = Quaternion.LookRotation(relativePos, Vector3.up);
        transform.rotation = rotation;


        if (animator != null)
            animator.CrossFade(RIFLE_SHOOT__ANIMATION,0);

        Invoke("AttackPlayer", .1f);

    }

    public void AttackPlayer() {

        if (isDead == false)
        {

            //*** Aim and attack at player
            if (isMeleeEnemy == false)
            {

                if (shootingDisabled == false)
                {
                    navMeshAgent.speed = 0f;
                    if (blaster != null)
                        blaster.ShootAtPlayer();
                }

                Invoke("GetEnemyPosition", 2f);
            }
            else
            {
                if (navMeshAgent != null)
                    navMeshAgent.isStopped = true;

                transform.LookAt(GameController.instance.player);
                currentAiState = AIMovementState.attackingMelee;

                if (animator != null)
                    animator.CrossFade(MELEE_ATTACK_ANIMATION, 0);



            }

        }
    }

    public void SpecialReaction()
    {
        navMeshAgent.speed = 0f;

        if (navMeshAgent != null)
            navMeshAgent.isStopped = true;

        if (animator != null)
            animator.CrossFade(ENEMY_HIDE__ANIMATION, 0);

        //navMeshAgent.speed = 0f;
    }


    //Upon collision with another GameObject, this GameObject will reverse direction
  
    public void EnemyIsStunned() {

        navMeshAgent.speed = 0f;

        //*** eactivate pole arm collider
        if (longPoleArmCollider != null)
            longPoleArmCollider.enabled = false;

        //*** Play Animation for Parried enemy
        if (animator != null)
            animator.CrossFade(MELEE_PARRIED_ANIMATION, 0);

        if (growlAudio != null)
            growlAudio.SetActive(false);


        //navMeshAgent.speed = 0f;
    }



    public void DeathByExplosiveProjectile() {
        //*** Increment Kill count
        GameController.instance.IncrementDisentegrationCount();


        CancelPreviousOperations();
        isDead = true; 


        EnemyIsDead();
      

        meshExploderHead.Explode();
        meshExploderBody.Explode();

        meshExploderHead.gameObject.SetActive(false);
        meshExploderBody.gameObject.SetActive(false);

       for(int i = 0; i < skinMeshRnderers.Count; i++)
        {
            skinMeshRnderers[i].enabled = false;



        }

       
    }

    public void DeathByProjectile()
    {
        Debug.Log("DEAD BY PROJECTILE !!!!");

        CancelPreviousOperations();
      

        EnemyIsDead();
    }

    public void DeathByMelee()
    {
        Debug.Log("DEAD BY MELEE !!!!");
        if (isDead == false)
        {
            isDead = true;

            CancelPreviousOperations();

            GameController.instance.IncrementMeleeKillCount();

            AudioManager.instance.PlayAudio(AudioData.audioClipType.alienOuch, this.gameObject.transform, 0f, .7f, true, true);

            EnemyIsDead();
        }
    }

    public void EnemyIsDead() {

        navMeshAgent.speed = 0f;

        if (growlAudio != null)
            growlAudio.SetActive(false);


        DestroyAlien();

        Debug.Log("ENEMY IS DEAD !!!!");

        if (longPoleArmCollider != null)
        longPoleArmCollider.enabled = false;

        CancelPreviousOperations();

        

        

        int oScoreValue = scoreForEnemyKill * GameController.instance.scoreMultiplier;

        KillScoreTextRenderer.instance.RenderScoreText(oScoreValue, this.gameObject.transform);

        //*** Increment Kill count
        GameController.instance.IncrementKillCount();

        GameController.instance.IncrementScoreCount(oScoreValue);

       

        if (enemyRigidbody != null)
            enemyRigidbody.isKinematic = true;

        if (navMeshAgent != null)
            navMeshAgent.isStopped = true;

        if (animator != null)
            animator.enabled = false;


        if (DisabledParticle != null)
            DisabledParticle.SetActive(false);

        AudioManager.instance.PlayRandomAriaQuip(1f);

        positionTarget = null;

        shootingDisabled = true;

        //ChallengeCompletionManager.instance.IncrementKillCount();


    }

    public void ForceEnemyDead()
    {
        Debug.Log(" ENEMY FORCED DEAD !!!!");

        navMeshAgent.speed = 0f;

        if (growlAudio != null)
            growlAudio.SetActive(false);

        DestroyAlien();


        if (longPoleArmCollider != null)
            longPoleArmCollider.enabled = false;

        CancelPreviousOperations();

       
        isDead = true;

        shootingDisabled = true;

      
        AudioManager.instance.PlayRandomizedAlienDeathSound(this.gameObject.transform, 0f);

        if (enemyRigidbody != null)
            enemyRigidbody.isKinematic = true;

        if (navMeshAgent != null)
            navMeshAgent.isStopped = true;

        if (animator != null)
            animator.enabled = false;

      
        positionTarget = null;

        //ChallengeCompletionManager.instance.IncrementKillCount();

       


    }


    public void DestroyAlien()
    {
       

        Destroy(this.gameObject, 5f);
    }

    public void CancelPreviousOperations() {
        CancelInvoke();

        //"MakeADecision"
        CancelInvoke("MakeADecision");
            
    }

    public void StrikeAnimation_Begin()
    {
        Debug.Log("Beginning Strike Animation!");
        Invoke("DelayStrikeCollider",.75f);

    }

    public void DelayStrikeCollider() {
        if (longPoleArmCollider != null)
            longPoleArmCollider.enabled = true;
    }


    public void StrikeAnimation_End()
    {
        Debug.Log("End Strike Animation!");
        if (longPoleArmCollider != null)
            longPoleArmCollider.enabled = false;
    }

    public void FreezeEnemy() {

        if (longPoleArmCollider != null)
            longPoleArmCollider.enabled = false;

        if (animator != null)
            animator.speed = 0f;

        currentAiState = AIMovementState.affected;
        CancelInvoke();

        if (navMeshAgent != null)
            navMeshAgent.isStopped = true;



        //*** Reset Enemy in 10 seconds
        Invoke("InitializeEnemy", 10f);
    }

    public void ForceEnemyStumble()
    {
        CancelInvoke();

        Invoke("DelyedForceStumble", .15f);

    }

    public void ResetEnemyStumble()
    {
        isDisabled = true;

        if (DisabledParticle != null)
            DisabledParticle.SetActive(true);

        if (animator != null)
            animator.applyRootMotion = false;
    }

    public void DelyedForceStumble() {

        isDisabled = true;

        CancelInvoke();

        if (longPoleArmCollider != null)
            longPoleArmCollider.enabled = false;

        if(animator != null)
        animator.applyRootMotion = false;

        if (animator != null)
            animator.CrossFade(STUMBLE_ANIMATION, 0);

        if(DisabledParticle != null)
        DisabledParticle.SetActive(true);

        if (navMeshAgent != null)
        {
            navMeshAgent.speed = 0f;
            navMeshAgent.isStopped = true;

        }

        //*** Re-initialize enemy Initialize  in reset time number of seconds 
        Invoke("InitializeEnemy", GameController.instance.resetTime);
    }
}








