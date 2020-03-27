using UnityEngine;

namespace Fralle
{
  [RequireComponent(typeof(Rigidbody))]
  public class Projectile : MonoBehaviour
  {
    new Rigidbody rigidbody;
    ProjectileData data;
    bool active;

    public void Initiate(ProjectileData inputData)
    {
      rigidbody = GetComponentInChildren<Rigidbody>();
      data = inputData;
      rigidbody.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
      rigidbody.useGravity = data.useGravity;
      rigidbody.AddForce(data.launcherCamera.forward * data.force, ForceMode.VelocityChange);
    }


    void OnCollisionEnter(Collision collision)
    {
      active = true;

      var position = collision.GetContact(0).point;
      if (data.kinematicOnImpact) rigidbody.isKinematic = true;
      if (data.explosionRadius > 0)
      {
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
      }
      else
      {
        var colRb = collision.gameObject.GetComponent<Rigidbody>();
        if (colRb) colRb.AddForce(position - collision.transform.position * data.pushForce);
        var damageable = collision.gameObject.GetComponent<IDamageable>();
        if (damageable != null) damageable.TakeDamage(data.explosionDamage);
      }

      Destroy(gameObject, data.destroyOnImpactTime);
    }

    void OnDrawGizmos()
    {
      if (data.explosionRadius > 0 && active) Gizmos.DrawWireSphere(transform.position, data.explosionRadius);
    }
  }
}