using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamageTrigger : MonoBehaviour
{
    public enum enemyType
    {
        None,
        Spider,
        Droid
    }

    public enemyType currentEnemytype;
    public bool lock_X_On_Death;
    public bool lock_Y_On_Death;
    public bool lock_Z_On_Death;

    bool isDead = false;

    public AI_Spider spiderAI;

    public EnemyDroneController droneAI;

    public GameObject damageParticles;


    public int scoreForEnemyKill;


    private float deathCheckInterval = .3f;
    private float deathCheckTimer = 0f;

    

    public void Start()
    {
        
    }

    public void Update()
    {
        deathCheckTimer += Time.deltaTime;


        if (deathCheckTimer >= deathCheckInterval)
        {
            deathCheckTimer = 0f;

            if (GameController.instance != null)
            {
                if (GameController.instance.currenGameState == GameController.gameState.gameOver)
                {
                    if (spiderAI != null)
                        spiderAI.SpiderIsDead();

                    if (droneAI != null)
                        droneAI.DroneDeath();

                }
            }
        }
    }

    public void FreezeEnemies()
    {

        if (spiderAI != null)
            spiderAI.Freeze();

        if (droneAI != null)
        {
            droneAI.DroneFreeze();
            droneAI.orbitalClass.FreezeOrbit();
        }


    }

    public void ForceUnfreezeEnemy()
    {
        if (spiderAI != null)
            spiderAI.ForceUnfreeze();

        if (droneAI != null)
        {
            droneAI.ForceUnfreeze();
            
        }



    }


    // Start is called before the first frame update
    private void OnTriggerEnter(Collider pOther)
    {


        //*** If Enemy touches shield, stun them
        if (pOther.gameObject.layer == 25)
        {
            //DelyedForceStumble();

            if (AudioManager.instance != null)
                AudioManager.instance.PlayAudio(AudioData.audioClipType.BubbleShieldHit, pOther.transform, 0f, -1f, true);

            return;
        }

        Debug.Log("other gameobject tag == " + pOther.gameObject.tag);
        if ((pOther.gameObject.tag == "Projectile" || pOther.gameObject.tag == "Flame" || pOther.gameObject.tag == "Spire" || pOther.gameObject.tag == "ExplodingProjectile"
            || pOther.gameObject.tag == "LethalMelee") && isDead == false)
        {
            isDead = true;
            Debug.Log("Hit with projectile!!!");
            //Invoke("DeathByProjectile", .2f);
           
            

            //AudioManager.instance.PlayAudio(AudioData.audioClipType.alienOuch, this.gameObject.transform, 0f, .7f, true, true);
            if (currentEnemytype == enemyType.Spider)
            {
                GameController.instance.IncrementSpidersKilled();
                GameController.instance.IncrementKillCount();

                int oScoreValue = scoreForEnemyKill * GameController.instance.scoreMultiplier;

                KillScoreTextRenderer.instance.RenderScoreText(oScoreValue, this.gameObject.transform);

                GameController.instance.IncrementScoreCount(oScoreValue);

                if (pOther.gameObject.tag == "Flame")
                {
                    Debug.Log("Spiders should be burning!!!");
                    spiderAI.DeathByFlames();

                    
                }else if (pOther.gameObject.tag == "ExplodingProjectile")
                {
                    spiderAI.Explode();
                    spiderAI.SpiderIsDead();
                }
                else
                {



                    spiderAI.SpiderIsDead();

                    if (damageParticles != null)
                        damageParticles.SetActive(true);

                }
            }

            if (currentEnemytype == enemyType.Droid)
            {
                GameController.instance.IncrementDroidssKilled();
                GameController.instance.IncrementKillCount();


                int oScoreValue = scoreForEnemyKill * GameController.instance.scoreMultiplier;

                KillScoreTextRenderer.instance.RenderScoreText(oScoreValue, this.gameObject.transform);

                GameController.instance.IncrementScoreCount(oScoreValue);

                if (pOther.gameObject.tag == "Flame")
                {
                    Debug.Log("Droids should be burning!!!");
                    droneAI.DroneDeathByFire();
                }
                else
                {
                  

                    droneAI.DroneDeath();

                    if (damageParticles != null)
                        damageParticles.SetActive(true);
                }
            }
        }

        if (pOther.gameObject.tag == "ExplodingProjectile" && isDead == false)
        {
            isDead = true;

            Debug.Log("Hit with  exploding projectile!!!");
            //Invoke("DeathByExplosiveProjectile", .2f);
            //GameController.instance.DecrementDroidCount();
            //AudioManager.instance.PlayAudio(AudioData.audioClipType.alienOuch, this.gameObject.transform, 0f, .7f, true, true);
        }

        /* if ((pOther.gameObject.tag == "LethalMelee"  && isDead == false))
         {
             isDead = true;
             Debug.Log("Hit with Melee weapon!!!");
             Invoke("DeathByMelee", .2f);


            // pOther.gameObject.SendMessage("MeleeJolt");
     }*/


    }
}
