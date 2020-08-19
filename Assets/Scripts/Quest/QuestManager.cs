using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class QuestManager : MonoBehaviour
{

    public Quest[] quest ;
    public QuestHandler questHandler ;
    public GameObject questWindow;
    
    public Text[] titleText;

    private void Start()
    {
        for (int i = 0; i < quest.Length; i++)
        {
            titleText[i].text = quest[i].title;
        }
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
            quest[i].isActive = true;
            questHandler.quest[i] = quest[i];
        }
    }
}
