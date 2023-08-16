using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_Blaster : MonoBehaviour
{
    public GameObject projectile;

    public void ShootAtPlayer()
    {

        

        this.gameObject.transform.LookAt(GameController.instance.GetEnemyAimTargetPosition());

        //*** Firing weapon
        GameObject oProjectile =  Instantiate(projectile, this.gameObject.transform,true);

        oProjectile.transform.localEulerAngles = Vector3.zero;

        oProjectile.transform.localPosition = Vector3.zero;

        oProjectile.transform.parent = null;

        if(AudioManager.instance != null)
        AudioManager.instance.PlayAudio(AudioData.audioClipType.Enemy_Blaster, this.gameObject.transform, 0f,.7f,true,true);
    }
}
