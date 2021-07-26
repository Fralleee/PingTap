using Fralle.Core;
using Fralle.Core.Extensions;
using System;
using UnityEngine;
using UnityEngine.AI;

namespace Fralle.Pingtap
{
	[Serializable]
	public class DamageDeathHandler
	{
		DamageController damageController;
		NavMeshAgent navMeshAgent;
		Animator animator;

		public void Setup(DamageController damageController)
		{
			this.damageController = damageController;
			this.damageController.OnDeath += HandleDeath;

			navMeshAgent = this.damageController.GetComponentInChildren<NavMeshAgent>();
			animator = this.damageController.GetComponentInChildren<Animator>();
		}

		public void Clean()
		{
			damageController.OnDeath -= HandleDeath;
		}

		void HandleDeath(DamageController dc, DamageData dd)
		{
			if (navMeshAgent)
				navMeshAgent.enabled = false;
			if (animator)
				animator.enabled = false;

			foreach (var disableOnDeath in dc.GetComponentsInChildren<IDisableOnDeath>())
				disableOnDeath.enabled = false;

			if (dc.gameObject.TryGetComponent(out CapsuleCollider targetCollider))
				UnityEngine.Object.Destroy(targetCollider);

			dc.gameObject.SetLayerRecursively(LayerMask.NameToLayer("Corpse"));
			UnityEngine.Object.Destroy(dc.gameObject, 3f);
		}
	}
}
