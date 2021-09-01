using Fralle.Core;
using Fralle.Core.Pooling;
using System.Collections;
using UnityEngine;

namespace Fralle.PingTap
{
  public class ProjectileAttack : AttackAction
  {
    [Header("ProjectileAttack")]
    [SerializeField] GameObject muzzleParticlePrefab;
    [SerializeField] Projectile projectilePrefab;
    [SerializeField] ProjectileData projectileData;

    [Space(10)]
    [SerializeField] int projectilesPerFire = 1;
    [SerializeField] float delayTimePerProjectiles = 0f;
    [SerializeField] float spreadRadiusOnMaxRange = 0f;

    ParticleSystem muzzleParticle;

    internal override void Start()
    {
      base.Start();

      SetupMuzzle();
    }

    void SetupMuzzle()
    {
      GameObject instance = Instantiate(muzzleParticlePrefab, Combatant.aimTransform.position, Combatant.aimTransform.rotation, Combatant.aimTransform);
      if (Combatant.gameObject.CompareTag("Player"))
        instance.SetLayerRecursively(LayerMask.NameToLayer("FPO")); // this should only be performed on localplayer
      muzzleParticle = instance.GetComponent<ParticleSystem>();
    }

    public override void Fire()
    {
      Transform muzzle = GetMuzzle();

      muzzleParticle.transform.position = muzzle.position;
      muzzleParticle.Play();

      projectileData.Attacker = Combatant;
      projectileData.Forward = Weapon.Combatant.aimTransform.forward;
      projectileData.Damage = Damage;
      projectileData.Element = Element;
      projectileData.DamageEffects = DamageEffects;
      projectileData.HitboxLayer = HitboxLayer;


      StartCoroutine(SpawnProjectiles(muzzle));
    }

    public override float GetRange() => projectileData.Range;

    IEnumerator SpawnProjectiles(Transform muzzle)
    {
      for (int i = 0; i < projectilesPerFire; i++)
      {
        Combatant.stats.OnAttack(1);

        Ray ray = new Ray(Weapon.Combatant.aimTransform.position, Weapon.Combatant.aimTransform.forward);
        if (Physics.Raycast(ray, out RaycastHit hitInfo, projectileData.Range, Weapon.Combatant.teamController.attackLayerMask))
          projectileData.Forward = (hitInfo.point - muzzle.position).normalized;
        else
          projectileData.Forward = (ray.GetPoint(Mathf.Min(projectileData.Range, 50f)) - muzzle.position).normalized;

        Vector2 spread = (Random.insideUnitCircle * spreadRadiusOnMaxRange) / projectileData.Range;
        projectileData.Forward += new Vector3(0, spread.y, spread.x);

        GameObject instance = ObjectPool.Spawn(projectilePrefab.gameObject, muzzle.position, Quaternion.LookRotation(projectileData.Forward, transform.up));
        Projectile projectile = instance.GetComponent<Projectile>();
        projectile.gameObject.layer = Combatant.teamController.allyProjectiles;
        projectile.Initiate(projectileData);

        yield return new WaitForSeconds(delayTimePerProjectiles);
      }
    }
  }
}
