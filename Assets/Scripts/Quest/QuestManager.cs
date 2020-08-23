
using TMPro;
using UnityEngine;


public class QuestManager : MonoBehaviour
{
    public Quest[] quest ;
    public QuestHandler questHandler;
    public QuestGoal QuestGoal;
    public GameObject questWindow;
    public TextMeshProUGUI[] titleText;
    
    
    private void Start()
    {
        RandomQuest();
        AcceptQuest();
        for (int i = 0; i < quest.Length; i++)
            titleText[i].text = quest[i].goal.TextForQuest();
    }

    public void RandomQuest()
    {
        for (int i = 0; i < quest.Length; i++)
        {
            Debug.Log(quest[i].goal.goalType);
            quest[i].goal.goalType = QuestGoal.RandomGoal();
            
            Debug.Log(quest[i].goal.goalType);
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
            if (!quest[i].isDone)
                quest[i].isActive = true;
            
            questHandler.quest[i] = quest[i];
        }
    }

    
}
