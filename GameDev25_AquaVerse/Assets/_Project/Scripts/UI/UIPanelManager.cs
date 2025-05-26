using UnityEngine;
using System.Collections.Generic;

public class UIPanelManager : MonoBehaviour
{
    public static UIPanelManager Instance;

    private Dictionary<string, GameObject> panels = new Dictionary<string, GameObject>();
    private string currentPanel = "";

    public FirstPersonController player;

    void Awake()
    {
        Instance = this;
    }

    public void RegisterPanel(string name, GameObject panel)
    {
        if (!panels.ContainsKey(name))
        {
            panels.Add(name, panel);
            panel.SetActive(false);
        }
    }

    public void ShowPanel(string name)
    {
        foreach (var kvp in panels)
        {
            kvp.Value.SetActive(false);
        }

        if (panels.ContainsKey(name))
        {
            panels[name].SetActive(true);
            currentPanel = name;

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            if (player != null)
                player.canMove = false;
        }
    }

    public void HideCurrentPanel()
    {
        if (currentPanel != "")
        {
            panels[currentPanel].SetActive(false);
            currentPanel = "";

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            if (player != null)
                player.canMove = true;
        }
    }

    public bool IsAnyPanelOpen()
    {
        return currentPanel != "";
    }
}
