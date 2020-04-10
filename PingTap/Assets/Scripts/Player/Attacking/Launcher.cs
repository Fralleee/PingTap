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

      projectileData.player = weapon.GetComponentInParent<Player>();
      projectileData.forward = weapon.playerCamera.forward;

      if (Physics.Raycast(weapon.playerCamera.position, weapon.playerCamera.forward, out var hitInfo, projectileData.range))
        projectileData.forward = (hitInfo.point - muzzle.position).normalized;

      Projectile projectile = Instantiate(projectilePrefab, muzzle.position, transform.rotation);

      projectile.Initiate(projectileData);
    }
  }

}