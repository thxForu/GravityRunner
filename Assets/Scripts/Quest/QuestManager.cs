using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class QuestManager : MonoBehaviour
{

    public Quest quest;
    public QuestHandler questHandler;
    public GameObject questWindow;
    
    public Text titleText;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Q))
            OpenQuestWindow();
        if(Input.GetKeyDown(KeyCode.A))
            AcceptQuest();

    }

    public void OpenQuestWindow()
    {
        questWindow.SetActive(true);
        titleText.text = quest.title;
    }

    public void AcceptQuest()
    {
        quest.isActive = true;
        questHandler.quest = quest;
    }
}
