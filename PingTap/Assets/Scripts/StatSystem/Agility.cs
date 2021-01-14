namespace StatsSystem
{
	public class Agility : Statistic
	{
		public Agility()
		{
			name = "Agility";
		}

		public float runSpeedMultiplier => 1 + 0.1f * level / (1 + 0.1f * level);
		public float jumpPowerMultiplier => 1 + 0.1f * level / (1 + 0.1f * level);

		public override void Apply(Stats stats)
		{
			stats.runSpeedMultiplier = runSpeedMultiplier;
			stats.jumpPowerMultiplier = jumpPowerMultiplier;
		}
	}
}
