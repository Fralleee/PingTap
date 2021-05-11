using CombatSystem.Action;
using CombatSystem.Combat.Damage;
using Fralle.Core.Basics;
using Fralle.Core.Extensions;
using System;
using System.Linq;
using UnityEngine;

namespace CombatSystem.Combat
{
	public class Combatant : MonoBehaviour
	{
		public event Action<Weapon, Weapon> OnWeaponSwitch = delegate { };
		public event Action<DamageData> OnHit = delegate { };

		public CombatStats Stats = new CombatStats();
		public CombatModifiers Modifiers = new CombatModifiers();

		public Transform AimTransform;
		public Transform WeaponHolder;
		public Weapon EquippedWeapon;

		[Header("Left Hand IK")]
		[SerializeField] FollowTransform leftHandIkTarget;
		[SerializeField] ToggleIK leftHandToggleIK;
		[SerializeField] FollowTransform fpsLeftHandIkTarget;
		[SerializeField] ToggleIK fpsLeftHandToggleIK;

		[Header("Right Hand IK")]
		[SerializeField] FollowTransform rightHandIkTarget;
		[SerializeField] ToggleIK rightHandToggleIK;
		[SerializeField] FollowTransform fpsRightHandIkTarget;
		[SerializeField] ToggleIK fpsRightHandToggleIK;

		AttackAction primaryAction;
		AttackAction secondaryAction;

		public void PrimaryAction(bool keyDown = false)
		{
			if (!EquippedWeapon || !primaryAction || primaryAction.Tapable && !keyDown)
				return;

			primaryAction.Perform();
		}

		public void SecondaryAction(bool keyDown = false)
		{
			if (!EquippedWeapon || !secondaryAction || secondaryAction.Tapable && !keyDown)
				return;

			secondaryAction.Perform();
		}

		public void SetFpsLayers(string layerName)
		{
			var layer = LayerMask.NameToLayer(layerName);
			EquippedWeapon.gameObject.SetLayerRecursively(layer);
		}

		void Awake()
		{
			SetDefaults();
		}

		void SetDefaults()
		{
			if (AimTransform == null)
				AimTransform = transform;
			if (WeaponHolder == null)
				WeaponHolder = transform;
		}


		public void ClearWeapons()
		{
			string[] stringArray = { "Weapon Camera", "FPS" };
			foreach (Transform child in WeaponHolder)
			{
				if (!stringArray.Any(child.name.Contains))
					DestroyImmediate(child.gameObject);
			}

			if (EquippedWeapon)
				EquippedWeapon = null;
		}

		public void EquipWeapon(Weapon weapon, bool animationDistance = true)
		{
			if (EquippedWeapon != null && weapon != null && EquippedWeapon.name == weapon.name)
				return;

			ClearWeapons();

			var oldWeapon = EquippedWeapon;
			var position = animationDistance ? WeaponHolder.position.With(y: -0.15f) : WeaponHolder.position;

			if (weapon != null)
			{
				EquippedWeapon = Instantiate(weapon, position, WeaponHolder.rotation, WeaponHolder);
				EquippedWeapon.Equip(this);

				SetupAttackActions();
				SetupIK();
			}
			else
			{
				EquippedWeapon = null;
				primaryAction = null;
				secondaryAction = null;
				rightHandToggleIK.Toggle(false);
				leftHandToggleIK.Toggle(false);
				fpsRightHandToggleIK?.Toggle(false);
				fpsLeftHandToggleIK?.Toggle(false);
			}

			OnWeaponSwitch(EquippedWeapon, oldWeapon);
		}

		void SetupAttackActions()
		{
			var attackActions = EquippedWeapon.GetComponentsInChildren<AttackAction>();
			if (attackActions.Length > 2)
				Debug.LogWarning($"Weapon {EquippedWeapon} has more attack actions than possible (2).");
			else if (attackActions.Length > 0)
			{
				primaryAction = attackActions[0];
				secondaryAction = attackActions.Length == 2 ? attackActions[1] : null;
			}
		}

		void SetupIK()
		{
			if (EquippedWeapon.rightHandGrip)
			{
				rightHandIkTarget.transformToFollow = EquippedWeapon.rightHandGrip;
				rightHandToggleIK.Toggle();
				if (fpsRightHandIkTarget && fpsRightHandToggleIK)
				{
					fpsRightHandIkTarget.transformToFollow = EquippedWeapon.rightHandGrip;
					fpsRightHandToggleIK.Toggle();
				}
			}

			if (EquippedWeapon.leftHandGrip)
			{
				leftHandIkTarget.transformToFollow = EquippedWeapon.leftHandGrip;
				leftHandToggleIK.Toggle();
				if (fpsLeftHandIkTarget && fpsLeftHandToggleIK)
				{
					fpsLeftHandIkTarget.transformToFollow = EquippedWeapon.leftHandGrip;
					fpsLeftHandToggleIK?.Toggle();
				}
			}
		}

		public void SuccessfulHit(DamageData damageData)
		{
			OnHit(damageData);
		}
	}
}
