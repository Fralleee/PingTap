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

      Projectile projectile = Instantiate(projectilePrefab, muzzle.position, transform.rotation);

      projectileData.launcherCamera = weapon.playerCamera;
      projectile.Initiate(projectileData);
    }
  }

}