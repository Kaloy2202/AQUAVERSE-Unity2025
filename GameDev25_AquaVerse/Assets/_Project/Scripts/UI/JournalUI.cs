using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class JournalUI : MonoBehaviour
{
    public static JournalUI Instance;

    public GameObject journalPanel;
    public TextMeshProUGUI questListText;

    private void Awake()
    {
        Instance = this;
        journalPanel.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            journalPanel.SetActive(!journalPanel.activeSelf);

            // Pause player look/move here if needed
            Cursor.lockState = journalPanel.activeSelf ? CursorLockMode.None : CursorLockMode.Locked;
            Cursor.visible = journalPanel.activeSelf;
        }
    }

    public void UpdateJournal(List<Quest> quests)
    {
        questListText.text = "";

        foreach (var quest in quests)
        {
            string status = quest.isCompleted ? "<color=green>✔</color>" : "<color=yellow>●</color>";
            questListText.text += $"{status} <b>{quest.title}</b>\n{quest.description}\n\n";
        }
    }
}
