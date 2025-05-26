using UnityEngine;

public class QuestGiver : MonoBehaviour, IInteractable
{
    public Quest questToGive;
    public string npcName = "Mang Kanorski";

    public void Interact()
    {
        if (!questToGive.isAccepted)
        {
            DialogueManager.Instance.StartDialogue(npcName, new[]
            {
                "Hey Fin, your Lola left something important.",
                "Find her old journal in the kubo. It might be the key to everything."
            });

            DialogueManager.Instance.QueueQuestOffer(
                npcName,
                questToGive.title,
                questToGive.description,
                acceptAction: () =>
                {
                    questToGive.isAccepted = true;
                    QuestManager.Instance.AddQuest(questToGive);
                },
                declineAction: () =>
                {
                    // Player declined the quest
                }
            );
        }
        else
        {
            DialogueManager.Instance.QueueQuestOffer(
                npcName,
                questToGive.title,
                questToGive.description,
                () => {
                    questToGive.isAccepted = true;
                    QuestManager.Instance.AddQuest(questToGive);
                },
                () => {
                    // Do nothing on decline
                }
            );

        }
    }
}
