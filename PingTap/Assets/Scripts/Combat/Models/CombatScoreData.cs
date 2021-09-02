using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;

namespace Fralle.PingTap
{
  [Serializable]
  public class CombatScoreData
  {
    [ReadOnly] public int KillingBlows;
    [ReadOnly] public float TotalDamage;
    [ReadOnly] public float TotalShotsFired;
    [ReadOnly] public float TotalShotsHit;
    [ReadOnly] public float AccuracyPercentage;

    List<DamageData> combatHistory = new List<DamageData>();

    public void OnSuccessfulAttack(DamageData damageData)
    {
      combatHistory.Add(damageData);
    }

    public void OnAttack(int shots)
    {
      TotalShotsFired += shots;
    }

    public void CalculateStats()
    {
      foreach (DamageData damageData in combatHistory)
      {
        TotalDamage += damageData.DamageAmount;
        TotalShotsHit += damageData.DamageFromHit ? 1 : 0;
        KillingBlows += damageData.KillingBlow ? 1 : 0;
      }

      AccuracyPercentage = TotalShotsHit / TotalShotsFired;
    }
  }
}
