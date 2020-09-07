using System;
using UnityEngine;
using UnityEngine.UI;

public class QuestHandler : MonoBehaviour
{
    [HideInInspector] public Quest[] quest = new Quest[3];
    
    public Image[] taskImages;
    public Image[] pauseTaskImages;
    public Image[] starsImages;
    public Image completeTaskImage;
    public Image completeStartImage;


    private void Start()
    {
        Invoke(nameof(QuestCheck),1f);
    }

    public void QuestCheck()
    {

        for (var i = 0; i < quest.Length; i++)
        {
            if (quest[i].isActive && quest[i].goal.IsReached())
            {
                quest[i].Complete();
            }
            if (quest[i].isDone)
            {
                starsImages[i].sprite = completeStartImage.sprite;
                taskImages[i].sprite = completeTaskImage.sprite;
                pauseTaskImages[i].sprite = completeTaskImage.sprite;
            }
        }
    }

    public bool CheckAll()
    {
        var completeAll = 0;
        foreach (var q in quest)
            if (q.isDone)
                completeAll++;

        if (completeAll == 3)
            return true;
        
        return false;
    }
}
