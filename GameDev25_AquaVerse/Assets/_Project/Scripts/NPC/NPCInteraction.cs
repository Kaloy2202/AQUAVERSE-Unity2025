using UnityEngine;

public class NPCInteraction : MonoBehaviour, IInteractable
{
    public string npcName = "Mang Kanorski";
    public string[] dialogueLines;

    public void Interact()
    {
        DialogueManager.Instance.StartDialogue(npcName, dialogueLines);
    }
}
