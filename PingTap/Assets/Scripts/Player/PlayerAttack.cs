using Fralle.Core;
using Fralle.PingTap;
using UnityEngine;
using UnityEngine.InputSystem;
using static PlayerControls;

namespace Fralle
{
  [RequireComponent(typeof(Combatant))]
  public class PlayerAttack : MonoBehaviour, IWeaponActions
  {
    [SerializeField] Combatant combatant;
    [SerializeField] Weapon[] weapons = new Weapon[0];
    [SerializeField] Transform weaponCamera;

    int firstPersonObjectsLayer;

    bool primaryFireHold;
    bool secondaryFireHold;

    Vector3 defaultWeaponCameraPosition;
    Quaternion defaultWeaponCameraRotation;

    void Awake()
    {
      firstPersonObjectsLayer = LayerMask.NameToLayer("FPO");

      if (combatant == null)
        combatant = GetComponent<Combatant>();

      combatant.OnWeaponSwitch += OnWeaponSwitch;

      defaultWeaponCameraPosition = weaponCamera.transform.localPosition;
      defaultWeaponCameraRotation = weaponCamera.transform.localRotation;
    }

    void Start()
    {
      Player.controls.Weapon.SetCallbacks(this);
      Player.controls.Weapon.Enable();

      if (combatant.EquippedWeapon == null)
        combatant.EquipWeapon(weapons[0]);
    }

    void Update()
    {
      if (primaryFireHold)
      {
        combatant.PrimaryAction();
      }
      else if (secondaryFireHold)
        combatant.SecondaryAction();
    }

    [ContextMenu("Equip Weapon")]
    public void EquipFirstWeaponInList()
    {
      combatant.EquipWeapon(weapons[0], false);
      combatant.EquippedWeapon.gameObject.SetLayerRecursively(firstPersonObjectsLayer);
      Debug.Log($"Equipped: {combatant.EquippedWeapon}");
    }

    [ContextMenu("Remove Weapon")]
    public void RemoveWeapon()
    {
      Debug.Log($"Removed: {combatant.EquippedWeapon}");
      combatant.ClearWeapons();
    }

    void OnWeaponSwitch(Weapon weapon, Weapon oldWeapon)
    {
      if (combatant.EquippedWeapon == null)
      {
        weaponCamera.localPosition = defaultWeaponCameraPosition;
        weaponCamera.localRotation = defaultWeaponCameraRotation;
        return;
      }

      combatant.EquippedWeapon.gameObject.SetLayerRecursively(firstPersonObjectsLayer);
      weaponCamera.localPosition = combatant.EquippedWeapon.weaponCameraTransform.localPosition;
      weaponCamera.localRotation = combatant.EquippedWeapon.weaponCameraTransform.localRotation;
    }

    void IWeaponActions.OnItemSelect(InputAction.CallbackContext context)
    {
      if (context.performed)
      {
        int number = (int)context.ReadValue<float>();
        if (weapons.Length >= number + 1)
          combatant.EquipWeapon(weapons[number]);
        else
          combatant.EquipWeapon(null);
      }
    }

    void IWeaponActions.OnPrimaryFire(InputAction.CallbackContext context)
    {
      if (context.performed)
      {
        primaryFireHold = true;
        if (context.duration <= 0)
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
        if (context.duration <= 0)
          combatant.SecondaryAction(true);
      }
      else if (context.canceled)
        secondaryFireHold = false;
    }
  }
}
