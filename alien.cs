using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BeautifulDissolves;

public class alien : MonoBehaviour
{
    private Transform attackPosition;

    public float movementSpeed;

    private float step = 0f;

    public Animator animator;

    public GameObject alienLaser;

    public GameObject laserFirePosition;

    private bool attacked;

    private float attackDelay = 3f;

    private GameObject projectile;

    private bool isDead;

    public Dissolve alienDissolve;

    public Rigidbody rigidBody;
    public Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        attackPosition = GameController.instance.GetAlienAttackPosition();

        //Invoke("Die", 2.5f);
    }

    // Update is called once per frame
    void Update()
    {
        if (GameController.instance.shipLanded && isDead == false)
        {
            moveIntoAttackPosition();

        }

    }

    public void moveIntoAttackPosition()
    {
        step = movementSpeed * Time.deltaTime; // calculate distance to move

        if (attacked == false)
        {
            transform.LookAt(new Vector3(attackPosition.position.x, attackPosition.position.y,
             0f));

            transform.position = Vector3.MoveTowards(transform.position,
           attackPosition.position, step);

        }

        // Check if the position of the cube and sphere are approximately equal.
        if (Vector3.Distance(transform.position, attackPosition.position) < 0.001f)
        {
            if (attacked == false)
            {
                attacked = true;
                BeginAttack();

            }
        }


    }


    public void BeginAttack()
    {
        transform.LookAt(GameController.instance.player.transform.position);

        Invoke("Attack", attackDelay);

        animator.Play("idle_pose_ with_ a_ gun", 0);

    }

    public void Attack()
    {

        if (isDead == false) { 

        projectile = Instantiate(alienLaser, laserFirePosition.transform.position, Quaternion.identity, laserFirePosition.transform);

        projectile.transform.rotation = laserFirePosition.transform.rotation;

        projectile.transform.parent = null;

        Invoke("ResetAttack", attackDelay);

        }
    }

    public void ResetAttack()
    {

        attackPosition = GameController.instance.GetAlienAttackPosition();
        attacked = false;
    }

    public void Die()
    {
        isDead = true;
        GameController.instance.score = GameController.instance.score + 100;
        GameController.instance.UpdateScore();
        animator.Play("dead", 0);
        alienDissolve.TriggerDissolve();
        Destroy(this.gameObject, 5f);
    }

    public void OnDissolveFinished()
    {
        animator.speed = 0f;
        rigidBody.useGravity = true;

    }

    private void OnTriggerEnter(Collider other)
    {
        if (isDead == false) {
            //if (other.gameObject.layer == 9)
            //{
            Destroy(other.gameObject);
            Die();
            //}
        
       
        //Debug.Log("Other Layer touched me !!!! == " + other.gameObject.name);
        }
    }

}
