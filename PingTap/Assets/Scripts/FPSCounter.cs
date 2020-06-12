using TMPro;
using UnityEngine;

public class FPSCounter : MonoBehaviour
{
  [SerializeField] float updateFrequency = 0.5f;

  TextMeshProUGUI text;
  int avgFrameRate;
  float internalTimer;

  void Awake()
  {
    text = GetComponent<TextMeshProUGUI>();
  }

  void Update()
  {
    internalTimer += Time.deltaTime;
    if (!(internalTimer > updateFrequency)) return;

    float current = (int)(1f / Time.unscaledDeltaTime);
    avgFrameRate = (int)current;
    text.text = $"{avgFrameRate} FPS";
    internalTimer = 0;
  }
}