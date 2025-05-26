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

    [Header("Quest Offer (Inside Dialogue Panel)")]
    public GameObject questOfferContainer;
    public TextMeshProUGUI questTitleText;
    public TextMeshProUGUI questDescText;
    public Button acceptButton;
    public Button declineButton;

    private string[] lines;
    private int currentIndex;
    private bool dialogueActive = false;

    // Quest callback delegates
    private UnityAction onQuestAccept;
    private UnityAction onQuestDecline;

    void Awake()
    {
        Instance = this;

        if (dialoguePanel != null) dialoguePanel.SetActive(false);
        if (questOfferContainer != null) questOfferContainer.SetActive(false);
    }

    public void StartDialogue(string npcName, string[] dialogueLines)
    {
        if (npcNameText == null || dialogueText == null || dialoguePanel == null)
        {
            Debug.LogError("DialogueManager: UI references are not assigned.");
            return;
        }

        npcNameText.text = npcName;
        lines = dialogueLines;
        currentIndex = 0;
        dialogueText.text = lines[currentIndex];
        dialogueActive = true;

        dialoguePanel.SetActive(true);
        UIManager.Instance.UnlockCursor();
    }

    void Update()
    {
        if (dialogueActive && Input.GetMouseButtonDown(0) && !questOfferContainer.activeSelf)
        {
            NextLine();
        }
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
            dialogueActive = false;

            if (onQuestAccept != null || onQuestDecline != null)
            {
                ShowQuestOfferUI();
            }
            else
            {
                CloseDialogue();
            }
        }
    }

    public void QueueQuestOffer(string npcName, string questTitle, string questDesc, UnityAction acceptAction, UnityAction declineAction)
    {
        if (questTitleText == null || questDescText == null)
        {
            Debug.LogError("DialogueManager: Quest offer UI references are not assigned.");
            return;
        }

        questTitleText.text = questTitle;
        questDescText.text = questDesc;
        onQuestAccept = acceptAction;
        onQuestDecline = declineAction;

        Debug.Log($"Quest from {npcName} queued: {questTitle}");
    }

    private void ShowQuestOfferUI()
    {
        if (questOfferContainer == null || acceptButton == null || declineButton == null)
        {
            Debug.LogError("DialogueManager: Quest offer UI buttons are not assigned.");
            return;
        }

        questOfferContainer.SetActive(true);
        Debug.Log("Showing quest offer panel");

        acceptButton.onClick.RemoveAllListeners();
        declineButton.onClick.RemoveAllListeners();

        acceptButton.onClick.AddListener(() =>
        {
            onQuestAccept?.Invoke();
            CloseDialogue();
        });

        declineButton.onClick.AddListener(() =>
        {
            onQuestDecline?.Invoke();
            CloseDialogue();
        });
    }

    public void CloseDialogue()
    {
        if (dialoguePanel != null) dialoguePanel.SetActive(false);
        if (questOfferContainer != null) questOfferContainer.SetActive(false);

        onQuestAccept = null;
        onQuestDecline = null;

        UIManager.Instance.LockCursor();
    }
}
