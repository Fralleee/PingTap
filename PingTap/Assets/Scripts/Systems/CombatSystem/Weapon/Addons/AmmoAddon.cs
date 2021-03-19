using CharacterStats;
using CombatSystem.Enums;
using System;
using System.Collections;
using UnityEngine;

namespace CombatSystem.Addons
{
	public class AmmoAddon : MonoBehaviour
	{
		public event Action<int> OnAmmoChanged = delegate { };

		public int maxAmmo = 30;
		public int currentAmmo;

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
			if (stats)
			{
				stats.reloadSpeed.OnChanged += ReloadSpeed_OnChanged;
				reloadStatMultiplier = stats.reloadSpeed.Value;
			}
		}

		void Start()
		{
			currentAmmo = maxAmmo;
			OnAmmoChanged(currentAmmo);
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
			else if (Input.GetKeyDown(KeyCode.R) && weapon.ActiveWeaponAction == Status.Ready && currentAmmo < maxAmmo)
				StartCoroutine(ReloadCooldown());
		}

		public void ChangeAmmo(int change, bool apply = true)
		{
			if (apply)
				currentAmmo += change;
			else
				currentAmmo = change;
			currentAmmo = Mathf.Clamp(currentAmmo, 0, maxAmmo);
			OnAmmoChanged(currentAmmo);
		}

		public bool HasAmmo(int requiredAmmo = 1)
		{
			if (infiniteAmmo || currentAmmo >= requiredAmmo)
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
			ChangeAmmo(maxAmmo, false);
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
				stats.reloadSpeed.OnChanged -= ReloadSpeed_OnChanged;
			}
		}
	}
}
