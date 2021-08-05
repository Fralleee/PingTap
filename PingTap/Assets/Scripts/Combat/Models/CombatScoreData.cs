﻿using Fralle.Core;
using System;
using System.Collections.Generic;

namespace Fralle.PingTap
{
	[Serializable]
	public class CombatScoreData
	{
		[Readonly] public int KillingBlows;
		[Readonly] public float TotalDamage;
		[Readonly] public float TotalShotsFired;
		[Readonly] public float TotalShotsHit;
		[Readonly] public float AccuracyPercentage;

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
			foreach (var damageData in combatHistory)
			{
				TotalDamage += damageData.DamageAmount;
				TotalShotsHit += damageData.DamageFromHit ? 1 : 0;
				KillingBlows += damageData.KillingBlow ? 1 : 0;
			}

			AccuracyPercentage = TotalShotsHit / TotalShotsFired;
		}
	}
}
