using UnityEngine;
using UnityEngine.Rendering.Universal.Glitch;

public class GlitchController : MonoBehaviour
{
  [SerializeField] DigitalGlitchFeature digitalGlitchFeature;
  [SerializeField] AnalogGlitchFeature analogGlitchFeature;

  [Header("Digital")]
  [SerializeField, Range(0f, 1f)] float intensity;

  [Header("Analog")]
  [SerializeField, Range(0f, 1f)] float scanLineJitter;
  [SerializeField, Range(0f, 1f)] float verticalJump;
  [SerializeField, Range(0f, 1f)] float horizontalShake;
  [SerializeField, Range(0f, 1f)] float colorDrift;

  void Update()
  {
    SetRandomValues();

    digitalGlitchFeature.Intensity = intensity;

    analogGlitchFeature.ScanLineJitter = scanLineJitter;
    analogGlitchFeature.VerticalJump = verticalJump;
    analogGlitchFeature.HorizontalShake = horizontalShake;
    analogGlitchFeature.ColorDrift = colorDrift;
  }

  void SetRandomValues()
  {
    intensity = ClampValue(intensity);
    scanLineJitter = ClampValue(scanLineJitter);
    verticalJump = ClampValue(verticalJump);
    horizontalShake = ClampValue(horizontalShake);
    colorDrift = ClampValue(colorDrift);
  }

  float ClampValue(float currentValue, float increment = 0.01f) => Mathf.Clamp(currentValue + Random.Range(-increment, increment), 0, 1);
}
