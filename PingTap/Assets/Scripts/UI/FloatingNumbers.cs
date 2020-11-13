using CombatSystem.Combat.Damage;
using UnityEngine;

namespace Fralle.UI
{
	public class FloatingNumbers : MonoBehaviour
	{
		[SerializeField] GameObject floatingNumberPrefab;

		void Awake()
		{
			var damageController = GetComponentInParent<DamageController>();
			damageController.OnDamageTaken += HandleDamageTaken;
		}

		void HandleDamageTaken(DamageController dc, DamageData damageData)
		{
			SetFloatingNumbers(Mathf.RoundToInt(damageData.damageAmount));
		}

		void SetFloatingNumbers(int number)
		{
			var instance = Instantiate(floatingNumberPrefab, transform.position, Quaternion.identity);
			instance.GetComponentInChildren<FloatingNumbersNumber>().SetText(number);
		}
	}
}
