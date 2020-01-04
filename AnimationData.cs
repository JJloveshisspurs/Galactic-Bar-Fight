using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AnimationData  {

	public enum AnimationType{
			Performing,
            Dancing,
			Chatting_While_Standing,
			Chatting_While_Sitting,
			BarTending,
			Irate,
			CombatWithBlaster,
			CombatWithMelee,
			Move
		}
    public int animationLayer;
		
	public AnimationType animationGroup;
		
	public List<string> animationNames;
		
		
		}
