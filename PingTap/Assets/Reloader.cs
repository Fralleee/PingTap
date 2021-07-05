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
	}

	void Update()
	{
		if (reload)
		{
			if (combatant.EquippedWeapon.AmmoAddonController.ReloadPercentage <= 0.5f)
				leftHandIk.weight = Mathf.Lerp(1, 0, combatant.EquippedWeapon.AmmoAddonController.ReloadPercentage * 2f);
			else
				leftHandIk.weight = Mathf.Lerp(0, 1, (combatant.EquippedWeapon.AmmoAddonController.ReloadPercentage - 0.5f) * 2f);
		}
	}
}
