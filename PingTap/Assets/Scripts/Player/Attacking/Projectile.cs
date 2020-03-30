﻿using UnityEditorInternal;
using UnityEngine;

namespace Fralle
{
  [RequireComponent(typeof(Rigidbody))]
  public class Projectile : MonoBehaviour
  {
    [SerializeField] GameObject impactParticlePrefab;
    [SerializeField] GameObject muzzleParticlePrefab;
    [SerializeField] GameObject[] trailParticles;

    new Rigidbody rigidbody;
    ProjectileData data;
    bool active;
    bool hasCollision;
    float distanceTraveled;
    float activeTime;
    float afterCollisionTime;

    void FixedUpdate()
    {
      distanceTraveled += rigidbody.velocity.magnitude * Time.deltaTime;
      if (distanceTraveled > data.range)
      {
        if (data.explodeOnMaxRange) Explode();
        else Destroy(gameObject);
      }

      if (data.explodeOnTime > 0)
      {
        activeTime += Time.fixedDeltaTime;
        if (activeTime > data.explodeOnTime) Explode();
      }

      if (hasCollision && data.explodeOnImpactTime > 0)
      {
        afterCollisionTime += Time.fixedDeltaTime;
        if (afterCollisionTime > data.explodeOnImpactTime) Explode();
      }
    }

    void Explode(Collision collision = null)
    {
      active = true;

      if (impactParticlePrefab)
      {
        var impactParticle = Instantiate( impactParticlePrefab, transform.position, Quaternion.identity );
        Destroy(impactParticle, 5f);
      }

      var colliders = Physics.OverlapSphere(transform.position, data.explosionRadius);
      foreach (var col in colliders)
      {
        var distance = Vector3.Distance(col.transform.position, transform.position);
        var distanceDamageMultiplier = Mathf.Clamp01(1 - distance / data.explosionRadius);

        var colRb = col.GetComponent<Rigidbody>();
        if (colRb) colRb.AddExplosionForce(data.pushForce, transform.position, data.explosionRadius);

        var damageable = col.GetComponent<IDamageable>();
        damageable?.TakeDamage(data.explosionDamage * distanceDamageMultiplier);
      }

      Destroy(gameObject);
    }

    void Hit(Collision collision)
    {
      active = true;

      var colRb = collision.gameObject.GetComponent<Rigidbody>();
      if (colRb) colRb.AddForce(transform.position - collision.transform.position * data.pushForce);

      var damageable = collision.gameObject.GetComponent<IDamageable>();
      damageable?.TakeDamage(data.explosionDamage);

      if (impactParticlePrefab)
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

    public void Initiate(ProjectileData inputData)
    {
      rigidbody = GetComponentInChildren<Rigidbody>();
      data = inputData;

      rigidbody.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
      rigidbody.useGravity = data.useGravity;
      rigidbody.AddForce(data.launcherCamera.forward * data.force, ForceMode.VelocityChange);

      if (muzzleParticlePrefab)
      {
        var muzzleParticle = Instantiate(muzzleParticlePrefab, transform.position, transform.rotation);
        Destroy(muzzleParticle, 1.5f);
      }
    }

    void OnCollisionEnter(Collision collision)
    {
      if (data.kinematicOnImpact) rigidbody.isKinematic = true;

      if (data.explodeOnImpactTime > 0) hasCollision = true;
      if (data.explosionRadius > 0 && data.explodeOnImpactTime == 0) Explode(collision);
      else if (data.explosionRadius == 0 && data.explodeOnImpactTime == 0) Hit(collision);
    }

    void OnDrawGizmos()
    {
      if (data.explosionRadius > 0 && active) Gizmos.DrawWireSphere(transform.position, data.explosionRadius);
    }
  }
}