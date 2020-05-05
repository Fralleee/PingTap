using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SliderSetting : MonoBehaviour
{
  [Header("General")]
  [SerializeField] Settings setting;
  [SerializeField] TMP_InputField input;
  [SerializeField] Slider slider;

  [Header("Clamp values")]
  [SerializeField] float Min = 0.01f;
  [SerializeField] float Max = 100f;

  string key;

  void Awake()
  {
    key = setting.ToString();

    slider.minValue = Min;
    slider.maxValue = Max;

    input.contentType = TMP_InputField.ContentType.DecimalNumber;

    if (slider)
    {
      slider.value = PlayerPrefs.GetFloat(key);
      slider.onValueChanged.AddListener(SliderValueChanged);
    }

    if (input)
    {
      input.text = PlayerPrefs.GetFloat(key).ToString("##.##");
      input.onValueChanged.AddListener(InputValueChanged);
    }
  }

  void SliderValueChanged(float value)
  {
    PlayerPrefs.SetFloat(key, value);

    if (!input) return;
    input.onValueChanged.RemoveAllListeners();
    input.text = value.ToString("##.##");
    input.onValueChanged.AddListener(InputValueChanged);
  }

  void InputValueChanged(string value)
  {
    float floatValue = float.Parse(value);
    floatValue = Mathf.Clamp(floatValue, Min, Max);

    PlayerPrefs.SetFloat(key, floatValue);

    if (!slider) return;
    slider.onValueChanged.RemoveAllListeners();
    slider.value = floatValue;
    slider.onValueChanged.AddListener(SliderValueChanged);
  }
}
