using System;
using UnityEngine;
using UnityEngine.UI;

public class QuestHandler : MonoBehaviour{
    
    [HideInInspector]public Quest[] quest = new Quest[3];
    
    public Image[] taskImages;
    public Image completeTaskImage;

    
    
    public void QuestCheck()
    {
        for (var i = 0; i < quest.Length; i++)
        {
            if (quest[i].isActive && quest[i].goal.IsReached())
            {
                quest[i].Complete();
                taskImages[i].sprite = completeTaskImage.sprite;
            }   
        }
    }
}
