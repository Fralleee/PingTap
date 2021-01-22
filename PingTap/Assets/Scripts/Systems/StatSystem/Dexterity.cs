using UnityEngine;

namespace StatsSystem
{
	public class Dexterity : Statistic
	{
		public Dexterity()
		{
			name = "Dexterity";
		}

		public float reloadSpeedMultiplier => Mathf.Pow(0.9f, level);
		public float meleeRecoveryMultiplier => Mathf.Pow(0.9f, level);

		public override void Apply(Stats stats)
		{
			stats.reloadSpeedMultiplier = reloadSpeedMultiplier;
			stats.meleeRecoveryMultiplier = meleeRecoveryMultiplier;
		}
	}
}
