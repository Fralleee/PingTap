using CombatSystem;
using CombatSystem.Combat;
using CombatSystem.Enums;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class Reloader : MonoBehaviour
{
	[SerializeField] ChainIKConstraint leftHandIk;

	Combatant combatant;
	bool reload;

	void Awake()
	{
		combatant = GetComponentInParent<Combatant>();
		combatant.OnWeaponSwitch += HandleWeaponSwitch;
	}

	void HandleWeaponSwitch(Weapon newWeapon, Weapon oldWeapon)
	{
		if (oldWeapon != null)
			oldWeapon.OnActiveWeaponActionChanged -= HandleWeaponActionChanged;
		newWeapon.OnActiveWeaponActionChanged += HandleWeaponActionChanged;
	}

	void HandleWeaponActionChanged(Status status)
	{
		reload = status == Status.Reloading;
		if (!reload)
			leftHandIk.weight = 1f;
	}

	void Update()
	{
		if (reload)
		{
			if (combatant.EquippedWeapon.AmmoAddonController.ReloadPercentage <= 0.25f)
				leftHandIk.weight = Mathf.Lerp(1, 0, combatant.EquippedWeapon.AmmoAddonController.ReloadPercentage * 4f);
			else if (combatant.EquippedWeapon.AmmoAddonController.ReloadPercentage - 0.25f < 0.5f)
				leftHandIk.weight = 0;
			else
				leftHandIk.weight = Mathf.Lerp(1, 0, combatant.EquippedWeapon.AmmoAddonController.ReloadPercentage * 0.25f);
		}
	}
}
