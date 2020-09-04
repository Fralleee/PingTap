using CombatSystem;
using CombatSystem.Action;
using CombatSystem.Combat;
using Fralle.Core.Attributes;
using System.Linq;
using UnityEngine;

namespace Fralle.Abilities.Turret
{
  [RequireComponent(typeof(Combatant))]
  public class TurretAttack : MonoBehaviour
  {
    [Readonly] public Weapon equippedWeapon;

    [SerializeField] Weapon[] weapons = new Weapon[0];

    TurretController turret;
    Combatant combatant;

    void Awake()
    {
      turret = GetComponent<TurretController>();
      combatant = GetComponent<Combatant>();
    }

    void Start()
    {
      if (weapons.Length > 0) EquipWeapon(weapons[0]);
    }

    void Update()
    {
      SwapWeapon();
      FireInput();
    }

    void EquipWeapon(Weapon weapon)
    {
      if (equippedWeapon && equippedWeapon.weaponName == weapon.weaponName) return;
      if (equippedWeapon) Destroy(equippedWeapon.gameObject);

      equippedWeapon = Instantiate(weapon, combatant.weaponHolder.position, combatant.weaponHolder.rotation, combatant.weaponHolder);
      equippedWeapon.Equip(combatant);
      GetMaxRange();
    }

    void GetMaxRange()
    {
      var attackActions = equippedWeapon.GetComponentsInChildren<AttackAction>().ToList();
      var longestRange = attackActions.OrderBy(x => x.GetRange()).FirstOrDefault().GetRange();
      turret.range = Mathf.Min(longestRange, 200);
    }

    void SwapWeapon()
    {
      for (var i = 1; i <= weapons.Length; i++)
        if (Input.GetKeyDown("" + i))
          EquipWeapon(weapons[i - 1]);
    }

    void FireInput()
    {
      if (!turret.target || !turret.IsDeployed) return;

      combatant.PrimaryAction(true);
      combatant.SecondaryAction(true);
    }
  }
}
