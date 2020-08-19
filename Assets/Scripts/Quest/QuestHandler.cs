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
        foreach (var t in quest)
            if (t.isActive && t.goal.IsReached())
                t.Complete();
    }
}
