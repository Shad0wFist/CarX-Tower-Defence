using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SliderValueTextUpdater : MonoBehaviour
{
    [SerializeField] private TMP_Text valueLabel;
    [SerializeField] private Slider slider;
    [SerializeField] private string numberFormat = "0.##";

    private void Awake()
    {
        slider.onValueChanged.AddListener(OnSliderValueChanged);
    }

    private void Start()
    {
        OnSliderValueChanged(slider.value);
    }

    private void OnSliderValueChanged(float value)
    {
        if (valueLabel != null)
            valueLabel.text = value.ToString(numberFormat);
    }

    private void OnDestroy()
    {
        slider.onValueChanged.RemoveListener(OnSliderValueChanged);
    }
}

