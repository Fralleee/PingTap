﻿using CombatSystem.Combat.Damage;
using Fralle.Core.Attributes;
using Fralle.Core.Extensions;
using UnityEngine;

namespace CombatSystem.Action
{
	public class RaycastAttack : AttackAction
	{
		[Header("RaycastAttack")]
		[SerializeField] float range = 50;
		public float pushForce = 3.5f;
		[SerializeField] GameObject impactParticlePrefab = null;
		[SerializeField] GameObject muzzleParticlePrefab = null;
		[SerializeField] GameObject lineRendererPrefab = null;
		[SerializeField] int bulletsPerFire = 1;

		[Header("Spread")]
		[SerializeField] float spreadRadius = 0f;
		[SerializeField] float spreadIncreaseEachShot = 0f;
		[SerializeField] float recovery = 1f;

		[Readonly] public float currentSpread;

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
			{
				var muzzleParticle = Instantiate(muzzleParticlePrefab, muzzle.position, attacker.aimTransform.rotation, attacker.aimTransform);
				Destroy(muzzleParticle, 1.5f);
			}

			for (var i = 0; i < bulletsPerFire; i++)
				FireBullet(muzzle);

			if (spreadIncreaseEachShot <= 0)
				return;
			currentSpread += spreadIncreaseEachShot;
			currentSpread = Mathf.Clamp(currentSpread, 0, spreadRadius);
		}

		public override float GetRange() => range;

		void FireBullet(Transform muzzle)
		{
			attacker.Stats.OnAttack(1);

			var forward = CalculateBulletSpread(1 / attacker.Modifiers.extraAccuracy);

			var layerMask = ~LayerMask.GetMask("Corpse", "Enemy Rigidbody", "Target");
			if (!Physics.Raycast(attacker.aimTransform.position, forward, out var hitInfo, range, layerMask))
				return;

			var damageData = DamageHelper.RaycastHit(this, hitInfo);

			BulletTrace(muzzle.position, hitInfo.point);

			if (damageData != null)
			{
				var impactParticle = Instantiate(damageData.impactEffect, hitInfo.point, Quaternion.LookRotation(attacker.aimTransform.forward, Vector3.up));
				Destroy(impactParticle, 5f);
			}
			else if (impactParticlePrefab)
			{
				var impactParticle = Instantiate(impactParticlePrefab, hitInfo.point, Quaternion.FromToRotation(Vector3.up, hitInfo.normal));
				Destroy(impactParticle, 5f);
			}
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
			var lineRendererInstance = Instantiate(lineRendererPrefab);
			var lineRenderer = lineRendererInstance.GetComponent<LineRenderer>();
			lineRenderer.SetPosition(0, origin);
			lineRenderer.SetPosition(1, target);
		}

	}
}
