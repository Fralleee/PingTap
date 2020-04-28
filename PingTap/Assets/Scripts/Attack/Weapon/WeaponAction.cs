using System.Collections;
using UnityEngine;

namespace Fralle.Attack
{
  [RequireComponent(typeof(Weapon))]
  [RequireComponent(typeof(Ammo))]
  [RequireComponent(typeof(Recoil))]
  public abstract class WeaponAction : Action
  {
    [Header("Shooting")] [SerializeField] internal MouseButton fireInput = MouseButton.Left;
    [SerializeField] internal float minDamage = 1;
    [SerializeField] internal float maxDamage = 10;
    [SerializeField] internal int ammoPerShot = 1;
    [SerializeField] internal int shotsPerSecond = 20;
    [SerializeField] internal bool tapable = false;
    [SerializeField] internal Element element;
    [SerializeField] internal DamageEffect[] damageEffects;

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
      bool shootWeapon =
        (tapable ? Input.GetMouseButtonDown((int)fireInput) : Input.GetMouseButton((int)fireInput)) &&
        weapon.ActiveWeaponAction == WeaponStatus.Ready;
      if (!shootWeapon) return;

      if (HasAmmo) weapon.ammoController.ChangeAmmo(-ammoPerShot);
      Fire();
      if (weapon.recoil) weapon.recoil.AddRecoil();
      if (HasAmmo) StartCoroutine(ShootingCooldown());
    }

    public abstract void Fire();

    internal IEnumerator ShootingCooldown()
    {
      weapon.ChangeWeaponAction(WeaponStatus.Firing);
      yield return new WaitForSeconds(1f / shotsPerSecond);
      weapon.ChangeWeaponAction(WeaponStatus.Ready);
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
}