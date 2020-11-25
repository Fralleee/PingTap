using CombatSystem.Action;
using Fralle.Core.Extensions;
using System;
using UnityEngine;

namespace CombatSystem.Combat
{
	public class Combatant : MonoBehaviour
	{
		public event Action<Weapon> OnWeaponSwitch = delegate { };

		public CombatStats Stats = new CombatStats();
		public CombatModifiers Modifiers = new CombatModifiers();

		public Transform aimTransform;
		public Transform weaponHolder;
		public Weapon weapon;

		AttackAction primaryAction;
		AttackAction secondaryAction;

		public void PrimaryAction(bool keyDown = false)
		{
			if (!weapon || !primaryAction || primaryAction.tapable && !keyDown)
				return;

			primaryAction.Perform();
		}

		public void SecondaryAction(bool keyDown = false)
		{
			if (!weapon || !secondaryAction || secondaryAction.tapable && !keyDown)
				return;

			secondaryAction.Perform();
		}

		public void SetFPSLayers(string layerName)
		{
			var layer = LayerMask.NameToLayer(layerName);
			weapon.gameObject.SetLayerRecursively(layer);
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

		public void SetupWeapon(Weapon w)
		{
			OnWeaponSwitch(w);
			weapon = w;
			var attackActions = weapon.GetComponentsInChildren<AttackAction>();
			if (attackActions.Length > 2)
				Debug.LogWarning($"Weapon {weapon} has more attack actions than possible (2).");
			else if (attackActions.Length > 0)
			{
				primaryAction = attackActions[0];
				secondaryAction = attackActions.Length == 2 ? attackActions[1] : null;
			}
		}
	}
}
