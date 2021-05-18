using CombatSystem.Enums;
using Fralle.CharacterStats;
using System;
using System.Collections;
using UnityEngine;

namespace CombatSystem.Addons
{
	public class AmmoAddon : MonoBehaviour
	{
		public event Action<int> OnAmmoChanged = delegate { };

		public int MaxAmmo = 30;
		public int CurrentAmmo;

		[SerializeField] bool infiniteAmmo = false;
		[SerializeField] float reloadSpeed = 0.75f;
		[SerializeField] AnimationCurve blendOverLifetime = new AnimationCurve();

		bool isReloading;
		float rotationTime;
		float reloadStatMultiplier = 1f;
		Weapon weapon;
		StatsController stats;

		void Awake()
		{
			weapon = GetComponent<Weapon>();

			stats = weapon.GetComponentInParent<StatsController>();
			if (!stats) return;
			stats.ReloadSpeed.OnChanged += ReloadSpeed_OnChanged;
			reloadStatMultiplier = stats.ReloadSpeed.Value;
		}

		void Start()
		{
			CurrentAmmo = MaxAmmo;
			OnAmmoChanged(CurrentAmmo);
		}

		void Update()
		{
			if (infiniteAmmo)
				return;
			if (isReloading)
			{
				var agePercent = rotationTime / (reloadSpeed * reloadStatMultiplier);
				rotationTime = Mathf.Clamp(rotationTime + Time.deltaTime, 0, reloadSpeed * reloadStatMultiplier);
				var animTime = blendOverLifetime.Evaluate(agePercent);
				var spinDelta = -(Mathf.Cos(Mathf.PI * animTime) - 1f) / 2f;
				transform.localRotation = Quaternion.Euler(new Vector3(spinDelta * 360f, 0, 0));
			}
			else if (Input.GetKeyDown(KeyCode.R) && weapon.ActiveWeaponAction == Status.Ready && CurrentAmmo < MaxAmmo)
				StartCoroutine(ReloadCooldown());
		}

		public void ChangeAmmo(int change, bool apply = true)
		{
			if (apply)
				CurrentAmmo += change;
			else
				CurrentAmmo = change;
			CurrentAmmo = Mathf.Clamp(CurrentAmmo, 0, MaxAmmo);
			OnAmmoChanged(CurrentAmmo);
		}

		public bool HasAmmo(int requiredAmmo = 1)
		{
			if (infiniteAmmo || CurrentAmmo >= requiredAmmo)
				return true;
			StartCoroutine(ReloadCooldown());
			return false;
		}

		IEnumerator ReloadCooldown()
		{
			weapon.ChangeWeaponAction(Status.Reloading);
			isReloading = true;
			rotationTime = 0f;
			yield return new WaitForSeconds(reloadSpeed * reloadStatMultiplier);
			ChangeAmmo(MaxAmmo, false);
			weapon.ChangeWeaponAction(Status.Ready);
			transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));
			isReloading = false;
		}

		void ReloadSpeed_OnChanged(CharacterStat stat)
		{
			reloadStatMultiplier = stat.Value;
		}

		void OnDestroy()
		{
			if (stats)
			{
				stats.ReloadSpeed.OnChanged -= ReloadSpeed_OnChanged;
			}
		}
	}
}
