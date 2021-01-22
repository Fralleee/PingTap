using CombatSystem.Combat;
using CombatSystem.Effect;
using CombatSystem.Enums;
using System;
using UnityEngine;

namespace CombatSystem.Offense
{
	[Serializable]
	public class ProjectileData
	{
		public float force = 100f;
		public bool useGravity;
		public bool kinematicOnImpact;
		public bool explodeOnMaxRange;

		public float explodeOnImpactTime;
		public float explodeOnTime;
		public float explosionRadius;
		public float pushForce;
		public float range;

		[HideInInspector] public Combatant attacker;
		[HideInInspector] public float damage;
		[HideInInspector] public Vector3 forward;
		[HideInInspector] public Element element;
		[HideInInspector] public DamageEffect[] damageEffects;
		[HideInInspector] public int hitboxLayer;
	}
}
