using CombatSystem;
using CombatSystem.Addons;
using CombatSystem.Combat;
using UnityEngine;
using UnityEngine.UI;

namespace Fralle.UI.HUD
{
	public class AmmoCrosshairUI : MonoBehaviour
	{
		[SerializeField] Image image;

		[SerializeField] int currentAmmo = 0;
		[SerializeField] int maxAmmo = 0;

		void Awake()
		{
			image = GetComponent<Image>();
			var combatant = GetComponentInParent<Combatant>();
			combatant.OnWeaponSwitch += HandleWeaponSwitch;

			if (combatant.weapon)
				HandleWeaponSwitch(combatant.weapon);
		}

		void HandleWeaponSwitch(Weapon weapon)
		{
			var ammoAddon = weapon.GetComponent<AmmoAddon>();
			ammoAddon.OnAmmoChanged += HandleAmmoChanged;

			maxAmmo = ammoAddon.maxAmmo;
			currentAmmo = ammoAddon.maxAmmo;

			image.fillAmount = 1;
		}

		void HandleAmmoChanged(int ammoCount)
		{
			currentAmmo = ammoCount;

			image.fillAmount = currentAmmo / (float)maxAmmo;
		}
	}
}
