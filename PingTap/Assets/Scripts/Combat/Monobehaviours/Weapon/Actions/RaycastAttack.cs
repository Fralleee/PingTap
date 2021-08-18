using Fralle.Core;
using Fralle.Core.Pooling;
using UnityEngine;

namespace Fralle.PingTap
{
  public class RaycastAttack : AttackAction
  {
    [Header("RaycastAttack")]
    public float Range = 50;
    public float PushForce = 3.5f;
    [SerializeField] GameObject muzzleParticlePrefab;
    [SerializeField] BulletTraceController bulletTraceController;
    [SerializeField] int bulletsPerFire = 1;
    public AnimationCurve RangeDamageFalloff = new AnimationCurve(new Keyframe(0, 1), new Keyframe(1, 0));

    [Header("Spread")]
    [SerializeField] float spreadRadius = 0f;
    [SerializeField] float spreadIncreaseEachShot = 0f;
    [SerializeField] float recovery = 1f;
    [SerializeField] bool spreadOnFirstShot;

    [Readonly] public float CurrentSpread;

    ParticleSystem muzzleParticle;
    float spreadStatMultiplier = 1f;

    internal override void Start()
    {
      base.Start();

      float lowestCurrentSpread = spreadOnFirstShot ? spreadIncreaseEachShot : 0f;
      CurrentSpread = lowestCurrentSpread;
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

      CurrentSpread += spreadIncreaseEachShot * spreadStatMultiplier;
      CurrentSpread = Mathf.Clamp(CurrentSpread, 0, spreadRadius * spreadStatMultiplier);
    }

    public override float GetRange() => Range;

    void SetupMuzzle()
    {
      GameObject instance = Instantiate(muzzleParticlePrefab, Combatant.AimTransform.position, Combatant.AimTransform.rotation, Combatant.AimTransform);
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
      Combatant.Stats.OnAttack(1);

      Vector3 forward = CalculateBulletSpread(1 / Combatant.Modifiers.ExtraAccuracy);

      Debug.DrawRay(Combatant.AimTransform.position, forward, Color.red);
      if (!Physics.Raycast(Combatant.AimTransform.position, forward, out RaycastHit hitInfo, Range, Combatant.teamController.AttackLayerMask))
      {
        BulletTrace(muzzle.position, muzzle.position + forward * Range);
        return;
      }

      DamageData damageData = DamageHelper.RaycastHit(this, hitInfo);

      BulletTrace(muzzle.position, hitInfo.point);

      if (damageData != null && damageData.ImpactEffect != null)
        ObjectPool.Spawn(damageData.ImpactEffect, hitInfo.point, Quaternion.LookRotation(hitInfo.normal, Vector3.up));
      else
        ObjectPool.Spawn(Combatant.impactAtlas.GetImpactEffectFromTag(hitInfo.collider.tag), hitInfo.point, Quaternion.LookRotation(hitInfo.normal, Vector3.up));
    }

    void ModifyCurrentSpread()
    {
      float lowestCurrentSpread = spreadOnFirstShot ? spreadIncreaseEachShot : 0f;
      if (CurrentSpread.EqualsWithTolerance(lowestCurrentSpread))
        return;

      CurrentSpread -= Time.deltaTime * recovery;
      CurrentSpread = Mathf.Clamp(CurrentSpread, lowestCurrentSpread, spreadRadius);
    }

    Vector3 CalculateBulletSpread(float modifier)
    {
      float spreadPercent = spreadIncreaseEachShot > 0 ? CurrentSpread : 1;
      Vector2 spread = spreadPercent * modifier * Random.insideUnitCircle * spreadRadius;
      return Combatant.AimTransform.forward + new Vector3(0, spread.x, spread.y);
    }

    void BulletTrace(Vector3 origin, Vector3 target)
    {
      if (!bulletTraceController)
        return;

      bulletTraceController.Trace(origin, target);
    }

#if UNITY_EDITOR
    internal override void OnValidate()
    {
      base.OnValidate();
      float lowestCurrentSpread = spreadOnFirstShot ? spreadIncreaseEachShot : 0f;
      CurrentSpread = lowestCurrentSpread;
    }
#endif
  }
}
