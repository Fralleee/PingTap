using Fralle.Core;
using System;
using System.Linq;

namespace Fralle.Pingtap
{
	[Serializable]
	public class CombatIKHandler
	{
		public bool enabled;

		Combatant combatant;
		HandIK[] leftHandIks;
		HandIK[] rightHandIks;
		HandIKTarget[] leftHandIKTargets;
		HandIKTarget[] rightHandIKTargets;

		public void Setup(Combatant combatant)
		{
			this.combatant = combatant;
			this.combatant.OnWeaponSwitch += HandleWeaponSwitch;

			var handIKs = this.combatant.GetComponentsInChildren<HandIK>();
			var handIKTargets = this.combatant.GetComponentsInChildren<HandIKTarget>();

			leftHandIks = handIKs.Where(x => x.hand == Hand.Left).ToArray();
			rightHandIks = handIKs.Where(x => x.hand == Hand.Right).ToArray();
			leftHandIKTargets = handIKTargets.Where(x => x.hand == Hand.Left).ToArray();
			rightHandIKTargets = handIKTargets.Where(x => x.hand == Hand.Right).ToArray();

			if (combatant.EquippedWeapon != null)
				SetupIK();
			else
			{
				foreach (var toggleIK in handIKs)
					toggleIK.Toggle(false);
			}
		}

		public void Clean()
		{
			combatant.OnWeaponSwitch -= HandleWeaponSwitch;
		}

		void SetupIK()
		{
			if (combatant.EquippedWeapon.leftHandGrip)
			{
				foreach (var ik in leftHandIks)
					ik.Toggle();

				foreach (var target in leftHandIKTargets)
					target.Target(combatant.EquippedWeapon.leftHandGrip);
			}

			if (combatant.EquippedWeapon.rightHandGrip)
			{
				foreach (var ik in rightHandIks)
					ik.Toggle();

				foreach (var target in rightHandIKTargets)
					target.Target(combatant.EquippedWeapon.rightHandGrip);
			}
		}

		void HandleWeaponSwitch(Weapon newWeapon, Weapon oldWeapon)
		{
			SetupIK();
		}
	}
}
