using EPOOutline;
using System;
using UnityEngine;

namespace Fralle.Pingtap
{
	[Serializable]
	public class DamageUIHandler
	{
		[Header("UI")]
		[SerializeField] GameObject healthbarPrefab;
		[SerializeField] GameObject floatingCombatText;

		DamageController damageController;
		GameObject ui;
		Outlinable outlinable;

		float delay = 0.5f;
		float timer;
		bool isShowing;

		public void Setup(DamageController damageController)
		{
			this.damageController = damageController;
			this.damageController.OnReceiveAttack += HandleReceiveAttack;

			outlinable = this.damageController.GetComponentInChildren<Outlinable>();
			if (outlinable)
				outlinable.enabled = false;

			ui = this.damageController.transform.Find("UI").gameObject;
			if (ui)
			{
				ui.SetActive(false);
				SetupUi();
			}
		}

		public void Timer()
		{
			if (!isShowing)
				return;

			timer -= Time.deltaTime;

			if (timer <= 0)
				Toggle(false);
		}

		public void Clean()
		{
			damageController.OnReceiveAttack -= HandleReceiveAttack;
		}

		public void Toggle(bool show, float? customDelay = null)
		{
			timer = customDelay.HasValue ? customDelay.Value : delay;

			if (ui)
				ui.SetActive(show);

			if (outlinable)
				outlinable.enabled = show;

			isShowing = show;
		}

		void SetupUi()
		{
			if (healthbarPrefab)
				GameObject.Instantiate(healthbarPrefab, ui.transform);
			if (floatingCombatText)
				GameObject.Instantiate(floatingCombatText, ui.transform);
		}

		void HandleReceiveAttack(DamageController dc, DamageData dd)
		{
			if (dd.Attacker.hasActiveCamera)
				Toggle(true, 1f);
		}
	}
}
