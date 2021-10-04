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

    [Header("Spread")]
    [SerializeField] float spreadRadius = 0f;
    [SerializeField] float spreadIncreaseEachShot = 0f;
    [SerializeField] float recovery = 1f;
    [SerializeField] bool spreadOnFirstShot;

    [ReadOnly] public float currentSpread;

    ParticleSystem muzzleParticle;
    float spreadStatMultiplier = 1f;

    internal override void Start()
    {
      base.Start();

      float lowestCurrentSpread = spreadOnFirstShot ? spreadIncreaseEachShot : 0f;
      currentSpread = lowestCurrentSpread;
      SetupMuzzle();
      SetupBulletTrace();
    }

    void Update()
    {
      ModifyCurrentSpread();
    }

    public override void Fire()
    {
      Transform muzzle = GetMuzzle();

      muzzleParticle.transform.position = muzzle.position;
      muzzleParticle.Play();

      for (int i = 0; i < bulletsPerFire; i++)
        FireBullet(muzzle);

      if (spreadIncreaseEachShot <= 0)
        return;

      currentSpread += spreadIncreaseEachShot * spreadStatMultiplier;
      currentSpread = Mathf.Clamp(currentSpread, 0, spreadRadius * spreadStatMultiplier);
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

    void ModifyCurrentSpread()
    {
      float lowestCurrentSpread = spreadOnFirstShot ? spreadIncreaseEachShot : 0f;
      if (currentSpread.EqualsWithTolerance(lowestCurrentSpread))
        return;

      currentSpread -= Time.deltaTime * recovery;
      currentSpread = Mathf.Clamp(currentSpread, lowestCurrentSpread, spreadRadius);
    }

    Vector3 CalculateBulletSpread(float modifier)
    {
      float spreadPercent = spreadIncreaseEachShot > 0 ? currentSpread : 1;
      Vector3 spread = Combatant.aimTransform.TransformDirection(spreadPercent * modifier * Random.insideUnitCircle * spreadRadius);
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
      float lowestCurrentSpread = spreadOnFirstShot ? spreadIncreaseEachShot : 0f;
      currentSpread = lowestCurrentSpread;
    }
  }
}
