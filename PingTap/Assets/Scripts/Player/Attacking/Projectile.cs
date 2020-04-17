using UnityEngine;

namespace Fralle
{
  [RequireComponent(typeof(SphereCollider))]
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

      if (!hasCollision || !(data.explodeOnImpactTime > 0)) return;
      afterCollisionTime += Time.fixedDeltaTime;
      if (afterCollisionTime > data.explodeOnImpactTime) Explode();
    }

    void Explode(Collision collision = null)
    {
      active = true;

      Vector3 position = transform.position;
      if (collision != null) position = collision.GetContact(0).point;

      if (impactParticlePrefab)
      {
        var impactParticle = Instantiate(impactParticlePrefab, position, Quaternion.identity);
        Destroy(impactParticle, 5f);
      }

      Collider[] colliders = Physics.OverlapSphere(position, data.explosionRadius);
      foreach (Collider col in colliders)
      {
        float distance = Vector3.Distance(col.transform.position, position);
        float distanceDamageMultiplier = Mathf.Clamp01(1 - distance / data.explosionRadius);

        var colRb = col.GetComponent<Rigidbody>();
        if (colRb) colRb.AddExplosionForce(data.pushForce, position, data.explosionRadius);

        var damageable = col.GetComponentInParent<DamageController>();
        if (damageable != null) damageable.TakeDamage(new DamageData() { player = data.player, damage = data.damage * distanceDamageMultiplier });
      }

      Destroy(gameObject);
    }

    void Hit(Collision collision)
    {
      active = true;

      var colRb = collision.gameObject.GetComponent<Rigidbody>();
      if (colRb) colRb.AddForce(transform.position - collision.transform.position * data.pushForce);

      var damageable = collision.gameObject.GetComponentInParent<DamageController>();
      if (damageable != null) damageable.TakeDamage(new DamageData() { player = data.player, damage = data.damage });

      if (impactParticlePrefab)
      {
        GameObject impactParticle = Instantiate(
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
      rigidbody.AddForce(data.forward * data.force, ForceMode.VelocityChange);

      if (!muzzleParticlePrefab) return;
      GameObject muzzleParticle = Instantiate(muzzleParticlePrefab, transform.position, transform.rotation);
      int layer = LayerMask.NameToLayer("First Person Objects");
      muzzleParticle.SetLayerRecursively(layer);
      Destroy(muzzleParticle, 1.5f);
    }

    void OnCollisionEnter(Collision collision)
    {
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