using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("References")]
    public FirstPersonController player;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        if (player != null)
            player.canMove = true;
    }

    public void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        if (player != null)
            player.canMove = false;
    }

    public void ShowPanel(GameObject panel)
    {
        panel.SetActive(true);
        UnlockCursor();
    }

    public void HidePanel(GameObject panel)
    {
        panel.SetActive(false);
        LockCursor();
    }

    public void TogglePanel(GameObject panel)
    {
        bool isActive = panel.activeSelf;
        panel.SetActive(!isActive);

        if (isActive)
            LockCursor();
        else
            UnlockCursor();
    }
}
