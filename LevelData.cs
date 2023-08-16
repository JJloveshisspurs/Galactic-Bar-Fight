using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LevelData 
{
    public enum ChallengeSection
    {
        Hub,
        TrainingDroids,
        Level1,
        Level2,
        Level3,
        Level4,
        Level5,
        ShootingGallery
    }

    public string levelName;

    public SavedChallengeData.ChallengeAvailability levelAvailability;

    public ChallengeSection challengeSection;
    public int easyDifficultyHighScore;
    public int mediumDifficultyHighScore;
    public int hardDifficultyHighScore;

       

    public List<SavedChallengeData> challengeList;

}
