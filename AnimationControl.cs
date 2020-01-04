using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AnimationControl : MonoBehaviour {

    public AnimationData.AnimationType selectedAnimationtype;

	public AnimationData currentAvailableAnimations;

    public int activeAnimationIndex;
    public int activeAnimationLayer;
    public float animationSpeed = 1f;

    public bool setRandomAnimationFromGroup;
    public bool RerollAnimationOnCompletion;

    public bool assignSpecicifcAnimationFromGroup;
    public int specificAnimationIndex;
    public int specificAnimationLayer;

    public Animator animator;

 
    private string specifiedAnimationName;

    public void Start()
    {
        if (setRandomAnimationFromGroup)
            GetAnimationRandomFromGroup();

        if (assignSpecicifcAnimationFromGroup)
            GetAnimationSpecificFromGroup();

        animator.speed = animationSpeed;
    }

    public void SetAnimationGeneral(){

        ResetLayerWeights(activeAnimationLayer);
        animator.SetLayerWeight(activeAnimationLayer, 1f);

       
       
       
        Debug.Log("Animation index == " + activeAnimationIndex.ToString() + " active animation layer ==" + activeAnimationLayer.ToString());
        animator.Play(currentAvailableAnimations.animationNames[activeAnimationIndex], activeAnimationLayer);

        if (RerollAnimationOnCompletion)
        {
            //Debug.Log(" Setting animation to reroll !");
            Invoke("RerollAnimation", animator.GetCurrentAnimatorStateInfo(activeAnimationLayer).length);
        }
	}

    public void SetSpecificAnimation(int pAnimationLayer, string pAnimationName , bool pCrossfade = false)
    {
        ResetLayerWeights(pAnimationLayer);



        if (pCrossfade)
        {
            animator.CrossFade(pAnimationName, .75f, pAnimationLayer, 1f);

        }
        else
        {
            animator.Play(pAnimationName, pAnimationLayer);

        }
    }

    public void GetAnimationRandomFromGroup()
    {
        currentAvailableAnimations =   AnimationController.instance.GetAnimationRandomByType(selectedAnimationtype);

        activeAnimationIndex = Random.Range(0, currentAvailableAnimations.animationNames.Count);
        activeAnimationLayer = currentAvailableAnimations.animationLayer;

        ResetLayerWeights(activeAnimationLayer);

        SetAnimationGeneral();
    }

    public void GetAnimationRandom()
    {

        currentAvailableAnimations = AnimationController.instance.GetAnimationRandomByType(selectedAnimationtype);

        activeAnimationIndex = Random.Range(0, currentAvailableAnimations.animationNames.Count);
        activeAnimationLayer = currentAvailableAnimations.animationLayer;

        ResetLayerWeights(activeAnimationLayer);

        SetAnimationGeneral();
    }

    public void GetAnimationSpecificFromGroup()
    {
        ResetLayerWeights(specificAnimationIndex);

        specifiedAnimationName =  AnimationController.instance.GetSpecificAnimation(selectedAnimationtype, specificAnimationIndex);

        //Debug.Log(" Available animations == null !!!!");
        currentAvailableAnimations = AnimationController.instance.GetAnimationRandomByType(selectedAnimationtype);


        activeAnimationIndex = specificAnimationIndex;
        activeAnimationLayer = currentAvailableAnimations.animationLayer;

        SetSpecificAnimation(activeAnimationLayer, specifiedAnimationName);
    }

    public void ResetLayerWeights(int pAnimationLayer)
    {
        animator.SetLayerWeight(0, 0f);
        animator.SetLayerWeight(1, 0f);
        animator.SetLayerWeight(2, 0f);

        animator.SetLayerWeight(pAnimationLayer, 1f);
    }

    public void RerollAnimation()
    {
       //Debug.Log(" Rerolling animation !");
        activeAnimationIndex = Random.Range(0, currentAvailableAnimations.animationNames.Count);
        activeAnimationLayer = currentAvailableAnimations.animationLayer;


        SetAnimationGeneral();
    }

}
