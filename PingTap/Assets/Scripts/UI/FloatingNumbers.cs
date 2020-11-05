using CombatSystem.Combat.Damage;
using TMPro;
using UnityEngine;

namespace Fralle.UI
{
	public class FloatingNumbers : MonoBehaviour
	{
		[SerializeField] TextMeshProUGUI text;
		[SerializeField] float hideDelay = 0.5f;
		[SerializeField] Vector2 randomPosition = new Vector2(0.25f, 0.25f);

		RectTransform rectTransform;
		DamageController damageController;
		LTDescr tweenObjectScale;
		LTDescr tweenObjectMove;

		Vector3 defaultPosition;
		Vector3 defaultScale;

		float hideTimer;

		void Awake()
		{
			rectTransform = GetComponent<RectTransform>();
			damageController = GetComponentInParent<DamageController>();
			damageController.OnDamageTaken += HandleDamageTaken;

			defaultPosition = transform.localPosition;
			defaultScale = transform.localScale;

			text.gameObject.SetActive(false);
		}

		void Update()
		{
			if (hideTimer > 0)
			{
				hideTimer -= Time.deltaTime;
				if (hideTimer <= 0)
					Hide();
			}
		}

		void Hide()
		{
			text.gameObject.SetActive(false);
		}

		void Show()
		{
			text.gameObject.SetActive(true);
		}

		void HandleDamageTaken(DamageController dc, DamageData damageData)
		{
			SetFloatingNumbers(Mathf.RoundToInt(damageData.damageAmount));
		}

		void SetFloatingNumbers(int number)
		{
			var actualNumber = number;
			if (hideTimer > 0)
				actualNumber += int.Parse(text.text);

			hideTimer = hideDelay;
			text.text = actualNumber.ToString();

			transform.localPosition = defaultPosition + new Vector3(Random.Range(-randomPosition.x, randomPosition.x), Random.Range(-randomPosition.y, randomPosition.y), 0);
			transform.localScale = defaultScale;
			Show();
			Animate();
		}

		void Animate()
		{

			if (tweenObjectScale != null)
				tweenObjectScale.reset();
			if (tweenObjectMove != null)
				tweenObjectMove.reset();

			tweenObjectScale = LeanTween.scale(gameObject, Vector3.zero, hideDelay);
			tweenObjectMove = LeanTween.move(rectTransform, new Vector3(transform.localPosition.x, -1.5f, 0), hideDelay);
			tweenObjectScale.setEase(LeanTweenType.easeInBack);
			tweenObjectMove.setEase(LeanTweenType.easeInBack);
		}

	}
}
