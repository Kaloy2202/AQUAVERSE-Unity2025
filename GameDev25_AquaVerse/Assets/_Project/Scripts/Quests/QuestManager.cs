using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance;

    public List<Quest> activeQuests = new List<Quest>();

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void AddQuest(Quest quest)
    {
        activeQuests.Add(quest);
        Debug.Log($"New Quest: {quest.title}");
        JournalUI.Instance.UpdateJournal(activeQuests);
    }

    public void CompleteQuest(string title)
    {
        var quest = activeQuests.Find(q => q.title == title);
        if (quest != null)
        {
            quest.isCompleted = true;
            Debug.Log($"Quest Completed: {title}");
            JournalUI.Instance.UpdateJournal(activeQuests);
        }
    }
}
