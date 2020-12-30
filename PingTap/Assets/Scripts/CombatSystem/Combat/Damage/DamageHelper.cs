using CombatSystem.Action;
using CombatSystem.Effect;
using CombatSystem.Enums;
using CombatSystem.Offense;
using Fralle.Core.Extensions;
using System.Collections.Generic;
using UnityEngine;

namespace CombatSystem.Combat.Damage
{
	public static class DamageHelper
	{
		static readonly int hitboxLayer = LayerMask.NameToLayer("Hitbox");

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

			bool hitboxHit = hit.collider.gameObject.layer == hitboxLayer;
			if (!hitboxHit)
				return null;

			var damageController = hit.transform.GetComponentInParent<DamageController>();
			if (damageController != null)
			{
				// this will cause issues if we are for example hitting targets with shotgun
				// we will receive more hits than shots fired

				var hitbox = hit.collider.transform.GetComponent<Hitbox>();
				var hitArea = hitbox ? hitbox.hitArea : HitArea.MAJOR;
				var damageAmount = raycastAttack.Damage;
				var damageData = new DamageData()
				{
					attacker = raycastAttack.attacker,
					element = raycastAttack.element,
					effects = SetupDamageEffects(raycastAttack.damageEffects, raycastAttack.attacker, damageAmount),
					hitAngle = Vector3.Angle((raycastAttack.weapon.transform.position - hit.transform.position).normalized, hit.transform.forward),
					force = raycastAttack.attacker.aimTransform.forward * raycastAttack.pushForce,
					position = hit.point,
					hitArea = hitArea,
					damageAmount = hitArea.GetMultiplier() * damageAmount,
					impactEffect = hitArea.GetImpactEffect(damageController)
				};

				damageController.ReceiveAttack(damageData);
				raycastAttack.attacker.SuccessfulHit(damageData);
				return damageData;
			}

			return null;
		}

		public static DamageData ProjectileHit(ProjectileData projectileData, Vector3 position, Collision collision)
		{
			AddForce(projectileData, position, collision);

			bool hitboxHit = collision.collider.gameObject.layer == hitboxLayer;
			if (!hitboxHit)
				return null;

			var hitbox = collision.collider.transform.GetComponent<Hitbox>();
			var hitArea = hitbox ? hitbox.hitArea : HitArea.MAJOR;
			var damageController = collision.transform.GetComponentInParent<DamageController>();
			if (damageController != null)
			{
				var damageData = new DamageData()
				{
					attacker = projectileData.attacker,
					element = projectileData.element,
					hitAngle = Vector3.Angle((position - collision.transform.position).normalized, collision.transform.forward),
					effects = SetupDamageEffects(projectileData.damageEffects, projectileData.attacker, projectileData.damage),
					force = projectileData.forward * projectileData.pushForce,
					position = collision.GetContact(0).point,
					hitArea = hitArea,
					damageAmount = hitArea.GetMultiplier() * projectileData.damage,
					impactEffect = hitArea.GetImpactEffect(damageController)
				};
				damageController.ReceiveAttack(damageData);
				projectileData.attacker.SuccessfulHit(damageData);

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

			var colliders = Physics.OverlapSphere(position, projectileData.explosionRadius);

			List<Rigidbody> rigidBodies = new List<Rigidbody>();
			List<DamageController> targets = new List<DamageController>();
			foreach (var col in colliders)
			{
				if (col.TryGetComponent(out Rigidbody rigidbody) && !rigidBodies.Contains(rigidbody))
				{
					rigidBodies.Add(rigidbody);
				}

				var damageController = col.GetComponentInParent<DamageController>();
				if (damageController != null && !targets.Contains(damageController))
				{
					targets.Add(damageController);
				}
			}

			foreach (var rigidbody in rigidBodies)
			{
				rigidbody.AddExplosionForce(projectileData.pushForce, position, projectileData.explosionRadius + 1, 0.5f);
			}

			foreach (var damageController in targets)
			{
				var distance = Vector3.Distance(damageController.transform.position, position);
				if (distance > projectileData.explosionRadius + 1)
					continue;

				var distanceMultiplier = Mathf.Clamp01(1 - Mathf.Pow(distance / (projectileData.explosionRadius + 1), 2));

				var damageAmount = projectileData.damage * distanceMultiplier;
				var targetPosition = damageController.transform.position;
				var damageData = new DamageData()
				{
					attacker = projectileData.attacker,
					element = projectileData.element,
					effects = SetupDamageEffects(projectileData.damageEffects, projectileData.attacker, projectileData.damage),
					hitAngle = Vector3.Angle((position - targetPosition).normalized, damageController.transform.forward),
					force = (targetPosition - position).normalized.With(y: 0.5f) * projectileData.pushForce * distanceMultiplier,
					position = position,
					damageAmount = damageAmount
				};
				damageController.ReceiveAttack(damageData);
				projectileData.attacker.SuccessfulHit(damageData);
			}
		}

		static void AddForce(ProjectileData projectileData, Vector3 position, Collision collision)
		{
			var direction = projectileData.forward;
			var rigidBody = collision.transform.GetComponent<Rigidbody>();
			if (rigidBody == null)
				return;

			if (direction == Vector3.zero)
				direction = -(position - collision.collider.transform.position).normalized;
			rigidBody.AddForce(direction * projectileData.pushForce);
		}

		static void AddForce(RaycastAttack raycastAttack, RaycastHit hit)
		{
			var rigidBody = hit.transform.GetComponent<Rigidbody>();
			if (rigidBody != null)
			{
				rigidBody.AddForce(raycastAttack.attacker.aimTransform.forward * raycastAttack.pushForce);
			}
		}
	}
}
