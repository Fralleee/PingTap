using CharacterStats;
using CombatSystem.Combat.Damage;
using Fralle.Core.Attributes;
using Fralle.Core.Extensions;
using Fralle.Core.Pooling;
using UnityEngine;

namespace CombatSystem.Action
{
	public class RaycastAttack : AttackAction
	{
		[Header("RaycastAttack")]
		public float range = 50;
		public float pushForce = 3.5f;
		[SerializeField] GameObject impactParticlePrefab = null;
		[SerializeField] GameObject muzzleParticlePrefab = null;
		[SerializeField] GameObject lineRendererPrefab = null;
		[SerializeField] int bulletsPerFire = 1;
		public AnimationCurve rangeDamageFalloff = new AnimationCurve(new Keyframe[] { new Keyframe(0, 1), new Keyframe(1, 0) });

		[Header("Spread")]
		[SerializeField] float spreadRadius = 0f;
		[SerializeField] float spreadIncreaseEachShot = 0f;
		[SerializeField] float recovery = 1f;

		[Readonly] public float currentSpread;

		float spreadStatMultiplier = 1f;

		internal override void Start()
		{
			base.Start();

			if (stats)
				spreadStatMultiplier = stats.aim.Value;
		}

		void Update()
		{
			if (currentSpread.EqualsWithTolerance(0f))
				return;

			currentSpread -= Time.deltaTime * recovery;
			currentSpread = Mathf.Clamp(currentSpread, 0, spreadRadius);
		}

		public override void Fire()
		{
			var muzzle = GetMuzzle();
			if (muzzleParticlePrefab)
				ObjectPool.Spawn(muzzleParticlePrefab, muzzle.position, attacker.aimTransform.rotation, attacker.aimTransform);

			for (var i = 0; i < bulletsPerFire; i++)
				FireBullet(muzzle);

			if (spreadIncreaseEachShot <= 0)
				return;

			currentSpread += spreadIncreaseEachShot * spreadStatMultiplier;
			currentSpread = Mathf.Clamp(currentSpread, 0, spreadRadius * spreadStatMultiplier);
		}

		public override float GetRange() => range;

		void FireBullet(Transform muzzle)
		{
			attacker.Stats.OnAttack(1);

			var forward = CalculateBulletSpread(1 / attacker.Modifiers.extraAccuracy);

			var layerMask = ~LayerMask.GetMask("Corpse", "Enemy Rigidbody", "Target");
			if (!Physics.Raycast(attacker.aimTransform.position, forward, out var hitInfo, range, layerMask))
			{
				BulletTrace(muzzle.position, muzzle.position + forward * range);
				return;
			}

			var damageData = DamageHelper.RaycastHit(this, hitInfo);

			BulletTrace(muzzle.position, hitInfo.point);

			if (damageData != null && damageData.impactEffect != null)
				ObjectPool.Spawn(damageData.impactEffect, hitInfo.point, Quaternion.LookRotation(hitInfo.normal, Vector3.up));
			else if (impactParticlePrefab)
				ObjectPool.Spawn(impactParticlePrefab, hitInfo.point, Quaternion.FromToRotation(Vector3.up, hitInfo.normal));
		}

		Vector3 CalculateBulletSpread(float modifier)
		{
			var spreadPercent = spreadIncreaseEachShot > 0 ? currentSpread : 1;
			var spread = spreadPercent * modifier * Random.insideUnitCircle * spreadRadius;
			return attacker.aimTransform.forward + new Vector3(0, spread.y, spread.x);
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
