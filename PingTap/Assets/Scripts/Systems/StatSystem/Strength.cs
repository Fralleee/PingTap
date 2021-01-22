namespace StatsSystem
{
	public class Strength : Statistic
	{
		public Strength()
		{
			name = "Strength";
		}

		public float meleePowerMultiplier => 1 + 0.1f * level / (1 + 0.1f * level);

		public override void Apply(Stats stats)
		{
			stats.meleePowerMultiplier = meleePowerMultiplier;
		}
	}
}
