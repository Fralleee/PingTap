using UnityEngine;

namespace Fralle.UI.Indicators
{
  public class AnimateFloatingText : MonoBehaviour
  {
    void Start()
    {
      var animationTime = Random.Range(1f, 1.5f);
      var rect = GetComponent<RectTransform>();
      var tweenObjectScale = LeanTween.scale(gameObject, Vector3.zero, animationTime);
      var tweenObjectMove = LeanTween.move(rect, new Vector3(transform.localPosition.x, -200f, 0), animationTime);
      tweenObjectScale.setEase(LeanTweenType.easeInBack);
      tweenObjectMove.setEase(LeanTweenType.easeInBack);
    }
  }
}