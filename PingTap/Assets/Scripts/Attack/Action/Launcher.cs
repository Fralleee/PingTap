using Fralle.Attack.Offense;
using System.Collections;
using UnityEngine;

namespace Fralle.Attack.Action
{
  public class Launcher : AttackAction
  {
    [Header("Launcher")]
    [SerializeField] Projectile projectilePrefab;
    [SerializeField] ProjectileData projectileData;

    [Space(10)]
    [SerializeField] int projectilesPerFire;
    [SerializeField] float delayTimePerProjectiles;
    [SerializeField] float radiusOnMaxRange;

    public override void Fire()
    {
      var muzzle = GetMuzzle();

      projectileData.player = player;
      projectileData.forward = weapon.playerCamera.forward;
      projectileData.damage = Damage;
      projectileData.element = element;
      projectileData.damageEffects = damageEffects;

      StartCoroutine(SpawnProjectiles(muzzle));
    }

    IEnumerator SpawnProjectiles(Transform muzzle)
    {
      for (var i = 0; i < projectilesPerFire; i++)
      {
        int layerMask = ~LayerMask.GetMask("Corpse");
        if (Physics.Raycast(weapon.playerCamera.position, weapon.playerCamera.forward, out var hitInfo, projectileData.range, layerMask))
          projectileData.forward = (hitInfo.point - muzzle.position).normalized;

        var spread = (Random.insideUnitCircle * radiusOnMaxRange) / projectileData.range;
        projectileData.forward += new Vector3(0, spread.y, spread.x);

        var projectile = Instantiate(projectilePrefab, muzzle.position, transform.rotation);
        projectile.Initiate(projectileData);
        yield return new WaitForSeconds(delayTimePerProjectiles);
      }
    }
  }
}