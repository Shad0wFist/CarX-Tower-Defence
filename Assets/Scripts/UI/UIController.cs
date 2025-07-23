using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIController : MonoBehaviour
{
    [SerializeField] private CannonTower tower;
    [SerializeField] private CannonRotator rotator;

    [SerializeField] private Slider shootIntervalSlider;

    [SerializeField] private Slider rotationSpeedSlider;

    [SerializeField] private Slider elevationSpeedSlider;

    [SerializeField] private TMP_Dropdown targetingModeDropdown;

    private void Start()
    {
        shootIntervalSlider.value = tower.ShootInterval;

        rotationSpeedSlider.value = rotator.RotationSpeed;
        elevationSpeedSlider.value = rotator.ElevationSpeed;

        targetingModeDropdown.ClearOptions();
        var names = Enum.GetNames(typeof(TargetingMode))
                        .Select(n => new TMP_Dropdown.OptionData(n))
                        .ToList();
        targetingModeDropdown.AddOptions(names);
        targetingModeDropdown.value = (int)tower.TargetingMode;
        targetingModeDropdown.RefreshShownValue();

        shootIntervalSlider.onValueChanged.AddListener(OnShootIntervalChanged);
        rotationSpeedSlider.onValueChanged.AddListener(OnRotationSpeedChanged);
        elevationSpeedSlider.onValueChanged.AddListener(OnElevationSpeedChanged);
        targetingModeDropdown.onValueChanged.AddListener(OnTargetingModeChanged);
    }

    private void OnShootIntervalChanged(float v)
    {
        tower.ShootInterval = v;
    }

    private void OnRotationSpeedChanged(float v)
    {
        rotator.RotationSpeed = v;
    }

    private void OnElevationSpeedChanged(float v)
    {
        rotator.ElevationSpeed = v;
    }

    private void OnTargetingModeChanged(int idx)
    {
        tower.TargetingMode = (TargetingMode)idx;
    }
}
