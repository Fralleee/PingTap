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

      if (combatant.EquippedWeapon)
        HandleWeaponSwitch(combatant.EquippedWeapon, null);
    }


    void HandleWeaponSwitch(Weapon weapon, Weapon oldWeapon)
    {
      if (weapon != null)
      {
        AmmoAddon ammoAddon = weapon.GetComponent<AmmoAddon>();
        ammoAddon.OnAmmoChanged += HandleAmmoChanged;
        currentAmmoText.text = ammoAddon.CurrentAmmo.ToString();
        maxAmmoText.text = ammoAddon.MaxAmmo.ToString();
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
