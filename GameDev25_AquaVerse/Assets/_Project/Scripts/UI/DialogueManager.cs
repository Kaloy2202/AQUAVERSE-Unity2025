using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;

    public GameObject dialoguePanel;
    public TextMeshProUGUI npcNameText;
    public TextMeshProUGUI dialogueText;
    public Button continueButton;

    private string[] lines;
    private int currentIndex;

    void Awake()
    {
        Instance = this;
        dialoguePanel.SetActive(false);
        continueButton.onClick.AddListener(NextLine);
    }

    public void StartDialogue(string npcName, string[] dialogueLines)
    {
        npcNameText.text = npcName;
        lines = dialogueLines;
        currentIndex = 0;
        dialogueText.text = lines[currentIndex];
        dialoguePanel.SetActive(true);
    }

    private void NextLine()
    {
        currentIndex++;
        if (currentIndex < lines.Length)
        {
            dialogueText.text = lines[currentIndex];
        }
        else
        {
            dialoguePanel.SetActive(false);
        }
    }
}
