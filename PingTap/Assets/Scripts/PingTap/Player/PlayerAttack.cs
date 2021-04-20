using CombatSystem;
using CombatSystem.Combat;
using Fralle.Core.Extensions;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Fralle
{
	[RequireComponent(typeof(Combatant))]
	public class PlayerAttack : MonoBehaviour
	{
		[SerializeField] Combatant combatant;
		[SerializeField] Weapon[] weapons = new Weapon[0];

		[HideInInspector] public PlayerInput PlayerInput;

		int firstPersonObjectsLayer;

		bool primaryFireHold;
		bool secondaryFireHold;

		void Awake()
		{
			firstPersonObjectsLayer = LayerMask.NameToLayer("First Person Objects");

			if (combatant == null)
				combatant = GetComponent<Combatant>();

			combatant.OnWeaponSwitch += OnWeaponSwitch;

			PlayerInput = GetComponent<PlayerInput>();
			PlayerInput.actions["PrimaryFire"].performed += OnPrimaryFire;
			PlayerInput.actions["PrimaryFire"].canceled += OnPrimaryFireCancel;
			PlayerInput.actions["SecondaryFire"].performed += OnSecondaryFire;
			PlayerInput.actions["SecondaryFire"].canceled += OnSecondaryFireCancel;
			PlayerInput.actions["ItemSelect"].performed += OnItemSelect;
		}

		void OnPrimaryFire(InputAction.CallbackContext context)
		{
			primaryFireHold = true;
			if (context.duration <= 0)
				combatant.PrimaryAction(true);
		}
		void OnPrimaryFireCancel(InputAction.CallbackContext context)
		{
			primaryFireHold = false;
		}

		void OnSecondaryFire(InputAction.CallbackContext context)
		{
			secondaryFireHold = true;
			if (context.duration <= 0)
				combatant.SecondaryAction(true);
		}
		void OnSecondaryFireCancel(InputAction.CallbackContext context)
		{
			secondaryFireHold = false;
		}

		void OnItemSelect(InputAction.CallbackContext context)
		{
			int number = (int)context.ReadValue<float>();
			if (weapons.Length >= number + 1)
				combatant.EquipWeapon(weapons[number]);
		}

		void OnWeaponSwitch(Weapon weapon, Weapon oldWeapon)
		{
			combatant.EquippedWeapon.gameObject.SetLayerRecursively(firstPersonObjectsLayer);
		}

		void Start()
		{
			if (combatant.EquippedWeapon == null)
				combatant.EquipWeapon(weapons[0]);
		}

		void Update()
		{
			if (primaryFireHold)
			{
				combatant.PrimaryAction();
			}
			else if (secondaryFireHold)
				combatant.SecondaryAction();
		}

		void OnDestroy()
		{
			PlayerInput.actions["PrimaryFire"].performed -= OnPrimaryFire;
			PlayerInput.actions["PrimaryFire"].canceled -= OnPrimaryFireCancel;
			PlayerInput.actions["SecondaryFire"].performed -= OnSecondaryFire;
			PlayerInput.actions["SecondaryFire"].canceled -= OnSecondaryFireCancel;
			PlayerInput.actions["ItemSelect"].performed -= OnItemSelect;
		}

		[ContextMenu("Equip Weapon")]
		public void EquipFirstWeaponInList()
		{
			combatant.EquipWeapon(weapons[0], false);
			combatant.EquippedWeapon.gameObject.SetLayerRecursively(LayerMask.NameToLayer("First Person Objects"));
			Debug.Log($"Equipped: {combatant.EquippedWeapon}");
		}

		[ContextMenu("Remove Weapon")]
		public void RemoveWeapon()
		{
			Debug.Log($"Removed: {combatant.EquippedWeapon}");
			combatant.ClearWeapons();
		}
	}
}
