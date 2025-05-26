using UnityEngine;

public class NPCInteraction : MonoBehaviour, IInteractable
{
    public string npcName = "Mang Kanorski";
    public string[] dialogueLines;

    public void Interact()
    {
        DialogueManager.Instance.StartDialogue(npcName, dialogueLines);
        TriggerQuest();
    }

    void TriggerQuest()
    {
        var quest = new Quest(
            "quest_001", // ✅ questId
            "Feed the Fish", // ✅ title
            "Buy feed from the shop and feed the fish in Pond A." // ✅ description
        );

        QuestManager.Instance.AddQuest(quest);
    }
}
