using CombatSystem.Combat.Damage;
using UnityEngine;

namespace Fralle.UI
{
	public class FloatingNumberSpawner : MonoBehaviour
	{
		[SerializeField] GameObject prefab;

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
			var instance = Instantiate(prefab, transform.position, Quaternion.identity);
			instance.GetComponentInChildren<FloatingNumber>().SetText(number);
		}
	}
}
