using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    [HideInInspector] public Quest[] quest ;
        
    public QuestHandler questHandler;
    public QuestGoal questGoal;
    public GameObject questWindow;
    public TMP_Text[] titleText;
    public TMP_Text[] pauseTitleText;
    public TMP_Text playerLevelText;
    public TMP_Text pausePlayerLevelText;
    private List<GoalType> _goalTypes;
    private void Start()
    {
        if (PlayerPrefs.HasKey("Task 0"))
        {
            GetQuest();
        }
        else
        {
            SetRandomQuest();
        }

        playerLevelText.text = "Level "+QuestGoal.PlayerLevel.ToString();
        pausePlayerLevelText.text = "Level "+QuestGoal.PlayerLevel.ToString();
        AcceptQuest();
        for (int i = 0; i < quest.Length; i++)
        {
            titleText[i].text = quest[i].goal.TextForQuest();
            pauseTitleText[i].text = quest[i].goal.TextForQuest();
        }

    }

    private void GetQuest()
    {
        for (int i = 0; i < quest.Length; i++)
        {
            quest[i].goal.goalType = questGoal.GetGoal(PlayerPrefs.GetInt($"Task {i}"));
            quest[i].isDone = PlayerPrefs.GetInt($"Task done {i}") == 1;
            quest[i].isActive = PlayerPrefs.GetInt($"Task active {i}") == 1;
        }
    }

    private void SaveQuest()
    {
        for (int i = 0; i < quest.Length; i++)
        {
            PlayerPrefs.SetInt($"Task {i}", (int) quest[i].goal.goalType);
            PlayerPrefs.SetInt($"Task done {i}", quest[i].isDone ? 1 : 0);
            PlayerPrefs.SetInt($"Task active {i}", quest[i].isDone ? 1 : 0);
        }
        PlayerPrefs.SetInt("PlayerLevel", QuestGoal.PlayerLevel);
        PlayerPrefs.Save();
    }
    
    public void SetRandomQuest()
    {
        _goalTypes = new List<GoalType>();
        _goalTypes.Add(questGoal.RandomGoal());
        for (int i = 0; i < quest.Length; i++)
        {
            var randGoal = questGoal.RandomGoal();
            while (_goalTypes.Contains(randGoal))
                randGoal = questGoal.RandomGoal();

            _goalTypes.Add(randGoal);
        }
        for (int i = 0; i < quest.Length; i++)
        {
            quest[i].goal.goalType = _goalTypes.ToArray()[i];
            quest[i].isActive = true;
            quest[i].isDone = false;
        }

        QuestGoal.PlayerLevel += 1;
        playerLevelText.text = "Level "+QuestGoal.PlayerLevel.ToString();
        Debug.Log(QuestGoal.PlayerLevel);
    }

    private void AcceptQuest()
    {
        for (var i = 0; i < quest.Length; i++)
        {
            questHandler.quest[i] = quest[i];
            quest[i].isActive = quest[i].isDone == false;
        }
    }

    private void OnDestroy()
    {
        SaveQuest();
    }
}
