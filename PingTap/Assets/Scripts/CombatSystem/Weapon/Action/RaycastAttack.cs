using CombatSystem.Combat.Damage;
using Fralle.Core.Attributes;
using Fralle.Core.Extensions;
using System.Linq;
using UnityEngine;

namespace CombatSystem.Action
{
  public class RaycastAttack : AttackAction
  {
    [Header("RaycastAttack")]
    [SerializeField] float range = 50;
    [SerializeField] float pushForce = 3.5f;
    [SerializeField] GameObject impactParticlePrefab = null;
    [SerializeField] GameObject muzzleParticlePrefab = null;
    [SerializeField] GameObject lineRendererPrefab = null;
    [SerializeField] int bulletsPerFire = 1;

    [Header("Spread")]
    [SerializeField] float spreadRadius = 0f;
    [SerializeField] float spreadIncreaseEachShot = 0f;
    [SerializeField] float recovery = 1f;

    [Readonly] public float currentSpread;

    void Update()
    {
      if (currentSpread.EqualsWithTolerance(0f)) return;
      currentSpread -= Time.deltaTime * recovery;
      currentSpread = Mathf.Clamp(currentSpread, 0, spreadRadius);
    }

    public override void Fire()
    {
      var muzzle = GetMuzzle();
      if (muzzleParticlePrefab)
      {
        var muzzleParticle = Instantiate(muzzleParticlePrefab, muzzle.position, attacker.aimTransform.rotation, attacker.aimTransform);
        Destroy(muzzleParticle, 1.5f);
      }

      for (var i = 0; i < bulletsPerFire; i++) FireBullet(muzzle);

      if (!(spreadIncreaseEachShot > 0)) return;
      currentSpread += spreadIncreaseEachShot;
      currentSpread = Mathf.Clamp(currentSpread, 0, spreadRadius);
    }

    void FireBullet(Transform muzzle)
    {
      attacker.Stats.OnAttack(1);

      var forward = CalculateBulletSpread(1 / attacker.Modifiers.extraAccuracy);

      var layerMask = ~LayerMask.GetMask("Corpse", "Enemy Rigidbody");
      if (!Physics.Raycast(attacker.aimTransform.position, forward, out var hitInfo, range, layerMask)) return;

      AddForce(hitInfo);

      var damageController = hitInfo.transform.GetComponent<DamageController>();
      if (damageController != null)
      {
        // this will cause issues if we are for example hitting targets with shotgun
        // we will receive more hits than shots fired

        var damageAmount = Damage;
        var damageData = new DamageData()
        {
          attacker = attacker,
          element = element,
          effects = damageEffects.Select(x => x.Setup(attacker, damageAmount)).ToArray(),
          hitAngle = Vector3.Angle((weapon.transform.position - hitInfo.transform.position).normalized, hitInfo.transform.forward),
          force = attacker.aimTransform.forward * pushForce,
          position = hitInfo.point,
          damageAmount = damageAmount
        };

        damageController.ReceiveAttack(damageData);
      }

      BulletTrace(muzzle.position, hitInfo.point);

      if (impactParticlePrefab)
      {
        var impactParticle = Instantiate(impactParticlePrefab, hitInfo.point, Quaternion.FromToRotation(Vector3.up, hitInfo.normal));
        Destroy(impactParticle, 5f);
      }
    }

    Vector3 CalculateBulletSpread(float modifier)
    {
      var spreadPercent = spreadIncreaseEachShot > 0 ? currentSpread : 1;
      var spread = spreadPercent * modifier * Random.insideUnitCircle * spreadRadius;
      return attacker.aimTransform.forward + new Vector3(0, spread.y, spread.x);
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
      if (rigidBody != null) rigidBody.AddForce(attacker.aimTransform.forward * pushForce);
    }
  }
}