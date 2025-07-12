using UnityEngine;
using UnityEngine.UI;

public class HealthBarController : MonoBehaviour
{
    [SerializeField] private Image slideBar = null;
    private IHealth healthSource;
    private Camera mainCamera;

    private void Awake()
    {
        mainCamera = Camera.main;
    }

    private void LateUpdate()
    {
        transform.rotation = 
        Quaternion.LookRotation(-mainCamera.transform.forward, Vector3.up);
    }

    public void Bind(IHealth health)
    {
        if (healthSource != null)
        {
            healthSource.OnHealthChanged -= UpdateBar;
        }

        healthSource = health;
        slideBar.fillAmount = (float)healthSource.CurrentHP / healthSource.MaxHP;

        healthSource.OnHealthChanged += UpdateBar;
    }

    private void UpdateBar(int current, int max)
    {
        slideBar.fillAmount = (float)current / max;
    }

    private void OnDestroy()
    {
        if (healthSource != null)
        {
            healthSource.OnHealthChanged -= UpdateBar;
        }
    }
}
