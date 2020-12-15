using CombatSystem;
using CombatSystem.Addons;
using CombatSystem.Combat;
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

		[SerializeField] int currentAmmo = 0;
		[SerializeField] int maxAmmo = 0;

		Image image;

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

			SetSpriteBasedOnAmmoCount(maxAmmo);
			image.fillAmount = 1;
		}

		void SetSpriteBasedOnAmmoCount(int maxAmmo)
		{
			if (maxAmmo == 2)
			{
				image.sprite = twoSegments;
			}
			else if (maxAmmo == 3)
			{
				image.sprite = threeSegments;
			}
			else if (maxAmmo == 4)
			{
				image.sprite = fourSegments;
			}
			else
			{
				image.sprite = oneSegment;
			}
		}

		void HandleAmmoChanged(int ammoCount)
		{
			currentAmmo = ammoCount;

			image.fillAmount = currentAmmo / (float)maxAmmo;
		}
	}
}
