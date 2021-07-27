using Fralle.Core;
using System;
using UnityEngine;

namespace Fralle.Pingtap
{
	[Serializable]
	public class CombatTargetHandler
	{
		TeamController teamController;
		Transform transform;

		LayerMask defaultLayer;

		public void Setup(Combatant combatant)
		{
			teamController = combatant.teamController;
			transform = combatant.AimTransform;

			defaultLayer = LayerMask.NameToLayer("Default");
		}

		public void DetectTargets()
		{
			if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hitInfo, 100f, 1 << defaultLayer | teamController.Hostiles | teamController.Neutrals))
			{
				DamageController damageController = hitInfo.transform.gameObject.GetComponent<DamageController>();
				if (damageController)
					damageController.RaycastHit();
			}
		}
	}
}