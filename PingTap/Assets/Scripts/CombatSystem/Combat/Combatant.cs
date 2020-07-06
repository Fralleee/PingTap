using CombatSystem.Action;
using Fralle.Core.Extensions;
using UnityEngine;

namespace CombatSystem.Combat
{
  public class Combatant : MonoBehaviour
  {
    public CombatStats Stats = new CombatStats();
    public CombatModifiers Modifiers = new CombatModifiers();

    public Transform aimTransform;
    public Transform weaponHolder;
    public Weapon weapon;

    Weapon equippedWeapon;
    AttackAction primaryAction;
    AttackAction secondaryAction;

    public void PrimaryAction(bool keyDown = false)
    {
      if (!equippedWeapon || !primaryAction || primaryAction.tapable && !keyDown) return;

      primaryAction.Perform();
    }

    public void SecondaryAction(bool keyDown = false)
    {
      if (!equippedWeapon || !secondaryAction || secondaryAction.tapable && !keyDown) return;

      secondaryAction.Perform();
    }

    public void SetFPSLayers(string layerName)
    {
      var layer = LayerMask.NameToLayer(layerName);
      equippedWeapon.gameObject.SetLayerRecursively(layer);
    }

    void Awake()
    {
      SetDefaults();
      EquipWeapon();
    }

    void SetDefaults()
    {
      if (aimTransform == null) aimTransform = transform;
      if (weaponHolder == null) weaponHolder = transform;
    }

    void EquipWeapon()
    {
      if (!weapon) return;

      equippedWeapon = Instantiate(weapon, weaponHolder.position, transform.rotation, weaponHolder);
      equippedWeapon.Equip(this);
      SetupAttackActions();
    }

    void SetupAttackActions()
    {
      var attackActions = equippedWeapon.GetComponentsInChildren<AttackAction>();
      if (attackActions.Length > 2) Debug.LogWarning($"Weapon {equippedWeapon} has more attack actions than possible (2).");
      else if (attackActions.Length > 0)
      {
        primaryAction = attackActions[0];
        secondaryAction = attackActions.Length == 2 ? attackActions[1] : null;
      }
    }
  }
}