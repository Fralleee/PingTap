using Fralle;
using UnityEngine;

public class Launcher : Weapon
{
  [Header("Launcher", order = 0)]
  [SerializeField] Projectile projectilePrefab;
  [SerializeField] ProjectileData projectileData;

  public override void Fire()
  {
    transform.localPosition -= new Vector3(0, 0, kickbackForce);

    var projectile = Instantiate(projectilePrefab, muzzle.position, transform.rotation);

    projectileData.launcherCamera = playerCamera;
    projectile.Initiate(projectileData);

    recoilController?.AddRecoil();
  }
}
