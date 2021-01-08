using CombatSystem.Addons;
using CombatSystem.Combat;
using CombatSystem.Effect;
using CombatSystem.Enums;
using CombatSystem.Interfaces;
using Fralle.Core.Enums;
using UnityEngine;

namespace CombatSystem.Action
{
	[RequireComponent(typeof(Weapon))]
	[RequireComponent(typeof(AmmoAddon))]
	[RequireComponent(typeof(RecoilAddon))]
	public abstract class AttackAction : MonoBehaviour, IAction
	{
		public MouseButton Button { get; set; }

		[Header("Shooting")]
		[SerializeField] internal float minDamage = 1;
		[SerializeField] internal float maxDamage = 10;
		[SerializeField] internal int ammoPerShot = 1;
		[SerializeField] internal int shotsPerSecond = 20;
		[SerializeField] internal bool tapable = false;
		[SerializeField] internal Element element = Element.Physical;
		[SerializeField] internal DamageEffect[] damageEffects = new DamageEffect[0];

		internal int hitboxLayer;
		internal Weapon weapon;
		internal Combatant attacker;
		int nextMuzzle;

		float fireRate;

		internal float Damage => Random.Range(minDamage, maxDamage);
		bool HasAmmo => weapon.ammoAddonController && weapon.ammoAddonController.HasAmmo();

		internal virtual void Awake()
		{

			hitboxLayer = LayerMask.NameToLayer("Hitbox");
			fireRate = 1f / shotsPerSecond;
		}

		internal virtual void Start()
		{
			weapon = GetComponent<Weapon>();
			attacker = weapon.GetComponentInParent<Combatant>();
		}

		//public void Perform()
		//{
		//	if (weapon.ActiveWeaponAction != Status.Ready)
		//		return;

		//	Fire();
		//	weapon.nextAvailableShot = fireRate;
		//	weapon.ammoAddonController?.ChangeAmmo(-ammoPerShot);

		//	if (weapon.recoilAddon)
		//		weapon.recoilAddon.AddRecoil();

		//	if (HasAmmo)
		//		weapon.ChangeWeaponAction(Status.Firing);
		//}

		// Multishot implementation
		public void Perform()
		{
			if (weapon.ActiveWeaponAction != Status.Ready)
				return;

			int shotsToFire = Mathf.RoundToInt(-weapon.nextAvailableShot / fireRate);
			for (int i = 0; i <= shotsToFire; i++)
			{
				Fire();
				weapon.nextAvailableShot += fireRate;
				weapon.ammoAddonController?.ChangeAmmo(-ammoPerShot);

				if (weapon.recoilAddon)
					weapon.recoilAddon.AddRecoil();

				if (!HasAmmo)
					break;
			}

			if (HasAmmo)
			{
				weapon.ChangeWeaponAction(Status.Firing);
			}
		}

		public abstract void Fire();
		public abstract float GetRange();

		internal Transform GetMuzzle()
		{
			var muzzle = weapon.muzzles[nextMuzzle];
			if (weapon.muzzles.Length <= 1)
				return muzzle;

			nextMuzzle++;
			if (nextMuzzle > weapon.muzzles.Length - 1)
				nextMuzzle = 0;

			return muzzle;
		}
	}
}
