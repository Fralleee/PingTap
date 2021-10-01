using Fralle.PingTap;
using TMPro;
using UnityEngine;

namespace Fralle.UI.HUD
{
  public class AmmoUI : MonoBehaviour
  {
    [SerializeField] TextMeshProUGUI currentAmmoText;
    [SerializeField] TextMeshProUGUI maxAmmoText;

    void Awake()
    {
      Combatant combatant = GetComponentInParent<Combatant>();
      combatant.OnWeaponSwitch += HandleWeaponSwitch;

      if (combatant.equippedWeapon)
        HandleWeaponSwitch(combatant.equippedWeapon, null);
    }


    void HandleWeaponSwitch(Weapon weapon, Weapon oldWeapon)
    {
      if (weapon != null)
      {
        weapon.Ammo.OnAmmoChanged += HandleAmmoChanged;
        currentAmmoText.text = weapon.Ammo.CurrentAmmo.ToString();
        maxAmmoText.text = weapon.Ammo.MaxAmmo.ToString();
      }
      else
      {
        currentAmmoText.text = "0";
        maxAmmoText.text = "0";
      }
    }


    void HandleAmmoChanged(int ammoCount)
    {
      currentAmmoText.text = ammoCount.ToString();
    }
  }
}
