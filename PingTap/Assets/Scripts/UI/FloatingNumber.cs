using TMPro;
using UnityEngine;

namespace Fralle.UI
{
	public class FloatingNumber : MonoBehaviour
	{
		[SerializeField] float destroyDelay = 0.5f;
		[SerializeField] Vector2 randomPosition = new Vector2(0.25f, 0.25f);

		TextMeshProUGUI text;
		RectTransform rectTransform;
		LTDescr tweenObjectScale;
		LTDescr tweenObjectMove;

		public void SetText(int damage)
		{

			text.text = damage.ToString();
		}

		void Awake()
		{
			rectTransform = GetComponent<RectTransform>();
			text = GetComponent<TextMeshProUGUI>();

			transform.localPosition = new Vector3(Random.Range(-randomPosition.x, randomPosition.x), Random.Range(0, randomPosition.y), 0);
			Animate();
		}

		void Animate()
		{

			if (tweenObjectScale != null)
				tweenObjectScale.reset();
			if (tweenObjectMove != null)
				tweenObjectMove.reset();

			tweenObjectScale = LeanTween.scale(gameObject, Vector3.zero, destroyDelay);
			tweenObjectMove = LeanTween.move(rectTransform, new Vector3(transform.localPosition.x, transform.localPosition.y - 1.5f, 0), destroyDelay);
			tweenObjectScale.setEase(LeanTweenType.easeInBack);
			tweenObjectMove.setEase(LeanTweenType.easeInBack);
		}
	}
}
