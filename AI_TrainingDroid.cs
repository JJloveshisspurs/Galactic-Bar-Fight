using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_TrainingDroid : MonoBehaviour
{

    bool isDead = false;

    public List<MeshExploder> androidMesh;

    public BoxCollider collider;


    public Rigidbody rigidbody;

    public Animator animator;

    public bool audioPlayed;


    public TargetDroidSpawner droidSpawner;





    public void Start()
    {
        Invoke("DelayedSpawnAssign", .3f);
    }

    public void DelayedSpawnAssign()
    {

        droidSpawner = GetComponentInParent<TargetDroidSpawner>();


    }

    private void OnTriggerEnter(Collider pOther)
    {
        if (pOther.tag == "Projectile" || pOther.tag == "ExplodingProjectile")
        {
            if (isDead == false)
            {
                isDead = true;


                PlayRandomDroidDestructionAudio();
                EnemyIsDead(pOther.gameObject.tag);

                //if (pOther.tag == "melee" || pOther.gameObject.tag == "LethalMelee")
                 //   pOther.gameObject.SendMessage("MeleeJolt");

                

            }
        }

    }

   

     public void EnemyIsDead( string pLayerName) {


           // Destroy(this.gameObject, 5f);

           // this.gameObject.transform.parent.SendMessage("DroidDefeated");
            this.gameObject.transform.parent.SendMessage("DroidDefeated");



        switch (pLayerName)
        {

            case "Projectile":

                Invoke("DeathBYProjectile", .2f);
                break;

            case "ExplodingProjectile":

                Invoke("DeathByExplosiveProjectile", .2f);
                break;


            case "melee":
                Invoke("DeathByMelee", .2f);

                break;


            default:


                break;




        }


    }

    public void DeathByMelee()
    {
        

        if (collider != null)
        collider.isTrigger = false;

        if(rigidbody != null)
        rigidbody.isKinematic = true;

        if(animator != null)
        animator.enabled = false;

        // EnemyIsDead();
        if (droidSpawner != null)
            droidSpawner.DroidDefeated();
    }


    public void DeathBYProjectile()
    {
        if (collider != null)
            collider.isTrigger = false;

        if (rigidbody != null)
            rigidbody.isKinematic = true;

        if (animator != null)
            animator.enabled = false;

        if (droidSpawner != null)
        {
            droidSpawner.DroidDefeated();
            //droidSpawner.ClearDroids();

        }
    }

    public void DeathByExplosiveProjectile()
    {
        if (collider != null)
            collider.isTrigger = false;


        //EnemyIsDead();

        for (int i = 0; i < androidMesh.Count; i++)
        {
            androidMesh[i].Explode();


            androidMesh[i].gameObject.SetActive(false);
        }


        if (droidSpawner != null)
            droidSpawner.DroidDefeated();


    }

    public void PlayRandomDroidDestructionAudio()
    {
        if (audioPlayed == false)
        {
            audioPlayed = true;

            int oRand = Random.Range(0, 2);

            if (oRand == 0)
            {
                AudioManager.instance.PlayAudio(AudioData.audioClipType.TargetDroid_Destruction1, this.gameObject.transform, 1,.8f,true,true);


            }


            if (oRand == 1)
            {
                AudioManager.instance.PlayAudio(AudioData.audioClipType.TargetDroid_Destruction2, this.gameObject.transform, 1,.8f,true,true);


            }
        }

    }
}
