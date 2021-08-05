using Fralle.PingTap;
using UnityEngine;
using UnityEngine.UI;

namespace Fralle.UI
{
	public class HealthbarChange : MonoBehaviour
	{
		[SerializeField] Image actualHealthbar;
		[SerializeField] Image changeHealthbar;

		[SerializeField] float changeSpeed = 0.75f;
		[SerializeField] float delay = 1f;

		DamageController damageController;

		float delayTimer;

		void Awake()
		{
			damageController = GetComponentInParent<DamageController>();
			damageController.OnHealthChange += HandleDamageControllerChange;
		}

		void Update()
		{
			delayTimer -= Time.deltaTime;
			if (!(delayTimer < 0))
				return;
			if (actualHealthbar.fillAmount < changeHealthbar.fillAmount)
				changeHealthbar.fillAmount -= changeSpeed * Time.deltaTime;
		}

		void HandleDamageControllerChange(float currentHealth, float maxHealth)
		{
			delayTimer = delay;
		}

		void OnDestroy()
		{
			damageController.OnHealthChange -= HandleDamageControllerChange;
		}
	}
}
