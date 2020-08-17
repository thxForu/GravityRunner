using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestHandler : MonoBehaviour
{
    public Quest quest;

    private void Update()
    {
        if (quest.isActive&&quest.goal.IsReached())
        {
            quest.Complete();
        }
    }
}
