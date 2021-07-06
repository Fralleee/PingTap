using UnityEngine;

namespace Fralle.PingTap.AI
{
	public enum AIDifficulty
	{
		Easy,
		Normal,
		Hard,
		Impossible
	}

	public static class AIDifficultyMethods
	{
		public static float GetAccuracy(this AIDifficulty difficulty)
		{
			switch (difficulty)
			{
				case AIDifficulty.Easy:
					return 0.175f;
				case AIDifficulty.Normal:
					return 0.125f;
				case AIDifficulty.Hard:
					return 0.05f;
				case AIDifficulty.Impossible:
					return 0.025f;
				default:
					return 1f;
			}
		}

		public static Vector3 GetAimOffset(this AIDifficulty difficulty)
		{
			switch (difficulty)
			{
				case AIDifficulty.Easy:
					return Vector3.up * 0.35f;
				case AIDifficulty.Normal:
					return Vector3.up * 0.75f;
				case AIDifficulty.Hard:
					return Vector3.up * 1.35f;
				case AIDifficulty.Impossible:
					return Vector3.up * 1.6f;
				default:
					return Vector3.zero;
			}
		}
	}
}
