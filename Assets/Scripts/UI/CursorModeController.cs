using UnityEngine;

public class CursorModeController : MonoBehaviour
{
    [SerializeField] private bool startInUIMode = false;
    [SerializeField] private GameObject ui;

    public bool IsUIMode { get; private set; }

    private void Awake()
    {
        IsUIMode = startInUIMode;
        UpdateUIMode();

        var ic = InputController.Instance;
        ic.OnToggleUI += ToggleUIMode;
    }

    private void OnDestroy()
    {
        var ic = InputController.Instance;
        if (ic != null)
        {
            ic.OnToggleUI -= ToggleUIMode;
        }
    }

    private void ToggleUIMode()
    {
        IsUIMode = !IsUIMode;
        UpdateUIMode();
    }

    private void UpdateUIMode()
    {
        Cursor.lockState = IsUIMode ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = IsUIMode;
        ui.SetActive(IsUIMode);
    }
}
