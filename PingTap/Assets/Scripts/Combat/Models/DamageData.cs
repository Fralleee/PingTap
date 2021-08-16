using UnityEngine;

namespace Fralle.PingTap
{
	public class DamageData
	{
		public Combatant Attacker;
		public DamageController Victim;
		public GameObject ImpactEffect;
		public Collider Collider;

		public HitArea HitArea;
		public Element Element;
		public DamageEffect[] Effects;

		public Vector3 Position;
		public Vector3 Direction;
		public Vector3 Normal;
		public Vector3 Force;

		public float HitAngle;
		public float DamageAmount;

		public bool KillingBlow;
		public bool Gib;
		public bool DamageFromHit = true;

		public DamageData()
		{
			Effects = new DamageEffect[0];
		}
	}
}
