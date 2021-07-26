using Fralle.Pingtap;
using UnityEngine;
using UnityEngine.UI;

namespace Fralle.UI.HUD
{
	public class AmmoCrosshairUI : MonoBehaviour
	{
		[SerializeField] Sprite oneSegment;
		[SerializeField] Sprite twoSegments;
		[SerializeField] Sprite threeSegments;
		[SerializeField] Sprite fourSegments;

		[SerializeField] int currentAmmo;
		[SerializeField] int maxAmmo;

		Image image;

		void Awake()
		{
			image = GetComponent<Image>();

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

				maxAmmo = ammoAddon.MaxAmmo;
				currentAmmo = ammoAddon.MaxAmmo;
			}
			else
			{
				maxAmmo = 0;
				currentAmmo = 0;
			}

			SetSpriteBasedOnAmmoCount(maxAmmo);
			image.fillAmount = 1;
		}

		void SetSpriteBasedOnAmmoCount(int ammoCount)
		{
			image.sprite = ammoCount switch
			{
				2 => twoSegments,
				3 => threeSegments,
				4 => fourSegments,
				_ => oneSegment
			};
		}

		void HandleAmmoChanged(int ammoCount)
		{
			currentAmmo = ammoCount;

			image.fillAmount = currentAmmo / (float)maxAmmo;
		}
	}
}
