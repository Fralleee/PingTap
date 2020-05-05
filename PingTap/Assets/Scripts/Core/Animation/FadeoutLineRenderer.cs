using UnityEngine;

namespace Fralle.Core.Animation
{
  public class FadeoutLineRenderer : MonoBehaviour
  {
    public float fadeoutTime;
    public bool destroyOnFadeout = true;

    float fadeTimer;
    LineRenderer lineRenderer;
    Color currentColor;

    void Awake()
    {
      lineRenderer = GetComponent<LineRenderer>();
      currentColor = lineRenderer.material.color;

      if (destroyOnFadeout) Destroy(gameObject, fadeoutTime);
    }

    void Update()
    {
      fadeTimer += Time.deltaTime;
      var newColor = Color.Lerp(currentColor, new Color(1f, 1f, 1f, 0f), fadeTimer / fadeoutTime);
      lineRenderer.material.color = newColor;
    }
  }
}