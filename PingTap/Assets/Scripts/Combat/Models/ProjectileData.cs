using System;
using UnityEngine;

namespace Fralle.PingTap
{
	[Serializable]
	public class ProjectileData
	{
		public float Force = 100f;
		public bool UseGravity;
		public bool KinematicOnImpact;
		public bool ExplodeOnMaxRange;

		public float ExplodeOnImpactTime;
		public float ExplodeOnTime;
		public float ExplosionRadius;
		public float PushForce;
		[Range(0, 1000f)] public float Range = 100f;

		[HideInInspector] public Combatant Attacker;
		[HideInInspector] public float Damage;
		[HideInInspector] public Vector3 Forward;
		[HideInInspector] public Element Element;
		[HideInInspector] public DamageEffect[] DamageEffects;
		[HideInInspector] public int HitboxLayer;
	}
}
