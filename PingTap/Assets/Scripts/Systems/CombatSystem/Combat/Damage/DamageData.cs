using CombatSystem.Effect;
using CombatSystem.Enums;
using UnityEngine;

namespace CombatSystem.Combat.Damage
{
	public class DamageData
	{
		public Combatant attacker;
		public DamageController victim;
		public GameObject impactEffect;

		public HitArea hitArea;
		public Element element;
		public DamageEffect[] effects;

		public Vector3 position;
		public Vector3 force;

		public float hitAngle;
		public float damageAmount;

		public bool killingBlow;
		public bool gib;
		public bool damageFromHit = true;

		public DamageData()
		{
			effects = new DamageEffect[0];
		}
	}
}
