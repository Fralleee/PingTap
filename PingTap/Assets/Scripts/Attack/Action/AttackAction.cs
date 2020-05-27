using Fralle.Attack.Addons;
using Fralle.Attack.Effect;
using Fralle.Core.Enums;
using System.Collections;
using UnityEngine;

namespace Fralle.Attack.Action
{
  [RequireComponent(typeof(Weapon))]
  [RequireComponent(typeof(Ammo))]
  [RequireComponent(typeof(Recoil))]
  public abstract class AttackAction : Active
  {
    [Header("Shooting")]
    [SerializeField] internal MouseButton fireInput = MouseButton.Left;
    [SerializeField] internal float minDamage = 1;
    [SerializeField] internal float maxDamage = 10;
    [SerializeField] internal int ammoPerShot = 1;
    [SerializeField] internal int shotsPerSecond = 20;
    [SerializeField] internal bool tapable = false;
    [SerializeField] internal Element element;
    [SerializeField] internal DamageEffect[] damageEffects;

    internal Weapon weapon;
    internal Player player;
    PlayerInputController input;
    int nextMuzzle;

    internal float Damage => Random.Range(minDamage, maxDamage);
    bool HasAmmo => weapon.ammoController && weapon.ammoController.HasAmmo();

    internal virtual void Start()
    {
      weapon = GetComponent<Weapon>();
      player = weapon.GetComponentInParent<Player>();
      input = weapon.GetComponentInParent<PlayerInputController>();
    }

    internal virtual void Update()
    {
      var shootWeapon = input.GetMouseButton(fireInput, tapable ? MouseButtonState.Down : MouseButtonState.Hold) && weapon.ActiveWeaponAction == Status.Ready;
      if (!shootWeapon) return;

      if (HasAmmo) weapon.ammoController.ChangeAmmo(-ammoPerShot);
      Fire();
      if (weapon.recoil) weapon.recoil.AddRecoil();
      if (HasAmmo) StartCoroutine(ShootingCooldown());
    }

    public abstract void Fire();

    internal IEnumerator ShootingCooldown()
    {
      weapon.ChangeWeaponAction(Status.Firing);
      yield return new WaitForSeconds(1f / shotsPerSecond);
      weapon.ChangeWeaponAction(Status.Ready);
    }

    internal Transform GetMuzzle()
    {
      var muzzle = weapon.muzzles[nextMuzzle];
      if (weapon.muzzles.Length <= 1) return muzzle;

      nextMuzzle++;
      if (nextMuzzle > weapon.muzzles.Length - 1) nextMuzzle = 0;

      return muzzle;
    }
  }
}