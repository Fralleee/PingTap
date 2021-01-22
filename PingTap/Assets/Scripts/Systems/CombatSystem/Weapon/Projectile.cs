using CombatSystem.Combat.Damage;
using Fralle.Core.Extensions;
using Fralle.Core.Pooling;
using UnityEngine;

namespace CombatSystem.Offense
{
	[RequireComponent(typeof(SphereCollider))]
	[RequireComponent(typeof(Rigidbody))]
	public class Projectile : MonoBehaviour
	{
		[SerializeField] GameObject impactEffectPrefab;
		[SerializeField] GameObject muzzleParticlePrefab;

		new Rigidbody rigidbody;
		ProjectileData data;
		bool hasCollision;
		float distanceTraveled;
		float activeTime;
		float afterCollisionTime;

		void Awake()
		{
			rigidbody = GetComponentInChildren<Rigidbody>();
		}

		void FixedUpdate()
		{
			distanceTraveled += rigidbody.velocity.magnitude * Time.deltaTime;
			if (distanceTraveled > data.range)
			{
				if (data.explodeOnMaxRange)
					Explode();
				else
					ObjectPool.Despawn(gameObject);
			}

			if (data.explodeOnTime > 0)
			{
				activeTime += Time.fixedDeltaTime;
				if (activeTime > data.explodeOnTime)
					Explode();
			}

			if (!hasCollision || (data.explodeOnImpactTime <= 0))
				return;
			afterCollisionTime += Time.fixedDeltaTime;
			if (afterCollisionTime > data.explodeOnImpactTime)
				Explode();
		}

		public void Initiate(ProjectileData inputData)
		{
			data = inputData;

			rigidbody.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
			rigidbody.useGravity = data.useGravity;
			rigidbody.AddForce(data.forward * data.force, ForceMode.VelocityChange);

			if (!muzzleParticlePrefab)
				return;

			var muzzleParticle = ObjectPool.Spawn(muzzleParticlePrefab, transform.position, transform.rotation);
			//var layer = LayerMask.NameToLayer("First Person Objects");
			//muzzleParticle.SetLayerRecursively(layer);
		}

		void Explode(Collision collision = null)
		{
			if (impactEffectPrefab)
				ObjectPool.Spawn(impactEffectPrefab, transform.position, Quaternion.identity);

			DamageHelper.Explosion(data, transform.position, collision);
			ObjectPool.Despawn(gameObject);
		}

		void Hit(Collision collision)
		{
			AddForce(collision, data.forward);

			var damageData = DamageHelper.ProjectileHit(data, transform.position, collision);
			if (damageData != null)
			{
				var contact = collision.GetContact(0);
				ObjectPool.Spawn(damageData.impactEffect, contact.point, Quaternion.LookRotation(contact.normal, Vector3.up));
			}
			else if (impactEffectPrefab)
				ObjectPool.Spawn(impactEffectPrefab, transform.position, Quaternion.FromToRotation(Vector3.up, collision.GetContact(0).normal));

			ObjectPool.Despawn(gameObject);
		}

		void AddForce(Collision collision, Vector3 direction)
		{
			var rigidBody = collision.transform.GetComponent<Rigidbody>();
			if (rigidBody == null)
				return;

			if (direction == Vector3.zero)
				direction = -(transform.position - collision.collider.transform.position).normalized;
			rigidBody.AddForce(direction * data.pushForce);
		}

		void OnCollisionEnter(Collision collision)
		{
			if (data.kinematicOnImpact)
			{
				rigidbody.isKinematic = true;
				transform.parent = collision.transform;
			}

			if (data.explodeOnImpactTime > 0)
				hasCollision = true;
			if (data.explosionRadius > 0 && data.explodeOnImpactTime.EqualsWithTolerance(0f))
				Explode(collision);
			else if (data.explosionRadius.EqualsWithTolerance(0f))
				Hit(collision);
		}

		void OnDisable()
		{
			hasCollision = false;
			distanceTraveled = 0;
			activeTime = 0;
			afterCollisionTime = 0;
			rigidbody.velocity = Vector3.zero;
			rigidbody.isKinematic = false;
		}
	}
}
