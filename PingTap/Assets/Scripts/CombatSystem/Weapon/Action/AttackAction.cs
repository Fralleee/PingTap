using CombatSystem.Addons;
using CombatSystem.Combat;
using CombatSystem.Effect;
using CombatSystem.Enums;
using CombatSystem.Interfaces;
using Fralle.Core.Enums;
using System.Collections;
using UnityEngine;

namespace CombatSystem.Action
{
  [RequireComponent(typeof(Weapon))]
  [RequireComponent(typeof(AmmoAddon))]
  [RequireComponent(typeof(RecoilAddon))]
  public abstract class AttackAction : MonoBehaviour, IAction
  {
    public MouseButton Button { get; set; }

    [Header("Shooting")]
    [SerializeField] internal float minDamage = 1;
    [SerializeField] internal float maxDamage = 10;
    [SerializeField] internal int ammoPerShot = 1;
    [SerializeField] internal int shotsPerSecond = 20;
    [SerializeField] internal bool tapable = false;
    [SerializeField] internal Element element = Element.Physical;
    [SerializeField] internal DamageEffect[] damageEffects = new DamageEffect[0];

    internal Weapon weapon;
    internal Combatant attacker;
    int nextMuzzle;

    internal float Damage => Random.Range(minDamage, maxDamage);
    bool HasAmmo => weapon.ammoAddonController && weapon.ammoAddonController.HasAmmo();

    internal virtual void Start()
    {
      weapon = GetComponent<Weapon>();
      attacker = weapon.GetComponentInParent<Combatant>();
    }

    public void Perform()
    {
      if (weapon.ActiveWeaponAction != Status.Ready) return;

      Fire();

      if (HasAmmo) weapon.ammoAddonController.ChangeAmmo(-ammoPerShot);
      if (weapon.recoilAddon) weapon.recoilAddon.AddRecoil();
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