using UnityEngine;

[System.Serializable]
public class Quest
{ 
    public bool isActive;
    public bool isDone;
    public string title;

    public QuestGoal goal;

    public void Complete()
    {
        isActive = false;
        isDone = true;
        Debug.Log(title+" was completed");
    }
}