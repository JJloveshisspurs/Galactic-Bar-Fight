using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AIBehaviorStates 
{
    //*** States for AI activities
    public enum AIBehavior{ 
    Socialize,
    MoveToPosition_ShortRange,
    MoveToPosition_LongRange,
    MovetoPosition_Melee,
    Attack_Projectile,
    Attack_Melee
    
    }

    //*** Current behavior reference
    public AIBehavior currentBehavior;
}
