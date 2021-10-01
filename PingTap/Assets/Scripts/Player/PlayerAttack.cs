using Fralle.Core;
using UnityEngine;
using UnityEngine.InputSystem;
using static PlayerControls;

namespace Fralle.PingTap
{
  [RequireComponent(typeof(Combatant))]
  public class PlayerAttack : MonoBehaviour, IWeaponActions
  {
    public Transform weaponCamera;

    Combatant combatant;
    Vector3 defaultWeaponCameraPosition;
    Quaternion defaultWeaponCameraRotation;
    int firstPersonObjectsLayer;
    bool primaryFireHold;
    bool secondaryFireHold;

    void Awake()
    {
      firstPersonObjectsLayer = LayerMask.NameToLayer("FPO");

      combatant = GetComponent<Combatant>();
      combatant.OnWeaponSwitch += OnWeaponSwitch;

      defaultWeaponCameraPosition = weaponCamera.transform.localPosition;
      defaultWeaponCameraRotation = weaponCamera.transform.localRotation;
    }

    void Start()
    {
      Player.controls.Weapon.SetCallbacks(this);
      Player.controls.Weapon.Enable();
    }

    void Update()
    {
      if (primaryFireHold)
        combatant.PrimaryAction();
      else if (secondaryFireHold)
        combatant.SecondaryAction();
    }

    void OnWeaponSwitch(Weapon weapon, Weapon oldWeapon)
    {
      if (combatant.equippedWeapon == null)
      {
        weaponCamera.localPosition = defaultWeaponCameraPosition;
        weaponCamera.localRotation = defaultWeaponCameraRotation;
        return;
      }

      combatant.equippedWeapon.gameObject.SetLayerRecursively(firstPersonObjectsLayer);
      weaponCamera.localPosition = combatant.equippedWeapon.weaponCameraTransform.localPosition;
      weaponCamera.localRotation = combatant.equippedWeapon.weaponCameraTransform.localRotation;
    }

    void IWeaponActions.OnItemSelect(InputAction.CallbackContext context)
    {
      if (!context.performed)
        return;

      int weaponSlotIndex = (int)context.ReadValue<float>();
      combatant.EquipWeapon(weaponSlotIndex);
    }

    void IWeaponActions.OnPrimaryFire(InputAction.CallbackContext context)
    {
      if (context.performed)
      {
        primaryFireHold = true;
        combatant.PrimaryAction(true);
      }
      else if (context.canceled)
        primaryFireHold = false;
    }

    void IWeaponActions.OnSecondaryFire(InputAction.CallbackContext context)
    {
      if (context.performed)
      {
        secondaryFireHold = true;
        combatant.SecondaryAction(true);
      }
      else if (context.canceled)
        secondaryFireHold = false;
    }

    public void OnReload(InputAction.CallbackContext context)
    {
      if (!context.performed)
        return;


      combatant.equippedWeapon.Reload();
    }
  }
}
