using CombatSystem;
using CombatSystem.Addons;
using CombatSystem.Combat;
using TMPro;
using UnityEngine;

public class AmmoUI : MonoBehaviour
{
	[SerializeField] TextMeshProUGUI currentAmmoText;
	[SerializeField] TextMeshProUGUI maxAmmoText;

	void Awake()
	{
		var combatant = GetComponentInParent<Combatant>();
		combatant.OnWeaponSwitch += HandleWeaponSwitch;

		if (combatant.EquippedWeapon)
			HandleWeaponSwitch(combatant.EquippedWeapon, null);
	}


	void HandleWeaponSwitch(Weapon weapon, Weapon oldWeapon)
	{
		var ammoAddon = weapon.GetComponent<AmmoAddon>();
		ammoAddon.OnAmmoChanged += HandleAmmoChanged;

		currentAmmoText.text = ammoAddon.CurrentAmmo.ToString();
		maxAmmoText.text = ammoAddon.MaxAmmo.ToString();
	}


	void HandleAmmoChanged(int ammoCount)
	{
		currentAmmoText.text = ammoCount.ToString();
	}
}
