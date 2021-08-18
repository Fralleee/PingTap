using Fralle.Core;
using Fralle.Core.Pooling;
using UnityEngine;

namespace Fralle.PingTap
{
  [RequireComponent(typeof(SphereCollider))]
  [RequireComponent(typeof(Rigidbody))]
  public class Projectile : MonoBehaviour
  {
    [SerializeField] GameObject impactEffectPrefab;

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
      if (rigidbody.velocity.normalized != Vector3.zero)
        rigidbody.rotation = Quaternion.LookRotation(rigidbody.velocity.normalized);
      distanceTraveled += rigidbody.velocity.magnitude * Time.deltaTime;
      if (distanceTraveled > data.Range)
      {
        if (data.ExplodeOnMaxRange)
          Explode();
        else
          ObjectPool.Despawn(gameObject);
      }

      if (data.ExplodeOnTime > 0)
      {
        activeTime += Time.fixedDeltaTime;
        if (activeTime > data.ExplodeOnTime)
          Explode();
      }

      if (!hasCollision || (data.ExplodeOnImpactTime <= 0))
        return;
      afterCollisionTime += Time.fixedDeltaTime;
      if (afterCollisionTime > data.ExplodeOnImpactTime)
        Explode();
    }

    public void Initiate(ProjectileData inputData)
    {
      data = inputData;

      rigidbody.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
      rigidbody.useGravity = data.UseGravity;
      rigidbody.AddForce(data.Forward * data.Force, ForceMode.VelocityChange);
    }

    void Explode(Collision collision = null)
    {
      if (impactEffectPrefab)
      {
        GameObject field = ObjectPool.Spawn(impactEffectPrefab, transform.position, Quaternion.identity);
        field.GetComponentInChildren<ExplosionShockwave>()?.Setup(data.ExplosionRadius);
      }

      DamageHelper.Explosion(data, transform.position, collision);
      ObjectPool.Despawn(gameObject);
    }

    void Hit(Collision collision)
    {
      DamageData damageData = DamageHelper.ProjectileHit(data, transform.position, collision);
      if (damageData != null)
      {
        ContactPoint contact = collision.GetContact(0);
        ObjectPool.Spawn(damageData.ImpactEffect, contact.point, Quaternion.LookRotation(contact.normal, Vector3.up));
      }
      else if (impactEffectPrefab)
        ObjectPool.Spawn(impactEffectPrefab, transform.position, Quaternion.FromToRotation(Vector3.up, collision.GetContact(0).normal));

      ObjectPool.Despawn(gameObject);
    }

    void OnCollisionEnter(Collision collision)
    {
      if (data.KinematicOnImpact)
      {
        rigidbody.isKinematic = true;
        transform.parent = collision.transform;
      }

      if (data.ExplodeOnImpactTime > 0)
        hasCollision = true;
      if (data.ExplosionRadius > 0 && data.ExplodeOnImpactTime.EqualsWithTolerance(0f))
        Explode(collision);
      else if (data.ExplosionRadius.EqualsWithTolerance(0f))
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
