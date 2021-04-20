using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Fralle.UI.Menu
{
  public class SliderSetting : MonoBehaviour
  {
    [Header("General")]
    [SerializeField] Settings setting = Settings.None;
    [SerializeField] TMP_InputField input = null;
    [SerializeField] Slider slider = null;

    [FormerlySerializedAs("Min")]
    [Header("Clamp values")]
    [SerializeField] float min = 0.01f;

    [FormerlySerializedAs("Max")]
    [SerializeField] float max = 100f;

    string key;

    void Awake()
    {
      key = setting.ToString();

      slider.minValue = min;
      slider.maxValue = max;

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
      floatValue = Mathf.Clamp(floatValue, min, max);

      PlayerPrefs.SetFloat(key, floatValue);

      if (!slider) return;
      slider.onValueChanged.RemoveAllListeners();
      slider.value = floatValue;
      slider.onValueChanged.AddListener(SliderValueChanged);
    }
  }
}
