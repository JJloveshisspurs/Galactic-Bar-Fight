using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Challanges")]
public class Challenges : ScriptableObject
{
    [SerializeField] List<Objective> objectives = new List<Objective>();
    [SerializeField] List<Reward> rewards = new List<Reward>();

    [System.Serializable]
    public class Objective
    {
        public string reference;
        public string description;
    }

    [System.Serializable]
    public class Reward
    {
        public int number;
        public GameObject item;
    }

    public IEnumerable<Objective> GetObjectives()
    {
        return objectives;
    }

    public IEnumerable<Reward> GetRewards()
    {
        return rewards;
    }

    public string GetTitle()
    {
        return name;
    }

    public int GetObjectiveCount()
    {
        return objectives.Count;
    }

    public bool HasObjective(string objectiveRef)
    {
        foreach(Objective objective in objectives)
        {
            if(objective.reference == objectiveRef)
            {
                return true;
            }
        }

        return false;
    }
}
