using Fralle.Attack.Offense;
using Fralle.Core.Attributes;
using System.Linq;
using UnityEngine;

namespace Fralle.Attack.Action
{
  public class Hitscan : AttackAction
  {
    [Header("Hitscan")] [SerializeField] float range = 50;
    [SerializeField] float pushForce = 3.5f;
    [SerializeField] GameObject impactParticlePrefab;
    [SerializeField] GameObject muzzleParticlePrefab;
    [SerializeField] GameObject lineRendererPrefab;
    [SerializeField] int bulletsPerFire = 1;

    [Header("Spread")]
    [SerializeField] float spreadRadius;
    [SerializeField] float spreadIncreaseEachShot;
    [SerializeField] float recovery = 1f;

    [Readonly] public float currentSpread;

    internal override void Update()
    {
      base.Update();
      if (currentSpread == 0) return;
      currentSpread -= Time.deltaTime * recovery;
      currentSpread = Mathf.Clamp(currentSpread, 0, spreadRadius);
    }

    public override void Fire()
    {
      var muzzle = GetMuzzle();

      for (var i = 0; i < bulletsPerFire; i++) FireBullet(muzzle);

      if (!(spreadIncreaseEachShot > 0)) return;
      currentSpread += spreadIncreaseEachShot;
      currentSpread = Mathf.Clamp(currentSpread, 0, spreadRadius);
    }

    void FireBullet(Transform muzzle)
    {
      player.stats.ReceiveShotsFired(1);

      var forward = CalculateBulletSpread();

      if (muzzleParticlePrefab)
      {
        var muzzleParticle = Instantiate(muzzleParticlePrefab, muzzle.position, transform.rotation, muzzle);
        Destroy(muzzleParticle, 1.5f);
      }

      int layerMask = ~LayerMask.GetMask("Corpse", "Enemy Rigidbody");
      if (!Physics.Raycast(weapon.playerCamera.position, forward, out var hitInfo, range, layerMask)) return;

      AddForce(hitInfo);

      var health = hitInfo.transform.GetComponent<Health>();
      if (health != null)
      {
        player.stats.ReceiveHits(1);

        float damageAmount = Damage;
        health.ReceiveAttack(new Damage()
        {
          player = player,
          element = element,
          effects = damageEffects.Select(x => x.Setup(player, damageAmount)).ToArray(),
          hitAngle = Vector3.Angle((weapon.transform.position - hitInfo.transform.position).normalized, hitInfo.transform.forward),
          force = weapon.playerCamera.forward * pushForce,
          position = hitInfo.point,
          damageAmount = damageAmount
        });
      }

      BulletTrace(muzzle.position, hitInfo.point);

      if (impactParticlePrefab)
      {
        var impactParticle = Instantiate(impactParticlePrefab, hitInfo.point, Quaternion.FromToRotation(Vector3.up, hitInfo.normal));
        Destroy(impactParticle, 5f);
      }
    }

    Vector3 CalculateBulletSpread()
    {
      float spreadPercent = spreadIncreaseEachShot > 0 ? currentSpread : 1;
      var spread = spreadPercent * Random.insideUnitCircle * spreadRadius;
      return weapon.playerCamera.forward + new Vector3(0, spread.y, spread.x);
    }

    void BulletTrace(Vector3 origin, Vector3 target)
    {
      if (!lineRendererPrefab) return;
      var lineRendererInstance = Instantiate(lineRendererPrefab);
      var lineRenderer = lineRendererInstance.GetComponent<LineRenderer>();
      lineRenderer.SetPosition(0, origin);
      lineRenderer.SetPosition(1, target);
    }

    void AddForce(RaycastHit hitInfo)
    {
      var rigidBody = hitInfo.transform.GetComponent<Rigidbody>();
      if (rigidBody != null) rigidBody.AddForce(weapon.playerCamera.forward * pushForce);
    }
  }
}