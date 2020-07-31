using TMPro;
using UnityEngine;

namespace Fralle.UI.HUD
{
  public class LevelCountUi : MonoBehaviour
  {
    TextMeshProUGUI text;

    void Awake()
    {
      text = GetComponent<TextMeshProUGUI>();
      //WaveManager.OnNewSchema += SetText;
    }

    void SetText()
    {
      //text.text = $"Level {WaveManager.Instance.currentArmy + 1}";
    }
  }
}