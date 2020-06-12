using Fralle.Attack.Offense;
using System.Collections;
using UnityEngine;

namespace Fralle.Attack.Action
{
  public class Launcher : AttackAction
  {
    [Header("Launcher")]
    [SerializeField] Projectile projectilePrefab = null;
    [SerializeField] ProjectileData projectileData = null;

    [Space(10)]
    [SerializeField] int projectilesPerFire = 0;
    [SerializeField] float delayTimePerProjectiles = 0f;
    [SerializeField] float radiusOnMaxRange = 0f;

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
        player.stats.ReceiveShotsFired(1);

        var layerMask = ~LayerMask.GetMask("Corpse");
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