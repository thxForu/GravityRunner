﻿using TMPro;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    [HideInInspector] public Quest[] quest ;
    public QuestHandler questHandler;
    public QuestGoal QuestGoal;
    public GameObject questWindow;
    public TextMeshProUGUI[] titleText;
    
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
        
        AcceptQuest();
        for (int i = 0; i < quest.Length; i++)
            titleText[i].text = quest[i].goal.TextForQuest();
    }

    public void GetQuest()
    {
        for (int i = 0; i < quest.Length; i++)
        {
            quest[i].goal.goalType = QuestGoal.GetGoal(PlayerPrefs.GetInt($"Task {i}"));
            quest[i].isDone = PlayerPrefs.GetInt($"Task done {i}") == 1;
            quest[i].isActive = PlayerPrefs.GetInt($"Task active {i}") == 1;   
        }
    }

    public void SaveQuest()
    {
        for (int i = 0; i < quest.Length; i++)
        {
            PlayerPrefs.SetInt($"Task {i}",(int)quest[i].goal.goalType);
            PlayerPrefs.SetInt($"Task done {i}",quest[i].isDone?1:0);
            PlayerPrefs.SetInt($"Task active {i}",quest[i].isDone?1:0);
        }
        
        PlayerPrefs.SetInt("PlayerLevel", QuestGoal.PlayerLevel);
        PlayerPrefs.Save();
    }
    
    public void SetRandomQuest()
    {
        foreach (var q in quest)
        {
            q.goal.goalType = QuestGoal.RandomGoal();
            q.isActive = true;
            q.isDone = false;
            
        }

        QuestGoal.PlayerLevel = QuestGoal.PlayerLevel + 1;
        Debug.Log(QuestGoal.PlayerLevel);
    }
    
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Q))
            OpenQuestWindow();
        if(Input.GetKeyDown(KeyCode.A))
            AcceptQuest();
        if (Input.GetKeyDown(KeyCode.K))
            questHandler.QuestCheck();
    }

    public void OpenQuestWindow()
    {
        questWindow.SetActive(true);
    }

    public void AcceptQuest()
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
