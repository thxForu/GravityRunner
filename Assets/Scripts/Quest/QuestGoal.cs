using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class QuestGoal 
{
    private int _currentDistance = DistanceCounter.DistanceCount;
    
    public GoalType GoalType;
    public int RequiredAmount;

    public bool IsReached()
    {
        return MoneyManager.Coins >= 20;
    }

    public bool NewRecord()
    {
        return _currentDistance > PlayerPrefs.GetInt("HighScore");
    }

    public bool CollectCoins(int collectCoinsForTask)
    {
        return MoneyManager.Coins >= collectCoinsForTask;
    }

    public bool RunMeters(int runMetersForTask)
    {
        return _currentDistance > runMetersForTask;
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

