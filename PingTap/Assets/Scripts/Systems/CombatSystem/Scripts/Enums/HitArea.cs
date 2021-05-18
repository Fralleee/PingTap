using CombatSystem.Combat.Damage;
using UnityEngine;

namespace CombatSystem.Enums
{
	public enum HitArea
	{
		Minor,
		Major,
		Nerve
	}

	public static class HitAreaMethods
	{
		public static float GetMultiplier(this HitArea ha)
		{
			switch (ha)
			{
				case HitArea.Minor:
					return 0.5f;
				case HitArea.Major:
					return 1.0f;
				case HitArea.Nerve:
					return 2.0f;
				default:
					return 1.0f;
			}
		}

		public static GameObject GetImpactEffect(this HitArea ha, DamageController damageController)
		{
			switch (ha)
			{
				case HitArea.Major:
					return damageController.MajorImpactEffect;
				case HitArea.Nerve:
					return damageController.NerveImpactEffect;
				default:
					return damageController.MajorImpactEffect;
			}
		}
	}
}
