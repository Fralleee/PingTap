namespace CombatSystem.Enums
{
	public enum HitArea
	{
		MINOR,
		MAJOR,
		NERVE
	}

	public static class HitAreaMethods
	{
		public static float GetMultiplier(this HitArea ha)
		{
			switch (ha)
			{
				case HitArea.MINOR:
					return 0.5f;
				case HitArea.MAJOR:
					return 1.0f;
				case HitArea.NERVE:
					return 2.0f;
				default:
					return 1.0f;
			}
		}
	}
}
