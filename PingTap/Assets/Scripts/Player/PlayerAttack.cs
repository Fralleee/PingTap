using CombatSystem;
using CombatSystem.Combat;
using Fralle.FpsController;
using UnityEngine;

namespace Fralle
{
	[RequireComponent(typeof(Combatant))]
	public class PlayerAttack : MonoBehaviour
	{
		public Weapon equippedWeapon;
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

#if UNITY_EDITOR
			equippedWeapon = Instantiate(weapon, combatant.weaponHolder.position, combatant.weaponHolder.rotation, combatant.weaponHolder);
#else
			equippedWeapon = Instantiate(weapon, combatant.weaponHolder.position.With(y: -0.5f), combatant.weaponHolder.rotation, combatant.weaponHolder);
#endif
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

		void ClearWeapons()
		{
			if (combatant == null || weapons.Length <= 0)
			{
				Debug.LogWarning("Could not equip weapon. Check weapons array and combatant reference.");
				return;
			}

			foreach (Transform child in combatant.weaponHolder)
				DestroyImmediate(child.gameObject);
		}

		[ContextMenu("Equip Weapon")]
		public void EquipFirstWeaponInList()
		{
			ClearWeapons();
			EquipWeapon(weapons[0]);
		}

		[ContextMenu("Remove Weapon")]
		public void RemoveWeapon()
		{
			ClearWeapons();
			equippedWeapon = null;
		}
	}
}
