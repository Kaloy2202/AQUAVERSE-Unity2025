using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Events;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;

    [Header("Dialogue Panel")]
    public GameObject dialoguePanel;
    public TextMeshProUGUI npcNameText;
    public TextMeshProUGUI dialogueText;
    public Button continueButton;

    [Header("Quest Offer Panel")]
    public GameObject questOfferPanel;
    public TextMeshProUGUI questTitleText;
    public TextMeshProUGUI questDescText;
    public Button acceptButton;
    public Button declineButton;

    private string[] lines;
    private int currentIndex;

    void Awake()
    {
        Instance = this;

        dialoguePanel.SetActive(false);
        questOfferPanel.SetActive(false);

        continueButton.onClick.AddListener(NextLine);
    }

    public void StartDialogue(string npcName, string[] dialogueLines)
    {
        npcNameText.text = npcName;
        lines = dialogueLines;
        currentIndex = 0;
        dialogueText.text = lines[currentIndex];

        UIPanelManager.Instance.ShowPanel("Dialogue");
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
            CloseDialogue();
        }
    }

    public void ShowQuestOffer(string npcName, string questTitle, string questDesc, UnityAction onAccept, UnityAction onDecline)
    {
        npcNameText.text = npcName;
        questTitleText.text = questTitle;
        questDescText.text = questDesc;

        acceptButton.onClick.RemoveAllListeners();
        declineButton.onClick.RemoveAllListeners();

        acceptButton.onClick.AddListener(onAccept);
        declineButton.onClick.AddListener(onDecline);

        dialoguePanel.SetActive(false);
        questOfferPanel.SetActive(true);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        FindObjectOfType<FirstPersonController>().canMove = false;
    }

    public void CloseDialogue()
    {
        dialoguePanel.SetActive(false);
        questOfferPanel.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        FindObjectOfType<FirstPersonController>().canMove = true;
    }
}
