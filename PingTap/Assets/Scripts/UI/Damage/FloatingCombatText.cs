using CombatSystem.Combat.Damage;
using TMPro;
using UnityEngine;

namespace Fralle.UI
{
	public class FloatingCombatText : MonoBehaviour
	{
		[SerializeField] GameObject combatTextPrefab;
		[SerializeField] float destroyDelay = 0.5f;
		[SerializeField] Vector2 randomPosition = new Vector2(0.5f, 0.5f);

		void Awake()
		{
			var damageController = GetComponentInParent<DamageController>();
			damageController.OnDamageTaken += HandleDamageTaken;
		}

		void HandleDamageTaken(DamageController dc, DamageData damageData)
		{
			AddFloatingCombatText(damageData);
		}

		void AddFloatingCombatText(DamageData damageData)
		{
			var instance = Instantiate(combatTextPrefab, transform.position, Quaternion.identity);
			SetPosition(instance);

			var text = instance.GetComponentInChildren<TextMeshProUGUI>();
			SetText(text, Mathf.RoundToInt(damageData.damageAmount));
			AnimateText(text.gameObject);

			Destroy(instance, destroyDelay);
		}

		void SetPosition(GameObject go)
		{
			var position = new Vector3(Random.Range(-randomPosition.x, randomPosition.x), Random.Range(0, randomPosition.y), 0);
			go.transform.position = transform.position + position;
		}

		void SetText(TextMeshProUGUI text, int number)
		{
			text.text = number.ToString();
		}

		void AnimateText(GameObject textGO)
		{
			LTDescr tweenObjectScale;
			LTDescr tweenObjectMove;
			var rectTransform = textGO.GetComponent<RectTransform>();

			tweenObjectScale = LeanTween.scale(textGO, Vector3.zero, destroyDelay);
			tweenObjectMove = LeanTween.move(rectTransform, new Vector3(0, rectTransform.position.y - 4f, 0), destroyDelay);
			tweenObjectScale.setEase(LeanTweenType.easeInBack);
			tweenObjectMove.setEase(LeanTweenType.easeInBack);
		}
	}
}
