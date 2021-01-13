using CombatSystem;
using CombatSystem.Combat;
using Fralle.Core.Attributes;
using Fralle.Core.Extensions;
using Fralle.FpsController;
using UnityEngine;

namespace Fralle
{
	[RequireComponent(typeof(Combatant))]
	public class PlayerAttack : MonoBehaviour
	{
		[Readonly] public Weapon equippedWeapon;

		[SerializeField] Weapon[] weapons = new Weapon[0];

		Combatant combatant;
		[HideInInspector] public InputController inputController;

		void Awake()
		{
			combatant = GetComponent<Combatant>();
			inputController = GetComponent<InputController>();
		}

		void Start()
		{
			if (weapons.Length > 0)
				EquipWeapon(weapons[0]);
		}

		void Update()
		{
			SwapWeapon();
			FireInput();
		}

		void EquipWeapon(Weapon weapon)
		{
			if (equippedWeapon && equippedWeapon.weaponName == weapon.weaponName)
				return;
			if (equippedWeapon)
				Destroy(equippedWeapon.gameObject);

			equippedWeapon = Instantiate(weapon, combatant.weaponHolder.position.With(y: -0.5f), combatant.weaponHolder.rotation, combatant.weaponHolder);
			equippedWeapon.Equip(combatant);
		}

		void SwapWeapon()
		{
			for (var i = 1; i <= weapons.Length; i++)
				if (inputController.GetKeyDown("" + i))
					EquipWeapon(weapons[i - 1]);
		}

		void FireInput()
		{
			if (inputController.Mouse1ButtonDown)
				combatant.PrimaryAction(true);
			else if (inputController.Mouse1ButtonHold)
				combatant.PrimaryAction();
			else if (inputController.Mouse2ButtonDown)
				combatant.SecondaryAction(true);
			else if (inputController.Mouse2ButtonHold)
				combatant.SecondaryAction();
		}
	}
}
