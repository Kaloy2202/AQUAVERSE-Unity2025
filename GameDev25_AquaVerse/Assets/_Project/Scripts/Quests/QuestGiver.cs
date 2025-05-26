using UnityEngine;

public class QuestGiver : MonoBehaviour, IInteractable
{
    public Quest questToGive;
    public string npcName = "Mang Kanorski";

    public void Interact()
    {
        if (!questToGive.isAccepted)
        {
            DialogueManager.Instance.ShowQuestOffer(
                npcName,
                questToGive.title,
                questToGive.description,
                () =>
                {
                    // On Accept
                    questToGive.isAccepted = true;
                    QuestManager.Instance.AddQuest(questToGive);
                    DialogueManager.Instance.CloseDialogue();
                },
                () =>
                {
                    // On Decline
                    DialogueManager.Instance.CloseDialogue();
                }
            );
        }
        else
        {
            DialogueManager.Instance.StartDialogue(npcName, new[] {
                "You're already working on that quest!",
                "Check your journal for updates."
            });
        }
    }
}
