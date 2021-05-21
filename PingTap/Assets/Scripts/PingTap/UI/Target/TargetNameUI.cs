using CombatSystem.Combat.Damage;
using TMPro;
using UnityEngine;

namespace Fralle.UI
{
	public class TargetNameUi : MonoBehaviour
	{
		TextMeshProUGUI nameText;

		void Awake()
		{
			nameText = GetComponent<TextMeshProUGUI>();
			DamageController damageController = GetComponentInParent<DamageController>();
			nameText.text = damageController.name;
		}
	}
}
