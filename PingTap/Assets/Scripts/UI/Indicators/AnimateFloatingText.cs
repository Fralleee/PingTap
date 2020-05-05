using UnityEngine;

namespace Fralle.UI.Indicators
{
  public class AnimateFloatingText : MonoBehaviour
  {
    void Start()
    {
      float animationTime = Random.Range(1f, 1.5f);
      var rect = GetComponent<RectTransform>();
      var tweenObjectScale = LeanTween.Scale(gameObject, Vector3.zero, animationTime);
      var tweenObjectMove = LeanTween.Move(rect, new Vector3(transform.localPosition.x, -200f, 0), animationTime);
      tweenObjectScale.SetEase(LeanTweenType.EaseInBack);
      tweenObjectMove.SetEase(LeanTweenType.EaseInBack);
    }
  }
}