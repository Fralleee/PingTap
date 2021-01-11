using CombatSystem;
using CombatSystem.Combat;
using Fralle.Core.Attributes;
using Fralle.Core.Extensions;
using UnityEngine;

namespace Fralle
{
	[RequireComponent(typeof(Combatant))]
	public class PlayerAttack : MonoBehaviour
	{
		[Readonly] public Weapon equippedWeapon;

		[SerializeField] Weapon[] weapons = new Weapon[0];

		Combatant combatant;

		void Awake()
		{
			combatant = GetComponent<Combatant>();
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

			Debug.Log($"Equipped weapon {weapon.weaponName}");
		}

		void SwapWeapon()
		{
			for (var i = 1; i <= weapons.Length; i++)
				if (Input.GetKeyDown("" + i))
					EquipWeapon(weapons[i - 1]);
		}

		void FireInput()
		{
			if (Input.GetMouseButtonDown(0))
				combatant.PrimaryAction(true);
			else if (Input.GetMouseButton(0))
				combatant.PrimaryAction();
			else if (Input.GetMouseButtonDown(1))
				combatant.SecondaryAction(true);
			else if (Input.GetMouseButton(1))
				combatant.SecondaryAction();
		}
	}
}
