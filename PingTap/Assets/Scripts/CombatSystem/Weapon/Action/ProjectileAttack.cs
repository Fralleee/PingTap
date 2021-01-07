using CombatSystem.Offense;
using Fralle.Core.Infrastructure;
using System.Collections;
using UnityEngine;

namespace CombatSystem.Action
{
	public class ProjectileAttack : AttackAction
	{
		[Header("ProjectileAttack")]
		[SerializeField] Projectile projectilePrefab = null;
		[SerializeField] ProjectileData projectileData = null;

		[Space(10)]
		[SerializeField] int projectilesPerFire = 0;
		[SerializeField] float delayTimePerProjectiles = 0f;
		[SerializeField] float radiusOnMaxRange = 0f;

		public override void Fire()
		{
			var muzzle = GetMuzzle();

			projectileData.attacker = attacker;
			projectileData.forward = weapon.combatant.aimTransform.forward;
			projectileData.damage = Damage;
			projectileData.element = element;
			projectileData.damageEffects = damageEffects;
			projectileData.hitboxLayer = hitboxLayer;


			StartCoroutine(SpawnProjectiles(muzzle));
		}

		public override float GetRange() => projectileData.range;

		IEnumerator SpawnProjectiles(Transform muzzle)
		{
			for (var i = 0; i < projectilesPerFire; i++)
			{
				attacker.Stats.OnAttack(1);

				var layerMask = ~LayerMask.GetMask("Corpse");
				if (Physics.Raycast(weapon.combatant.aimTransform.position, weapon.combatant.aimTransform.forward, out var hitInfo, projectileData.range, layerMask))
					projectileData.forward = (hitInfo.point - muzzle.position).normalized;

				var spread = (Random.insideUnitCircle * radiusOnMaxRange) / projectileData.range;
				projectileData.forward += new Vector3(0, spread.y, spread.x);

				var instance = ObjectPool.Instantiate(projectilePrefab.gameObject, muzzle.position, transform.rotation);
				var projectile = instance.GetComponent<Projectile>();
				projectile.Initiate(projectileData);

				yield return new WaitForSeconds(delayTimePerProjectiles);
			}
		}
	}
}
