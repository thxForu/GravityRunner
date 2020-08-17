using UnityEngine;
using UnityEngine.Serialization;


[System.Serializable]
public class Quest
{ 
    public bool isActive;
    public string title;

    public QuestGoal goal;

    public void Complete()
    {
        isActive = false;
        Debug.Log(title+" was completed");
    }
}
