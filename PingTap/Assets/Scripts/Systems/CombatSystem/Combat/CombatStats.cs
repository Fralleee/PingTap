using CombatSystem.Combat.Damage;
using Fralle.Core.Attributes;
using System;
using System.Collections.Generic;

namespace CombatSystem.Combat
{
  [Serializable]
  public class CombatStats
  {
    [Readonly] public int killingBlows;
    [Readonly] public float totalDamage;
    [Readonly] public float totalShotsFired;
    [Readonly] public float totalShotsHit;
    [Readonly] public float accuracyPercentage;

    List<DamageData> combatHistory = new List<DamageData>();

    public void OnSuccessfulAttack(DamageData damageData)
    {
      combatHistory.Add(damageData);
    }

    public void OnAttack(int shots)
    {
      totalShotsFired += shots;
    }

    public void CalculateStats()
    {
      foreach (var damageData in combatHistory)
      {
        totalDamage += damageData.damageAmount;
        totalShotsHit += damageData.damageFromHit ? 1 : 0;
        killingBlows += damageData.killingBlow ? 1 : 0;
      }

      accuracyPercentage = totalShotsHit / totalShotsFired;
    }
  }
}