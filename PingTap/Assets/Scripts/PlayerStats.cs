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

  [Header("Hitbox")]
  [Readonly] public int nerveHits;
  [Readonly] public int majorHits;
  [Readonly] public int minorHits;

  public void ReceiveDamageStats(Damage damage)
  {
    totalDamage += damage.damageAmount;
    switch (damage.hitBoxType)
    {
      case HitBoxType.Nerve:
        nerveHits++;
        break;
      case HitBoxType.Major:
        majorHits++;
        break;
      case HitBoxType.Minor:
        minorHits++;
        break;
      default:
        throw new ArgumentOutOfRangeException();
    }
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
