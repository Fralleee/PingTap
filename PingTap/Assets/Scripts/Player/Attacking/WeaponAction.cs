using Fralle;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Weapon))]
[RequireComponent(typeof(AmmoController))]
[RequireComponent(typeof(RecoilController))]
public abstract class WeaponAction : PlayerAction
{
  [Header("Shooting")]
  [SerializeField] internal MouseButton fireInput = MouseButton.Left;
  [SerializeField] internal float minDamage = 1;
  [SerializeField] internal float maxDamage = 10;
  [SerializeField] internal int ammoPerShot = 1;
  [SerializeField] internal int shotsPerSecond = 20;
  [SerializeField] internal bool tapable = false;

  internal Weapon weapon;
  internal Player player;
  int nextMuzzle;

  internal float Damage => Random.Range(minDamage, maxDamage);
  bool HasAmmo => weapon.ammoController && weapon.ammoController.HasAmmo();

  internal virtual void Start()
  {
    weapon = GetComponent<Weapon>();
    player = weapon.GetComponentInParent<Player>();
  }

  internal virtual void Update()
  {
    bool shootWeapon = (tapable ? Input.GetMouseButtonDown((int)fireInput) : Input.GetMouseButton((int)fireInput)) && weapon.activeWeaponAction == ActiveWeaponAction.READY;
    if (!shootWeapon) return;

    if (HasAmmo) weapon.ammoController.ChangeAmmo(-ammoPerShot);
    Fire();
    if (weapon.recoilController) weapon.recoilController.AddRecoil();
    if (HasAmmo) StartCoroutine(ShootingCooldown());
  }

  public abstract void Fire();

  internal IEnumerator ShootingCooldown()
  {
    weapon.activeWeaponAction = ActiveWeaponAction.FIRING;
    yield return new WaitForSeconds(1f / shotsPerSecond);
    weapon.activeWeaponAction = ActiveWeaponAction.READY;
  }

  internal Transform GetMuzzle()
  {
    Transform muzzle = weapon.muzzles[nextMuzzle];
    if (weapon.muzzles.Length <= 1) return muzzle;

    nextMuzzle++;
    if (nextMuzzle > weapon.muzzles.Length - 1) nextMuzzle = 0;

    return muzzle;
  }
}
