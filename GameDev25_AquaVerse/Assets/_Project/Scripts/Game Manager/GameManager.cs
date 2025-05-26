using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("UI Panels")]
    public GameObject dialoguePanel;
    public GameObject journalPanel;
    public GameObject shopPanel;

    [Header("References")]
    public FirstPersonController player;

    private void Awake()
    {
        // Singleton pattern
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        // Register all panels to the UIPanelManager
        UIPanelManager.Instance.RegisterPanel("Dialogue", dialoguePanel);
        UIPanelManager.Instance.RegisterPanel("Journal", journalPanel);
        UIPanelManager.Instance.RegisterPanel("Shop", shopPanel);

        // Provide player reference
        UIPanelManager.Instance.player = player;
    }

    void Update()
    {
        // Example: Close panel with Escape
        if (Input.GetKeyDown(KeyCode.Escape) && UIPanelManager.Instance.IsAnyPanelOpen())
        {
            UIPanelManager.Instance.HideCurrentPanel();
        }

        // Example: Toggle Journal with Tab (if no other panel is open)
        if (Input.GetKeyDown(KeyCode.Tab) && !UIPanelManager.Instance.IsAnyPanelOpen())
        {
            UIPanelManager.Instance.ShowPanel("Journal");
        }
    }
}
