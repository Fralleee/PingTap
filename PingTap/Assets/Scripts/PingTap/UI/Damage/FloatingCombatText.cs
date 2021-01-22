using CombatSystem.Combat.Damage;
using Fralle.Core.Pooling;
using TMPro;
using UnityEngine;

namespace Fralle.UI
{
	public class FloatingCombatText : MonoBehaviour
	{
		[SerializeField] GameObject combatTextPrefab;
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
			var instance = ObjectPool.Spawn(combatTextPrefab, transform.position, Quaternion.identity);

			SetPosition(instance);

			var text = instance.GetComponentInChildren<TextMeshPro>();
			SetText(text, Mathf.RoundToInt(damageData.damageAmount));
		}

		void SetPosition(GameObject go)
		{
			var position = new Vector3(Random.Range(-randomPosition.x, randomPosition.x), Random.Range(0, randomPosition.y), 0);
			go.transform.position = transform.position + position;
		}

		void SetText(TextMeshPro text, int number)
		{
			text.text = number.ToString();
		}
	}
}
