using UnityEngine;

namespace Fralle
{
  public class FloatingText : MonoBehaviour
  {
    [SerializeField] float destroyTime = 1f;

    [SerializeField] Vector2 randomPosition = new Vector2(75f, 25f);

    void Start()
    {
      Destroy(gameObject, destroyTime);

      transform.localPosition += new Vector3(Random.Range(-randomPosition.x, randomPosition.x), Random.Range(-randomPosition.y, randomPosition.y), 0);

      var rect = GetComponent<RectTransform>();
      
      LTDescr tweenObjectScale = LeanTween.scale(gameObject, Vector3.zero, destroyTime);
      LTDescr tweenObjectMove = LeanTween.move(rect, new Vector3(transform.localPosition.x, -100f, 0), destroyTime);

      tweenObjectScale.setEase(LeanTweenType.easeInBack);
      tweenObjectMove.setEase(LeanTweenType.easeInBack);
    }
  }
}