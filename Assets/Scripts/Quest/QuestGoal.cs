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
        return GetGoal(Random.Range(0,5));
    }

    public GoalType GetGoal(int numberGoal)
    {
        return (GoalType) numberGoal;
    }
    
    public bool IsReached()
    {
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
            
            case GoalType.CollectUniqueCoins:
                return true;
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
                return $"Collect {requiredAmount} coins";
            
            case GoalType.RunMeters:
                return $"Run {requiredAmount} meters";
            
            case GoalType.NewRecord:
                return "Set new record";
            
            case GoalType.DodgeComet:
                return $"Dodge {requiredAmount} comets";
            
            case GoalType.FlyOverSaw:
                return $"Fly over {requiredAmount} saws";
            
            case GoalType.CollectUniqueCoins:
                return $"Collect {requiredAmount}unique coins";
            
            default:
                Debug.LogError("no Tasks ");
                return "no task";
        }
    }
    

    public bool NewRecord()
    {
        if (PlayerPrefs.HasKey("HighScore"))
            return DistanceCounter.DistanceCount > PlayerPrefs.GetInt("HighScore");
        else
            PlayerPrefs.SetInt("HighScore",DistanceCounter.DistanceCount);
        
        return false;
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
    CollectUniqueCoins
}

