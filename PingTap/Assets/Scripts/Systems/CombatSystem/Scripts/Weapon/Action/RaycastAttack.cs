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
		[SerializeField] bool spreadOnFirstShot;

		[Readonly] public float CurrentSpread;

		float spreadStatMultiplier = 1f;

		internal override void Start()
		{
			base.Start();

			if (Stats)
				spreadStatMultiplier = Stats.Aim.Value;

			float lowestCurrentSpread = spreadOnFirstShot ? spreadIncreaseEachShot : 0f;
			CurrentSpread = lowestCurrentSpread;
		}

		void Update()
		{
			ModifyCurrentSpread();
		}

		public override void Fire()
		{
			var muzzle = GetMuzzle();
			if (muzzleParticlePrefab) {
				var instance = ObjectPool.Spawn(muzzleParticlePrefab, muzzle.position, Combatant.AimTransform.rotation, Combatant.AimTransform);
				instance.SetLayerRecursively(LayerMask.NameToLayer("First Person Objects")); // this should only be performed on localplayer
			}
				
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
			Combatant.Stats.OnAttack(1);

			Vector3 forward = CalculateBulletSpread(1 / Combatant.Modifiers.ExtraAccuracy);

			int layerMask = ~LayerMask.GetMask("Corpse", "Enemy Rigidbody", "Target");
			if (!Physics.Raycast(Combatant.AimTransform.position, forward, out var hitInfo, Range, layerMask))
			{
				BulletTrace(muzzle.position, muzzle.position + forward * Range);
				return;
			}

			var damageData = DamageHelper.RaycastHit(this, hitInfo);

			BulletTrace(muzzle.position, hitInfo.point);

			if (damageData != null && damageData.ImpactEffect != null)
				ObjectPool.Spawn(damageData.ImpactEffect, hitInfo.point, Quaternion.LookRotation(hitInfo.normal, Vector3.up));
			else
				ObjectPool.Spawn(Combatant.impactAtlas.GetImpactEffectFromTag(hitInfo.collider.tag), hitInfo.point, Quaternion.LookRotation(hitInfo.normal, Vector3.up));
		}

		void ModifyCurrentSpread()
		{
			float lowestCurrentSpread = spreadOnFirstShot ? spreadIncreaseEachShot : 0f;
			if (CurrentSpread.EqualsWithTolerance(lowestCurrentSpread))
				return;

			CurrentSpread -= Time.deltaTime * recovery;
			CurrentSpread = Mathf.Clamp(CurrentSpread, lowestCurrentSpread, spreadRadius);
		}

		Vector3 CalculateBulletSpread(float modifier)
		{
			float spreadPercent = spreadIncreaseEachShot > 0 ? CurrentSpread : 1;
			Vector2 spread = spreadPercent * modifier * Random.insideUnitCircle * spreadRadius;
			return Combatant.AimTransform.forward + new Vector3(0, spread.y, spread.x);
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

#if UNITY_EDITOR
		internal override void OnValidate()
		{
			base.OnValidate();
			float lowestCurrentSpread = spreadOnFirstShot ? spreadIncreaseEachShot : 0f;
			CurrentSpread = lowestCurrentSpread;
		}
#endif
	}
}
