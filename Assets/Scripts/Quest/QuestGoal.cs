using UnityEngine;
using Random = UnityEngine.Random;

[System.Serializable]
public class QuestGoal
{
    [HideInInspector] public GoalType goalType;
    
    [HideInInspector] public int coinsRequiredAmount = 15,
        metersRequiredAmount = 40,
        dodgeCometRequiredAmount = 2,
        dodgeSawRequiredAmount = 3 ;

    public static int PlayerLevel
    {
        get => PlayerPrefs.GetInt(Constans.PLAYER_LEVEL);
        set => PlayerPrefs.SetInt(Constans.PLAYER_LEVEL, value);
    }

    //return random quest from enum
    public GoalType RandomGoal()
    {
        return GetGoal(Random.Range(0,5));
    }
    
    //return quest by number
    public GoalType GetGoal(int numberGoal)
    {
        return (GoalType) numberGoal;
    }
    
    public bool IsReached()
    {
        switch (goalType)
        {
            case GoalType.CollectCoins:
                return CollectCoins(coinsRequiredAmount*PlayerLevel);
            
            case GoalType.RunMeters:
                return RunMeters(metersRequiredAmount*PlayerLevel);
            
            case GoalType.NewRecord:
                return NewRecord();
            
            case GoalType.DodgeComet:
                return DodgeComets(dodgeCometRequiredAmount*PlayerLevel);
            
            case GoalType.FlyOverSaw:
                return DodgeSaws(dodgeSawRequiredAmount*PlayerLevel);
            /*
            case GoalType.CollectUniqueCoins:
                return CollectUniqueCoins(uniqueCoinsRequiredAmount*playerLevel);
            */
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
                return $"Collect {coinsRequiredAmount*PlayerLevel} coins";
            
            case GoalType.RunMeters:
                return $"Run {metersRequiredAmount*PlayerLevel} meters";
            
            case GoalType.NewRecord:
                return "Set new record";
            
            case GoalType.DodgeComet:
                return $"Dodge {dodgeCometRequiredAmount*PlayerLevel} comets";
            
            case GoalType.FlyOverSaw:
                return $"Fly over {dodgeSawRequiredAmount*PlayerLevel} saws";
            /*
            case GoalType.CollectUniqueCoins:
                return $"Collect {uniqueCoinsRequiredAmount*playerLevel}unique coins";
            */
            default:
                Debug.LogError("no Tasks ");
                return "no task";
        }
    }
    

    public bool NewRecord()
    {
        if (PlayerPrefs.HasKey(Constans.PLAYER_HIGH_SCORE))
            return DistanceCounter.DistanceCount > PlayerPrefs.GetInt(Constans.PLAYER_HIGH_SCORE);
        else
            PlayerPrefs.SetInt(Constans.PLAYER_HIGH_SCORE,DistanceCounter.DistanceCount);
        
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
        return DodgeComet.CountDodge >= cometsForDodge;
    }
    
    public bool DodgeSaws(int sawsForDodge)
    {
        return DodgeSaw.CountDodge >= sawsForDodge;
    }
}
public enum GoalType
{
    NewRecord,
    CollectCoins,
    RunMeters,
    DodgeComet,
    FlyOverSaw,
    //CollectUniqueCoins
}

