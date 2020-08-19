using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestHandler : MonoBehaviour
{
    public Quest[] quest = new Quest[3] ;

    private void Start()
    {
        Debug.Log(quest.Length);
    }

    public void QuestCheck()
    {
        for (int i = 0; i < quest.Length; i++)
        {
            if (quest[i].isActive && quest[i].goal.IsReached())
            {
                quest[i].Complete();
            }
        }
    }
}
