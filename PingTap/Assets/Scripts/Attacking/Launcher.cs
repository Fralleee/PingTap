using Fralle;
using UnityEngine;

public class Launcher : WeaponAction
{
  [Header("Launcher", order = 0)]
  [SerializeField] Projectile projectilePrefab;
  [SerializeField] ProjectileData projectileData;

  public override void Fire()
  {
    var projectile = Instantiate(projectilePrefab, weapon.muzzle.position, transform.rotation);

    projectileData.launcherCamera = weapon.playerCamera;
    projectile.Initiate(projectileData);
  }
}
