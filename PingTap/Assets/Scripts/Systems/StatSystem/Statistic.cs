namespace StatsSystem
{
	public abstract class Statistic
	{
		public string name;
		public int level;

		public virtual void Apply(Stats stats) { }
	}
}
