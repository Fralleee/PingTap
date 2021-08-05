using Fralle.PingTap;
using TMPro;
using UnityEngine;

namespace Fralle.UI
{
	public class TargetNameUI : MonoBehaviour
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
