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
    float distanceTraveled;

    void FixedUpdate()
    {
      distanceTraveled += rigidbody.velocity.magnitude * Time.deltaTime;
      if (distanceTraveled > data.range)
      {
        if (data.explodeOnMaxRange) Explode();
        else Destroy(gameObject);
      }
    }

    void Explode(Collision collision = null)
    {
      active = true;

      bool hasImpact = collision != null;
      var position = hasImpact ? collision.GetContact(0).point : transform.position;
      if (hasImpact && data.kinematicOnImpact) rigidbody.isKinematic = true;

      var colliders = Physics.OverlapSphere(position, data.explosionRadius);
      foreach (var col in colliders)
      {
        var distance = Vector3.Distance(col.transform.position, position);
        var distanceDamageMultiplier = Mathf.Clamp01(1 - distance / data.explosionRadius);

        var colRb = col.GetComponent<Rigidbody>();
        if (colRb) colRb.AddExplosionForce(data.pushForce, position, data.explosionRadius);

        var damageable = col.GetComponent<IDamageable>();
        if (damageable != null) damageable.TakeDamage(data.explosionDamage * distanceDamageMultiplier);
      }

      Destroy(gameObject, hasImpact ? data.destroyOnImpactTime : 0);
    }

    void Hit(Collision collision)
    {
      active = true;

      if (data.kinematicOnImpact) rigidbody.isKinematic = true;

      var colRb = collision.gameObject.GetComponent<Rigidbody>();
      if (colRb) colRb.AddForce(transform.position - collision.transform.position * data.pushForce);

      var damageable = collision.gameObject.GetComponent<IDamageable>();
      if (damageable != null) damageable.TakeDamage(data.explosionDamage);

      if (impactParticlePrefab)
      {
        var impactParticle = Instantiate(
          impactParticlePrefab,
          transform.position,
          Quaternion.FromToRotation(Vector3.up, collision.GetContact(0).normal)
        );
        Destroy(impactParticle, 5f);
      }


      Destroy(gameObject, data.destroyOnImpactTime);
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
      if (data.explosionRadius > 0) Explode(collision);
      else Hit(collision);
    }

    void OnDrawGizmos()
    {
      if (data.explosionRadius > 0 && active) Gizmos.DrawWireSphere(transform.position, data.explosionRadius);
    }
  }
}