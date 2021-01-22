using UnityEngine;

namespace StatsSystem
{
	public class Aim : Statistic
	{
		public Aim()
		{
			name = "Aim";
		}

		public float recoilReduction => Mathf.Pow(0.9f, level);
		public float spreadReduction => Mathf.Pow(0.9f, level);

		public override void Apply(Stats stats)
		{
			stats.recoilReduction = recoilReduction;
			stats.spreadReduction = spreadReduction;
		}
	}
}
