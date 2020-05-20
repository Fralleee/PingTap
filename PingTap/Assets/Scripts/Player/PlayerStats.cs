using Fralle.Attack.Offense;
using Fralle.Core.Attributes;
using System;
using UnityEngine;

[Serializable]
public class PlayerStats
{
  [Header("General")]
  [Readonly] public int killingBlows;
  [Readonly] public float totalDamage;
  [Readonly] public float totalShotsFired;
  [Readonly] public float totalShotsHit;
  [Readonly] public float accuracyPercentage;

  public void ReceiveDamageStats(Damage damage)
  {
    totalDamage += damage.damageAmount;
  }

  public void ReceiveShotsFired(int shots)
  {
    totalShotsFired += shots;
  }

  public void ReceiveHits(int hits)
  {
    totalShotsHit += hits;
    accuracyPercentage = totalShotsHit / totalShotsFired;
  }

  public void ReceiveKillingBlow(int killCount)
  {
    killingBlows += killCount;
  }
}
