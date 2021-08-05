using Fralle.Core;
using System;
using System.Collections;
using UnityEngine;

namespace Fralle.PingTap
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

		public float ReloadTime => (reloadSpeed * reloadStatMultiplier);

		[Header("Debug")]
		[Readonly] public float ReloadPercentage;

		void Awake()
		{
			weapon = GetComponent<Weapon>();
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
				ReloadPercentage = rotationTime / ReloadTime;
				rotationTime = Mathf.Clamp(rotationTime + Time.deltaTime, 0, ReloadTime);
				var animTime = blendOverLifetime.Evaluate(ReloadPercentage);
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
			yield return new WaitForSeconds(ReloadTime);
			ChangeAmmo(MaxAmmo, false);
			weapon.ChangeWeaponAction(Status.Ready);
			transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));
			isReloading = false;
		}
	}
}
