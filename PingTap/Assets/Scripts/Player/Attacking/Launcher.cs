using UnityEngine;

namespace Fralle
{
  public class Launcher : WeaponAction
  {
    [Header("Launcher")]
    [SerializeField] Projectile projectilePrefab;
    [SerializeField] ProjectileData projectileData;

    public override void Fire()
    {
      Transform muzzle = GetMuzzle();

      projectileData.player = player;
      projectileData.forward = weapon.playerCamera.forward;
      projectileData.damage = Damage;

      int layerMask = ~LayerMask.GetMask("Corpse");
      if (Physics.Raycast(weapon.playerCamera.position, weapon.playerCamera.forward, out RaycastHit hitInfo, projectileData.range, layerMask))
        projectileData.forward = (hitInfo.point - muzzle.position).normalized;

      Projectile projectile = Instantiate(projectilePrefab, muzzle.position, transform.rotation);

      projectile.Initiate(projectileData);
    }
  }

}