using UnityEngine;

[System.Serializable]
public class Quest
{
    public string questId;
    public string title;
    
    [TextArea]
    public string description;

    public bool isAccepted = false;
    public bool isCompleted = false;

    public Quest(string id, string title, string description)
    {
        this.questId = id;
        this.title = title;
        this.description = description;
    }
}
