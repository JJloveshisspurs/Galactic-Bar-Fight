using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveFileUpdated 
{
    [SerializeField]
    public int lifeTimeScore;

    [SerializeField]
    public int lifeTimeKills;

    [SerializeField]
    public int powerupDrinksConsumed;

    [SerializeField]
    public int lifetimeRoundscompleted;

    [SerializeField]
    public bool completedIntroTutorial;


    [SerializeField]
    public int lifeTimeDroidsDestroyed;

    [SerializeField]
    public int lifeTimeShootingGalleryScoreAcquired;

    [SerializeField]
    public float timeSpentWatchingCommercials;


    [SerializeField]
    public float timeSpentWatchingStageEntertainment;

    [SerializeField]
    public int lifeTimeDiscoBallHits;

    [SerializeField]
    public bool enteredFirstCombatArena;

  

    [SerializeField]
    public List<LevelData> levels;
}
