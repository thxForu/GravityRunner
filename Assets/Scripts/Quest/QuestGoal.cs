using UnityEngine;
using Random = UnityEngine.Random;

[System.Serializable]
public class QuestGoal
{
    public GoalType goalType;
    public int requiredAmount;
    public int playerLevel;
    public int count;
    public GoalType RandomGoal()
    {
        var random = (GoalType) Random.Range(0,5);
        return random;
    }
    
    public bool IsReached()
    {
        RandomGoal();
        switch (goalType)
        {
            case GoalType.CollectCoins:
                return CollectCoins(requiredAmount*playerLevel);
            
            case GoalType.RunMeters:
                return RunMeters(requiredAmount*playerLevel);
            
            case GoalType.NewRecord:
                return NewRecord();
            
            case GoalType.DodgeComet:
                return DodgeComets(requiredAmount*playerLevel);
            
            case GoalType.FlyOverSaw:
                return DodgeSaws(requiredAmount*playerLevel);
            
            default:
                Debug.LogError("no Tasks ");
                return false;
        }
    }
    public string TextForQuest()
    {
        switch (goalType)
        {
            case GoalType.CollectCoins:
                return $"Collect {count} coins";
            
            case GoalType.RunMeters:
                return $"Run {count} meters";
            
            case GoalType.NewRecord:
                return "Set new record";
            
            case GoalType.DodgeComet:
                return $"Dodge {count} comets";
            
            case GoalType.FlyOverSaw:
                return $"Fly over {count} saws";
            default:
                Debug.LogError("no Tasks ");
                return "no task";
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
        return DistanceCounter.DistanceCount >= runMetersForTask;
    }

    public bool DodgeComets(int cometsForDodge)
    {
        return DodgeComet.cometDodge >= cometsForDodge;
    }
    public bool DodgeSaws(int sawsForDodge)
    {
        return DodgeSaw.sawDodge >= sawsForDodge;
    }
}
public enum GoalType
{
    NewRecord,
    CollectCoins,
    RunMeters,
    DodgeComet,
    FlyOverSaw,
}

