using CombatSystem.Combat.Damage;
using Fralle.CharacterStats;
using Fralle.Core.Attributes;
using Fralle.Core.Extensions;
using Fralle.Core.Pooling;
using UnityEngine;

namespace CombatSystem.Action
{
	public class RaycastAttack : AttackAction
	{
		[Header("RaycastAttack")]
		public float Range = 50;
		public float PushForce = 3.5f;
		[SerializeField] GameObject impactParticlePrefab;
		[SerializeField] GameObject muzzleParticlePrefab;
		[SerializeField] GameObject lineRendererPrefab;
		[SerializeField] int bulletsPerFire = 1;
		public AnimationCurve RangeDamageFalloff = new AnimationCurve(new Keyframe(0, 1), new Keyframe(1, 0));

		[Header("Spread")]
		[SerializeField] float spreadRadius = 0f;
		[SerializeField] float spreadIncreaseEachShot = 0f;
		[SerializeField] float recovery = 1f;

		[Readonly] public float CurrentSpread;

		float spreadStatMultiplier = 1f;

		internal override void Start()
		{
			base.Start();

			if (Stats)
				spreadStatMultiplier = Stats.Aim.Value;
		}

		void Update()
		{
			if (CurrentSpread.EqualsWithTolerance(0f))
				return;

			CurrentSpread -= Time.deltaTime * recovery;
			CurrentSpread = Mathf.Clamp(CurrentSpread, 0, spreadRadius);
		}

		public override void Fire()
		{
			var muzzle = GetMuzzle();
			if (muzzleParticlePrefab)
				ObjectPool.Spawn(muzzleParticlePrefab, muzzle.position, Attacker.AimTransform.rotation, Attacker.AimTransform);				

			for (int i = 0; i < bulletsPerFire; i++)
				FireBullet(muzzle);

			if (spreadIncreaseEachShot <= 0)
				return;

			CurrentSpread += spreadIncreaseEachShot * spreadStatMultiplier;
			CurrentSpread = Mathf.Clamp(CurrentSpread, 0, spreadRadius * spreadStatMultiplier);
		}

		public override float GetRange() => Range;

		void FireBullet(Transform muzzle)
		{
			Attacker.Stats.OnAttack(1);

			Vector3 forward = CalculateBulletSpread(1 / Attacker.Modifiers.ExtraAccuracy);

			int layerMask = ~LayerMask.GetMask("Corpse", "Enemy Rigidbody", "Target");
			if (!Physics.Raycast(Attacker.AimTransform.position, forward, out var hitInfo, Range, layerMask))
			{
				BulletTrace(muzzle.position, muzzle.position + forward * Range);
				return;
			}

			var damageData = DamageHelper.RaycastHit(this, hitInfo);

			BulletTrace(muzzle.position, hitInfo.point);

			if (damageData != null && damageData.ImpactEffect != null)
				ObjectPool.Spawn(damageData.ImpactEffect, hitInfo.point, Quaternion.LookRotation(hitInfo.normal, Vector3.up));
			else if (impactParticlePrefab)
				ObjectPool.Spawn(impactParticlePrefab, hitInfo.point, Quaternion.FromToRotation(Vector3.up, hitInfo.normal));
		}

		Vector3 CalculateBulletSpread(float modifier)
		{
			float spreadPercent = spreadIncreaseEachShot > 0 ? CurrentSpread : 1;
			Vector2 spread = spreadPercent * modifier * Random.insideUnitCircle * spreadRadius;
			return Attacker.AimTransform.forward + new Vector3(0, spread.y, spread.x);
		}

		void BulletTrace(Vector3 origin, Vector3 target)
		{
			if (!lineRendererPrefab)
				return;
			var instance = ObjectPool.Instantiate(lineRendererPrefab);
			var lineRenderer = instance.GetComponent<LineRenderer>();
			lineRenderer.SetPosition(0, origin);
			lineRenderer.SetPosition(1, target);
		}


		internal override void Aim_OnChanged(CharacterStat stat)
		{
			base.Aim_OnChanged(stat);
			spreadStatMultiplier = 1 + (stat.Value / 100f);
		}

	}
}
