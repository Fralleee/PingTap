﻿using Fralle.Core.Extensions;
using Fralle.Core.Pooling;
using System.Collections;
using UnityEngine;

namespace Fralle.Pingtap
{
	public class ProjectileAttack : AttackAction
	{
		[Header("ProjectileAttack")]
		[SerializeField] GameObject muzzleParticlePrefab;
		[SerializeField] Projectile projectilePrefab;
		[SerializeField] ProjectileData projectileData;

		[Space(10)]
		[SerializeField] int projectilesPerFire = 1;
		[SerializeField] float delayTimePerProjectiles = 0f;
		[SerializeField] float spreadRadiusOnMaxRange = 0f;

		ParticleSystem muzzleParticle;

		internal override void Start()
		{
			base.Start();

			SetupMuzzle();
		}

		void SetupMuzzle()
		{
			var instance = Instantiate(muzzleParticlePrefab, Combatant.AimTransform.position, Combatant.AimTransform.rotation, Combatant.AimTransform);
			if (Combatant.gameObject.CompareTag("Player"))
				instance.SetLayerRecursively(LayerMask.NameToLayer("FPO")); // this should only be performed on localplayer
			muzzleParticle = instance.GetComponent<ParticleSystem>();
		}

		public override void Fire()
		{
			var muzzle = GetMuzzle();

			muzzleParticle.transform.position = muzzle.position;
			muzzleParticle.Play();

			projectileData.Attacker = Combatant;
			projectileData.Forward = Weapon.Combatant.AimTransform.forward;
			projectileData.Damage = Damage;
			projectileData.Element = Element;
			projectileData.DamageEffects = DamageEffects;
			projectileData.HitboxLayer = HitboxLayer;


			StartCoroutine(SpawnProjectiles(muzzle));
		}

		public override float GetRange() => projectileData.Range;

		IEnumerator SpawnProjectiles(Transform muzzle)
		{
			for (int i = 0; i < projectilesPerFire; i++)
			{
				Combatant.Stats.OnAttack(1);

				int layerMask = ~LayerMask.GetMask("Corpse", "Target Information");
				Ray ray = new Ray(Weapon.Combatant.AimTransform.position, Weapon.Combatant.AimTransform.forward);
				if (Physics.Raycast(ray, out var hitInfo, projectileData.Range, layerMask))
					projectileData.Forward = (hitInfo.point - muzzle.position).normalized;
				else
					projectileData.Forward = (ray.GetPoint(Mathf.Min(projectileData.Range, 50f)) - muzzle.position).normalized;

				var spread = (Random.insideUnitCircle * spreadRadiusOnMaxRange) / projectileData.Range;
				projectileData.Forward += new Vector3(0, spread.y, spread.x);

				var instance = ObjectPool.Spawn(projectilePrefab.gameObject, muzzle.position, Quaternion.LookRotation(projectileData.Forward, transform.up));
				var projectile = instance.GetComponent<Projectile>();
				projectile.gameObject.layer = Combatant.teamController.AllyProjectiles;
				projectile.Initiate(projectileData);

				yield return new WaitForSeconds(delayTimePerProjectiles);
			}
		}
	}
}