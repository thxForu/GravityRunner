using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class QuestGoal 
{
    
    public GoalType GoalType;
    public int RequiredAmount;

    
    public bool IsReached()
    {
        switch (GoalType)
        {
            case GoalType.CollectCoins:
                return CollectCoins(3);
            
            case GoalType.RunMeters:
                Debug.Log("adasds");
                return RunMeters(50);
            case GoalType.NewRecord:
                return NewRecord();
            default:
                Debug.Log("no Tasks ");
                return false;
        }
    }

    public bool NewRecord()
    {
        return DistanceCounter.DistanceCount > PlayerPrefs.GetInt("HighScore");
    }

    public bool CollectCoins(int collectCoinsForTask)
    {
        return MoneyManager.Coins >= collectCoinsForTask;
    }

    public bool RunMeters(int runMetersForTask)
    {
        Debug.Log(DistanceCounter.DistanceCount);
        return DistanceCounter.DistanceCount >= runMetersForTask;
    }
}
public enum GoalType
{
    NewRecord,
    CollectCoins,
    RunMeters,
    DodgeComet,
    FlyOverSaw,
    JumpOverFaults
}

