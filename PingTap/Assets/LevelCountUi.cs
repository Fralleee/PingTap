using TMPro;
using UnityEngine;

public class LevelCountUi : MonoBehaviour
{
  TextMeshProUGUI text;

  void Awake()
  {
    text = GetComponent<TextMeshProUGUI>();
    WaveManager.OnNewSchema += SetText;
  }

  void SetText(WaveManager waveManager)
  {
    text.text = $"Level {waveManager.currentArmy + 1}";
  }
}
