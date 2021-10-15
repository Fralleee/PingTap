using Fralle.Core;
using Fralle.Core.Pooling;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Fralle.PingTap
{
  public class RaycastAttack : AttackAction
  {
    [Header("RaycastAttack")]
    public float range = 50;
    public float pushForce = 3.5f;
    [SerializeField] GameObject muzzleParticlePrefab;
    [SerializeField] BulletTraceController bulletTraceController;
    [SerializeField] int bulletsPerFire = 1;
    public AnimationCurve rangeDamageFalloff = new AnimationCurve(new Keyframe(0, 1), new Keyframe(1, 0));

    [Header("BulletSpread")]
    [SerializeField] float bulletSpreadRadius = 0f;
    [SerializeField] float bulletSpreadIncreaseEachShot = 0f;
    [SerializeField] float bulletSpreadRecovery = 1f;
    [SerializeField] bool bulletSpreadOnFirstShot;

    [ReadOnly] public float currentBulletSpread;

    ParticleSystem muzzleParticle;
    float bulletSpreadStatMultiplier = 1f;

    internal override void Start()
    {
      base.Start();

      float lowestCurrentBulletSpread = bulletSpreadOnFirstShot ? bulletSpreadIncreaseEachShot : 0f;
      currentBulletSpread = lowestCurrentBulletSpread;
      SetupMuzzle();
      SetupBulletTrace();
    }

    void Update()
    {
      ModifyCurrentBulletSpread();
    }

    public override void Fire()
    {
      Transform muzzle = GetMuzzle();

      muzzleParticle.transform.position = muzzle.position;
      muzzleParticle.Play();

      for (int i = 0; i < bulletsPerFire; i++)
        FireBullet(muzzle);

      if (bulletSpreadIncreaseEachShot <= 0)
        return;

      currentBulletSpread += bulletSpreadIncreaseEachShot * bulletSpreadStatMultiplier;
      currentBulletSpread = Mathf.Clamp(currentBulletSpread, 0, bulletSpreadRadius * bulletSpreadStatMultiplier);
    }

    public override float GetRange() => range;

    void SetupMuzzle()
    {
      GameObject instance = Instantiate(muzzleParticlePrefab, Combatant.aimTransform.position, Combatant.aimTransform.rotation, Combatant.aimTransform);
      if (Combatant.gameObject.CompareTag("Player"))
        instance.SetLayerRecursively(LayerMask.NameToLayer("FPO")); // this should only be performed on localplayer
      muzzleParticle = instance.GetComponent<ParticleSystem>();
    }

    void SetupBulletTrace()
    {
      if (bulletTraceController)
        bulletTraceController.gameObject.SetLayerRecursively(LayerMask.NameToLayer("Default"));
    }

    void FireBullet(Transform muzzle)
    {
      Combatant.stats.OnAttack(1);

      Vector3 forward = CalculateBulletSpread(1 / Combatant.modifiers.ExtraAccuracy);

      if (!Physics.Raycast(Combatant.aimTransform.position, forward, out RaycastHit hitInfo, range, Combatant.teamController.attackLayerMask))
      {
        BulletTrace(muzzle.position, muzzle.position + forward * range);
        return;
      }

      DamageData damageData = DamageHelper.RaycastHit(this, hitInfo);

      BulletTrace(muzzle.position, hitInfo.point);

      if (damageData != null && damageData.ImpactEffect)
        ObjectPool.Spawn(damageData.ImpactEffect, hitInfo.point, Quaternion.LookRotation(hitInfo.normal, Vector3.up));
      else
        ObjectPool.Spawn(Combatant.impactAtlas.GetImpactEffectFromTag(hitInfo.collider.tag), hitInfo.point, Quaternion.LookRotation(hitInfo.normal, Vector3.up));
    }

    void ModifyCurrentBulletSpread()
    {
      float lowestCurrentBulletSpread = bulletSpreadOnFirstShot ? bulletSpreadIncreaseEachShot : 0f;
      if (currentBulletSpread.EqualsWithTolerance(lowestCurrentBulletSpread))
        return;

      currentBulletSpread -= Time.deltaTime * bulletSpreadRecovery;
      currentBulletSpread = Mathf.Clamp(currentBulletSpread, lowestCurrentBulletSpread, bulletSpreadRadius);
    }

    Vector3 CalculateBulletSpread(float modifier)
    {
      float spreadPercent = bulletSpreadIncreaseEachShot > 0 ? currentBulletSpread : 1;
      Vector3 spread = Combatant.aimTransform.TransformDirection(spreadPercent * modifier * Random.insideUnitCircle * bulletSpreadRadius);
      return Combatant.aimTransform.forward + spread;
    }

    void BulletTrace(Vector3 origin, Vector3 target)
    {
      if (!bulletTraceController)
        return;

      bulletTraceController.Trace(origin, target);
    }

    internal override void OnValidate()
    {
      base.OnValidate();
      float lowestCurrentSpread = bulletSpreadOnFirstShot ? bulletSpreadIncreaseEachShot : 0f;
      currentBulletSpread = lowestCurrentSpread;
    }
  }
}
