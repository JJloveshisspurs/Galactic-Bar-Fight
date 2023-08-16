using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AI_Spider : MonoBehaviour
{

    private const string SPIDER_IDLE_ANIMATION = "Base Layer.Spider_Idle";
    private const string SPIDER_RUN_ANIMATION = "Base Layer.Spider_Run";
    private const string SPIDER_ATTACK_ANIMATION = "Base Layer.Spider_Attack";
    private const string SPIDER_DIE_ANIMATION = "Base Layer.Spider_Die";
    public enum AIMovementState
    {

        Spawn,
        AttackPlayer,
        Run,
        DIE

    }

    public NavMeshAgent navMeshAgent;

    public Animator animator;

    public bool testAnimations;

    public GameObject positionTarget;

    public float distance;

    public bool enableMovement;

    public float attackDistance = 5f;

    public cannon AcidShot;

    public bool isDead;

    public Rigidbody enemyRigidBody;

    public bool findPlayerOnStart;

    public float movementSpeed;

    public GameObject purple_Flames_Prefab;
    public MeshExploder spider_SkinMesh_Exploder;
    public SkinnedMeshRenderer spider_SkinMesh;
     
    public float attackDelayTime = 5f;

    public float attackDelayTimer;

    private float initialAttackDelay = 3f;

    private bool attackIsActive;

    private bool isFrozen;

    private float initialAnimatorspeed = 1f;
    private float initialMovementSpeed = 20f;

    public GameObject spiderChitterAudioObject;


    // Start is called before the first frame update
    void Start()
    {
        if (findPlayerOnStart)
            SetPlayerPosition();


        if(testAnimations)
            StartCoroutine(testAnimationsOnSPawn());

        Invoke("DelayedInitialize",1f);
    }

    public void SetPlayerPosition()
    {

        positionTarget = GameObject.Find("PlayerControllerVR_Rebuild2023");

    }

    public void DelayedInitialize()
    {
        SetPlayerPosition();
        enableMovement = true;
        navMeshAgent.Warp(navMeshAgent.transform.position);
        navMeshAgent.speed = initialMovementSpeed;
        animator.speed = initialAnimatorspeed;
        isFrozen = false;

    }

    private void Update()
    {
        if (GameController.instance.currenGameState != GameController.gameState.gameplay || GameController.instance.timeIsFrozen == true )
        {

            Freeze();
        }


        if (positionTarget == null || isDead == true || isFrozen == true)
            return;

        
        
        if (enableMovement == true)
        {

            //*** Get absolute value of distance
            distance = Mathf.Abs(Vector3.Distance(this.gameObject.transform.position, positionTarget.transform.position));

            attackDelayTimer += Time.deltaTime;

            
            if (distance <= attackDistance )
            {
                if (attackDelayTimer >= attackDelayTime)
                {
                    attackDelayTimer = 0f;
                    //BeginAttack();

                    //*** Begin delayed attack
                    Invoke("BeginAttack",initialAttackDelay);
                }

            }
            else
            {
                EndAttack();
            }

        }
    }

    public void BeginAttack()
    {

        if (isFrozen == false)
        {
            attackIsActive = true;
            AcidShot.attack = true;
            animator.CrossFade(SPIDER_ATTACK_ANIMATION, 0);
            navMeshAgent.speed = 0f;
            attackDelayTimer = 0f;
        }
    }


    public void EndAttack()
    {
        AcidShot.attack = false;
        navMeshAgent.SetDestination(positionTarget.transform.position);
        //navMeshAgent.Warp(positionTarget.transform.position);
        animator.CrossFade(SPIDER_RUN_ANIMATION, 0);
        navMeshAgent.speed = movementSpeed;
        attackDelayTimer = 0f;
        attackIsActive = false;
    }

    IEnumerator testAnimationsOnSPawn()
    {
        animator.CrossFade(SPIDER_IDLE_ANIMATION, 0);
        yield return new WaitForSeconds(4f);
        animator.CrossFade(SPIDER_RUN_ANIMATION, 0);

        yield return new WaitForSeconds(4f);

        animator.CrossFade(SPIDER_ATTACK_ANIMATION, 0);
        yield return new WaitForSeconds(4f);
        animator.CrossFade(SPIDER_DIE_ANIMATION, 0);
        yield return new WaitForSeconds(4f);

        StartCoroutine(testAnimationsOnSPawn());

    }

    public void SpiderIsDead()
    {

        //if (isDead == false)
        //{

            GameController.instance.DecrementSpiderCount();

            AudioManager.instance.PlayAudio(AudioData.audioClipType.SpiderSqueal, this.gameObject.transform, 0f, .7f, false, true);
            spiderChitterAudioObject.gameObject.SetActive(false);

            //enemyRigidBody.freezeRotation = true;
            enemyRigidBody.constraints = RigidbodyConstraints.FreezeAll; 

            Debug.Log("Spider is dead!");
            isDead = true;
            animator.speed = Random.Range(.9f, 1.2f);
            animator.CrossFade(SPIDER_DIE_ANIMATION, 0);
            navMeshAgent.speed = 0;


            Destroy(this.gameObject, 2f);
        //}
    }

    public void DeathByFlames()
    {

        //if (isDead == false)
        //{
            purple_Flames_Prefab.SetActive(true);

            GameController.instance.DecrementSpiderCount();

            AudioManager.instance.PlayAudio(AudioData.audioClipType.SpiderSqueal, this.gameObject.transform, 0f, .7f, false, true);
            spiderChitterAudioObject.gameObject.SetActive(false);

        //enemyRigidBody.freezeRotation = true;
        enemyRigidBody.constraints = RigidbodyConstraints.FreezeAll;

            Debug.Log("Spider is dead!");
            isDead = true;
            animator.speed = Random.Range(.9f, 1.2f);
            animator.CrossFade(SPIDER_DIE_ANIMATION, 0);
            navMeshAgent.speed = 0;

           

            Destroy(this.gameObject, 2f);
        //}


    }

    public void Freeze()
    {
        isFrozen = true;
        animator.speed = 0f;
        navMeshAgent.speed = 0f;

        positionTarget = null;
        Invoke("DelayedInitialize", 10f);

        if (GameController.instance != null)
            GameController.instance.ResetTimeFreeze(10f);
    }

    public void ForceUnfreeze()
    {
        DelayedInitialize();



    }

    public void Explode()
    {
        spider_SkinMesh.enabled = false;
        spider_SkinMesh_Exploder.Explode();
    }

    public void OnDestroy()
    {
        if (isDead == false)
            GameController.instance.DecrementSpiderCount();
    }
}
