using CombatSystem.Action;
using CombatSystem.Combat.Damage;
using Fralle.Core.Extensions;
using System;
using UnityEngine;

namespace CombatSystem.Combat
{
	public class Combatant : MonoBehaviour
	{
		public event Action<Weapon, Weapon> OnWeaponSwitch = delegate { };
		public event Action<DamageData> OnHit = delegate { };

		public CombatStats Stats = new CombatStats();
		public CombatModifiers Modifiers = new CombatModifiers();

		public Transform aimTransform;
		public Transform weaponHolder;
		public Weapon equippedWeapon;

		AttackAction primaryAction;
		AttackAction secondaryAction;

		public void PrimaryAction(bool keyDown = false)
		{
			if (!equippedWeapon || !primaryAction || primaryAction.tapable && !keyDown)
				return;

			primaryAction.Perform();
		}

		public void SecondaryAction(bool keyDown = false)
		{
			if (!equippedWeapon || !secondaryAction || secondaryAction.tapable && !keyDown)
				return;

			secondaryAction.Perform();
		}

		public void SetFPSLayers(string layerName)
		{
			var layer = LayerMask.NameToLayer(layerName);
			equippedWeapon.gameObject.SetLayerRecursively(layer);
		}

		void Awake()
		{
			SetDefaults();
		}

		void SetDefaults()
		{
			if (aimTransform == null)
				aimTransform = transform;
			if (weaponHolder == null)
				weaponHolder = transform;
		}

		public void ClearWeapons()
		{
			foreach (Transform child in weaponHolder)
			{
				if (child.name != "Weapon Camera")
					DestroyImmediate(child.gameObject);
			}

			equippedWeapon = null;
		}

		public void EquipWeapon(Weapon weapon, bool animationDistance = true)
		{
			if (equippedWeapon != null && equippedWeapon.name == weapon.name)
				return;

			ClearWeapons();

			var position = animationDistance ? weaponHolder.position.With(y: -0.5f) : weaponHolder.position;
			equippedWeapon = Instantiate(weapon, position, weaponHolder.rotation, weaponHolder);

			equippedWeapon.Equip(this);
			SetupWeapon(equippedWeapon);
		}

		void SetupWeapon(Weapon weapon)
		{
			OnWeaponSwitch(weapon, equippedWeapon);
			equippedWeapon = weapon;
			var attackActions = equippedWeapon.GetComponentsInChildren<AttackAction>();
			if (attackActions.Length > 2)
				Debug.LogWarning($"Weapon {equippedWeapon} has more attack actions than possible (2).");
			else if (attackActions.Length > 0)
			{
				primaryAction = attackActions[0];
				secondaryAction = attackActions.Length == 2 ? attackActions[1] : null;
			}
		}

		public void SuccessfulHit(DamageData damageData)
		{
			OnHit(damageData);
		}
	}
}
