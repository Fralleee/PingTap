using Fralle.PingTap;
using Sirenix.OdinInspector;
using System.Linq;
using UnityEngine;

namespace Fralle.Abilities.Turret
{
  [RequireComponent(typeof(Combatant))]
  public class TurretAttack : MonoBehaviour
  {
    [ReadOnly] public Weapon EquippedWeapon;

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
      if (weapons.Length > 0)
        EquipWeapon(weapons[0]);
    }

    void Update()
    {
      SwapWeapon();
      FireInput();
    }

    void EquipWeapon(Weapon weapon)
    {
      //if (EquippedWeapon && EquippedWeapon.WeaponName == weapon.WeaponName)
      //  return;
      //if (EquippedWeapon)
      //  Destroy(EquippedWeapon.gameObject);

      //EquippedWeapon = Instantiate(weapon, combatant.weaponHolder.position, combatant.weaponHolder.rotation, combatant.weaponHolder);
      //EquippedWeapon.Equip(combatant);
      GetMaxRange();
    }

    void GetMaxRange()
    {
      AttackAction[] attackActions = EquippedWeapon.GetComponentsInChildren<AttackAction>();
      float longestRange = attackActions.Select(attackAction => attackAction.GetRange()).Prepend(0f).Max();
      turret.Range = Mathf.Min(longestRange, 200);
    }

    void SwapWeapon()
    {
      for (int i = 1; i <= weapons.Length; i++)
        if (Input.GetKeyDown("" + i))
          EquipWeapon(weapons[i - 1]);
    }

    void FireInput()
    {
      if (!turret.Target || !turret.IsDeployed)
        return;

      combatant.PrimaryAction(true);
      combatant.SecondaryAction(true);
    }
  }
}
