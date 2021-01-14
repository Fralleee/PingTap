namespace StatsSystem
{
	public class Stamina : Statistic
	{
		public Stamina()
		{
			name = "Stamina";
		}

		public float healthMultiplier => 1 + 0.1f * level / (1 + 0.1f * level);
		public float regenerationMultiplier => 1 + 0.1f * level / (1 + 0.1f * level);

		public override void Apply(Stats stats)
		{
			stats.healthMultiplier = healthMultiplier;
			stats.regenerationMultiplier = regenerationMultiplier;
		}
	}
}
