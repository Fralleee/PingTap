using CombatSystem.Combat.Damage;
using Fralle.Core.Extensions;
using UnityEngine;

namespace CombatSystem.Offense
{
	[RequireComponent(typeof(SphereCollider))]
	[RequireComponent(typeof(Rigidbody))]
	public class Projectile : MonoBehaviour
	{
		[SerializeField] GameObject impactParticlePrefab = null;
		[SerializeField] GameObject muzzleParticlePrefab = null;

		new Rigidbody rigidbody;
		ProjectileData data;
		bool hasCollision;
		float distanceTraveled;
		float activeTime;
		float afterCollisionTime;

		void FixedUpdate()
		{
			distanceTraveled += rigidbody.velocity.magnitude * Time.deltaTime;
			if (distanceTraveled > data.range)
			{
				if (data.explodeOnMaxRange)
					Explode();
				else
					Destroy(gameObject);
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
			rigidbody = GetComponentInChildren<Rigidbody>();
			data = inputData;

			rigidbody.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
			rigidbody.useGravity = data.useGravity;
			rigidbody.AddForce(data.forward * data.force, ForceMode.VelocityChange);

			if (!muzzleParticlePrefab)
				return;
			var muzzleParticle = Instantiate(muzzleParticlePrefab, transform.position, transform.rotation);
			var layer = LayerMask.NameToLayer("First Person Objects");
			muzzleParticle.SetLayerRecursively(layer);
			Destroy(muzzleParticle, 1.5f);
		}

		void Explode(Collision collision = null)
		{
			if (impactParticlePrefab)
			{
				var impactParticle = Instantiate(impactParticlePrefab, transform.position, Quaternion.identity);
				Destroy(impactParticle, 5f);
			}

			DamageHelper.Explosion(data, transform.position, collision);

			Destroy(gameObject);
		}

		void Hit(Collision collision)
		{
			AddForce(collision, data.forward);

			var damageData = DamageHelper.ProjectileHit(data, transform.position, collision);
			if (damageData != null)
			{
				var contact = collision.GetContact(0);
				var impactParticle = Instantiate(damageData.impactEffect, contact.point, Quaternion.LookRotation(contact.normal, Vector3.up));
				Destroy(impactParticle, 5f);
			}
			else if (impactParticlePrefab)
			{
				var impactParticle = Instantiate(
					impactParticlePrefab,
					transform.position,
					Quaternion.FromToRotation(Vector3.up, collision.GetContact(0).normal)
				);
				Destroy(impactParticle, 5f);
			}


			Destroy(gameObject);
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
	}
}
