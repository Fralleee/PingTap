using Fralle.Core.Extensions;
using System.Collections.Generic;
using UnityEngine;

namespace Fralle.Pingtap
{
	public static class DamageHelper
	{
		public static DamageEffect[] SetupDamageEffects(DamageEffect[] damageEffects, Combatant combatant, float damageAmount)
		{
			for (int i = 0; i < damageEffects.Length; i++)
			{
				damageEffects[i] = damageEffects[i].Setup(combatant, damageAmount);
			}

			return damageEffects;
		}

		public static DamageData RaycastHit(RaycastAttack raycastAttack, RaycastHit hit)
		{

			AddForce(raycastAttack, hit);

			bool hitboxHit = raycastAttack.Combatant.teamController.Hitboxes.IsInLayerMask(hit.collider.gameObject.layer);
			if (!hitboxHit)
				return null;

			var damageController = hit.transform.GetComponentInParent<DamageController>();
			if (damageController != null)
			{
				// this will cause issues if we are for example hitting targets with shotgun
				// we will receive more hits than shots fired
				var hitbox = hit.collider.transform.GetComponent<Hitbox>();
				var hitArea = hitbox ? hitbox.HitArea : HitArea.Chest;
				var falloffMultiplier = raycastAttack.RangeDamageFalloff.Evaluate(hit.distance / raycastAttack.Range);
				var damageAmount = raycastAttack.Damage;
				var damageData = new DamageData()
				{
					Attacker = raycastAttack.Combatant,
					Element = raycastAttack.Element,
					Effects = SetupDamageEffects(raycastAttack.DamageEffects, raycastAttack.Combatant, damageAmount),
					HitAngle = Vector3.Angle((raycastAttack.Weapon.transform.position - hit.transform.position).normalized, hit.transform.forward),
					Force = raycastAttack.Combatant.AimTransform.forward * raycastAttack.PushForce,
					Position = hit.point,
					Normal = hit.normal,
					HitArea = hitArea,
					DamageAmount = hitArea.GetMultiplier() * damageAmount * falloffMultiplier,
					ImpactEffect = hitArea.GetImpactEffect(damageController)
				};

				damageController.ReceiveAttack(damageData);
				raycastAttack.Combatant.SuccessfulHit(damageData);
				return damageData;
			}

			return null;
		}

		public static DamageData ProjectileHit(ProjectileData projectileData, Vector3 position, Collision collision)
		{
			AddForce(projectileData, position, collision);

			bool hitboxHit = projectileData.Attacker.teamController.Hitboxes.IsInLayerMask(collision.collider.gameObject.layer);
			if (!hitboxHit)
				return null;

			var hitbox = collision.collider.transform.GetComponent<Hitbox>();
			var hitArea = hitbox ? hitbox.HitArea : HitArea.Chest;
			var damageController = collision.transform.GetComponentInParent<DamageController>();
			if (damageController != null)
			{
				var damageData = new DamageData()
				{
					Attacker = projectileData.Attacker,
					Element = projectileData.Element,
					HitAngle = Vector3.Angle((position - collision.transform.position).normalized, collision.transform.forward),
					Effects = SetupDamageEffects(projectileData.DamageEffects, projectileData.Attacker, projectileData.Damage),
					Force = projectileData.Forward * projectileData.Force,
					Position = collision.GetContact(0).point,
					Normal = collision.GetContact(0).normal,
					HitArea = hitArea,
					DamageAmount = hitArea.GetMultiplier() * projectileData.Damage,
					ImpactEffect = hitArea.GetImpactEffect(damageController)
				};
				damageController.ReceiveAttack(damageData);
				projectileData.Attacker.SuccessfulHit(damageData);

				return damageData;
			}
			return null;
		}

		public static void Explosion(ProjectileData projectileData, Vector3 position, Collision collision = null)
		{
			if (collision != null)
			{
				position = collision.GetContact(0).point;
			}

			var teamController = projectileData.Attacker.teamController;
			var colliders = Physics.OverlapSphere(position, projectileData.ExplosionRadius, teamController.Hostiles | 1 << 0);

			List<Rigidbody> rigidBodies = new List<Rigidbody>();
			HashSet<DamageController> targets = new HashSet<DamageController>();
			foreach (var col in colliders)
			{
				if (col.TryGetComponent(out Rigidbody rigidbody) && !rigidBodies.Contains(rigidbody))
					rigidBodies.Add(rigidbody);

				var damageController = col.GetComponentInParent<DamageController>();
				if (damageController != null)
					targets.Add(damageController);
			}

			foreach (var rigidbody in rigidBodies)
				rigidbody.AddExplosionForce(projectileData.PushForce, position, projectileData.ExplosionRadius + 1, 0.5f, ForceMode.Impulse);

			foreach (var damageController in targets)
			{
				var distance = Vector3.Distance(damageController.transform.position, position);
				if (distance > projectileData.ExplosionRadius + 1)
					continue;

				var distanceMultiplier = Mathf.Clamp01(1 - Mathf.Pow(distance / (projectileData.ExplosionRadius + 1), 2));

				var damageAmount = projectileData.Damage * distanceMultiplier;
				var targetPosition = damageController.transform.position;
				var damageData = new DamageData()
				{
					Attacker = projectileData.Attacker,
					Element = projectileData.Element,
					Effects = SetupDamageEffects(projectileData.DamageEffects, projectileData.Attacker, projectileData.Damage),
					HitAngle = Vector3.Angle((position - targetPosition).normalized, damageController.transform.forward),
					Force = (targetPosition - position).normalized.With(y: 0.5f) * projectileData.PushForce * distanceMultiplier,
					Position = position,
					DamageAmount = damageAmount
				};
				damageController.ReceiveAttack(damageData);
				projectileData.Attacker.SuccessfulHit(damageData);
			}
		}

		static void AddForce(ProjectileData projectileData, Vector3 position, Collision collision)
		{
			var direction = projectileData.Forward;
			var rigidBody = collision.transform.GetComponent<Rigidbody>();
			if (rigidBody == null)
				return;

			if (direction == Vector3.zero)
				direction = -(position - collision.collider.transform.position).normalized;
			rigidBody.AddForce(direction * projectileData.PushForce, ForceMode.Impulse);
		}

		static void AddForce(RaycastAttack raycastAttack, RaycastHit hit)
		{
			var rigidBody = hit.transform.GetComponent<Rigidbody>();
			if (rigidBody != null)
			{
				rigidBody.AddForce(raycastAttack.Combatant.AimTransform.forward * raycastAttack.PushForce, ForceMode.Impulse);
			}
		}
	}
}
