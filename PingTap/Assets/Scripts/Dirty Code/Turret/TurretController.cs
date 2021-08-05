using Fralle.Core.Extensions;
using Fralle.PingTap;
using UnityEngine;

namespace Fralle.Abilities.Turret
{
	public class TurretController : MonoBehaviour
	{
		public DamageController Target;
		public Transform AimRig;

		public bool IsDeployed;

		public float TargetHeight;
		public float Range = 25f;

		void Update()
		{
			RotateTowardsTarget();
		}

		void RotateTowardsTarget()
		{
			if (!Target)
				return;

			Vector3 targetPosition = Target.transform.position.With(y: TargetHeight);
			Vector3 direction = (targetPosition - AimRig.position).normalized;
			Quaternion lookRotation = Quaternion.LookRotation(direction);
			AimRig.rotation = Quaternion.Slerp(AimRig.rotation, lookRotation, Time.deltaTime * 6f);
		}

	}
}
