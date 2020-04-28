using System.Collections;
using UnityEngine;

namespace Fralle.Attack
{
  public class Launcher : WeaponAction
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
      Transform muzzle = GetMuzzle();

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
        if (Physics.Raycast(weapon.playerCamera.position, weapon.playerCamera.forward, out RaycastHit hitInfo, projectileData.range, layerMask))
          projectileData.forward = (hitInfo.point - muzzle.position).normalized;

        Vector2 spread = (Random.insideUnitCircle * radiusOnMaxRange) / projectileData.range;
        projectileData.forward += new Vector3(0, spread.y, spread.x);

        Projectile projectile = Instantiate(projectilePrefab, muzzle.position, transform.rotation);
        projectile.Initiate(projectileData);
        yield return new WaitForSeconds(delayTimePerProjectiles);
      }
    }
  }
}