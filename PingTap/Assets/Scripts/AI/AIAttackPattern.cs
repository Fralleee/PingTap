using System;
using UnityEngine;

namespace Fralle.Pingtap
{
	[Serializable]
	public class AIAttackPattern
	{
		public Vector2 TimeBetweenAttacks = new Vector2(0.125f, 0.25f);
		public Vector2 BulletsPerAttack = Vector2.one;

		public AIAttackPattern(Vector2 timeBetweenAttacks, Vector2 bulletsPerAttack)
		{
			TimeBetweenAttacks = timeBetweenAttacks;
			BulletsPerAttack = bulletsPerAttack;
		}
	}
}
