using CombatSystem;
using CombatSystem.Combat;
using Fralle.FpsController;
using UnityEngine;

namespace Fralle
{
	[RequireComponent(typeof(Combatant))]
	public class PlayerAttack : MonoBehaviour
	{
		[SerializeField] Combatant combatant;
		[SerializeField] Weapon[] weapons = new Weapon[0];
		[HideInInspector] public InputController inputController;

		void Awake()
		{
			if (combatant == null)
				combatant = GetComponent<Combatant>();

			inputController = GetComponent<InputController>();
		}

		void Start()
		{
			if (combatant.equippedWeapon == null)
				combatant.EquipWeapon(weapons[0]);
		}

		void Update()
		{
			SwapWeapon();
			FireInput();
		}

		void SwapWeapon()
		{
			for (var i = 1; i <= weapons.Length; i++)
				if (inputController.GetKeyDown("" + i))
					combatant.EquipWeapon(weapons[i - 1]);
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

		[ContextMenu("Equip Weapon")]
		public void EquipFirstWeaponInList()
		{
			combatant.EquipWeapon(weapons[0], false);
		}

		[ContextMenu("Remove Weapon")]
		public void RemoveWeapon()
		{
			combatant.ClearWeapons();
		}
	}
}
