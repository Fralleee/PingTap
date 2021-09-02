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
    public static float GetAccuracy(this AIDifficulty difficulty) =>
      difficulty switch
      {
        AIDifficulty.Easy => 0.175f,
        AIDifficulty.Normal => 0.125f,
        AIDifficulty.Hard => 0.05f,
        AIDifficulty.Impossible => 0.025f,
        _ => 1f
      };

    public static Vector3 GetAimOffset(this AIDifficulty difficulty) =>
      difficulty switch
      {
        AIDifficulty.Easy => Vector3.up * 0.35f,
        AIDifficulty.Normal => Vector3.up * 0.75f,
        AIDifficulty.Hard => Vector3.up * 1.35f,
        AIDifficulty.Impossible => Vector3.up * 1.6f,
        _ => Vector3.zero
      };
  }
}
