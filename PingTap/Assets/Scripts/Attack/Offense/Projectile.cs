using Fralle.Core.Extensions;
using System.Linq;
using UnityEngine;

namespace Fralle.Attack.Offense
{
  [RequireComponent(typeof(SphereCollider))]
  [RequireComponent(typeof(Rigidbody))]
  public class Projectile : MonoBehaviour
  {
    [SerializeField] GameObject impactParticlePrefab;
    [SerializeField] GameObject muzzleParticlePrefab;

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
        if (data.explodeOnMaxRange) Explode();
        else Destroy(gameObject);
      }

      if (data.explodeOnTime > 0)
      {
        activeTime += Time.fixedDeltaTime;
        if (activeTime > data.explodeOnTime) Explode();
      }

      if (!hasCollision || !(data.explodeOnImpactTime > 0)) return;
      afterCollisionTime += Time.fixedDeltaTime;
      if (afterCollisionTime > data.explodeOnImpactTime) Explode();
    }


    public void Initiate(ProjectileData inputData)
    {
      rigidbody = GetComponentInChildren<Rigidbody>();
      data = inputData;

      rigidbody.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
      rigidbody.useGravity = data.useGravity;
      rigidbody.AddForce(data.forward * data.force, ForceMode.VelocityChange);

      if (!muzzleParticlePrefab) return;
      var muzzleParticle = Instantiate(muzzleParticlePrefab, transform.position, transform.rotation);
      var layer = LayerMask.NameToLayer("First Person Objects");
      muzzleParticle.SetLayerRecursively(layer);
      Destroy(muzzleParticle, 1.5f);
    }

    void Explode(Collision collision = null)
    {
      var position = transform.position;
      if (collision != null) position = collision.GetContact(0).point;

      if (impactParticlePrefab)
      {
        var impactParticle = Instantiate(impactParticlePrefab, position, Quaternion.identity);
        Destroy(impactParticle, 5f);
      }

      var colliders = Physics.OverlapSphere(position, data.explosionRadius);
      foreach (var col in colliders)
      {
        AddExplosionForce(col, position);
      }

      var targets = colliders.Select(x => x.GetComponentInParent<Health>()).Where(x => x != null).Distinct().ToArray();

      foreach (var health in targets)
      {
        var distance = Vector3.Distance(health.transform.position, position);
        if (distance > data.explosionRadius + 1) continue;

        var distanceMultiplier = Mathf.Clamp01(1 - Mathf.Pow(distance / (data.explosionRadius + 1), 2));

        var damageAmount = data.damage * distanceMultiplier;
        var targetPosition = health.transform.position;
        var damage = new Damage()
        {
          player = data.player,
          element = data.element,
          effects = data.damageEffects.Select(x => x.Setup(data.player, damageAmount)).ToArray(),
          hitAngle = Vector3.Angle((position - targetPosition).normalized, health.transform.forward),
          force = (targetPosition - position).normalized.With(y: 0.5f) * data.pushForce * distanceMultiplier,
          position = position,
          damageAmount = damageAmount
        };
        health.ReceiveAttack(damage);
      }

      Destroy(gameObject);
    }

    void Hit(Collision collision)
    {
      AddForce(collision, data.forward);

      var health = collision.gameObject.GetComponent<Health>();
      if (health != null)
      {
        health.ReceiveAttack(new Damage()
        {
          player = data.player,
          element = data.element,
          hitAngle = Vector3.Angle((transform.position - collision.transform.position).normalized, collision.transform.forward),
          effects = data.damageEffects.Select(x => x.Setup(data.player, data.damage)).ToArray(),
          force = data.forward * data.pushForce,
          position = collision.GetContact(0).point,
          damageAmount = data.damage
        });
      }

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

    void AddExplosionForce(Component component, Vector3 position)
    {
      var rigidBody = component.GetComponent<Rigidbody>();
      if (rigidBody != null) rigidBody.AddExplosionForce(data.pushForce, position, data.explosionRadius + 1, 0.5f);
    }

    void AddForce(Collision collision, Vector3 direction)
    {
      var rigidBody = collision.transform.GetComponent<Rigidbody>();
      if (rigidBody == null) return;

      if (direction == Vector3.zero) direction = -(transform.position - collision.collider.transform.position).normalized;
      rigidBody.AddForce(direction * data.pushForce);
    }

    void OnCollisionEnter(Collision collision)
    {
      data.player.stats.ReceiveHits(1);

      if (data.kinematicOnImpact)
      {
        rigidbody.isKinematic = true;
        transform.parent = collision.transform;
      }

      if (data.explodeOnImpactTime > 0) hasCollision = true;
      if (data.explosionRadius > 0 && data.explodeOnImpactTime == 0) Explode(collision);
      else if (data.explosionRadius == 0) Hit(collision);
    }
  }
}