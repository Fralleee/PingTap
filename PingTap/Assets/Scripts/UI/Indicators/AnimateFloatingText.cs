using UnityEngine;

namespace Fralle
{
  public class AnimateFloatingText : MonoBehaviour
  {
    void Start()
    {
      float animationTime = Random.Range(1f, 1.5f);
      var rect = GetComponent<RectTransform>();
      LTDescr tweenObjectScale = LeanTween.scale(gameObject, Vector3.zero, animationTime);
      LTDescr tweenObjectMove = LeanTween.move(rect, new Vector3(transform.localPosition.x, -200f, 0), animationTime);
      tweenObjectScale.setEase(LeanTweenType.easeInBack);
      tweenObjectMove.setEase(LeanTweenType.easeInBack);
    }
  }
}