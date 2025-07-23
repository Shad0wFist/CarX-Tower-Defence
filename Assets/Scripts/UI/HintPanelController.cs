using UnityEngine;

public class HintPanelController : MonoBehaviour
{
    [SerializeField] private GameObject hintPanel;
    [SerializeField] private bool isVisibleOnStart = true;

    private bool isVisible;

    private void Awake()
    {
        isVisible = isVisibleOnStart;
        UpdateVisibility();

        if (InputController.Instance != null)
            InputController.Instance.OnToggleHint += ToggleHint;
    }

    private void OnDestroy()
    {
        if (InputController.Instance != null)
            InputController.Instance.OnToggleHint -= ToggleHint;
    }

    private void ToggleHint()
    {
        isVisible = !isVisible;
        UpdateVisibility();
    }

    private void UpdateVisibility()
    {
        if (hintPanel != null)
            hintPanel.SetActive(isVisible);
    }
}
