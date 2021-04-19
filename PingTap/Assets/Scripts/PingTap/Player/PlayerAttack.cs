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

		[HideInInspector] public PlayerInput playerInput;

		int firstPersonObjectsLayer;

		bool primaryFireHold;
		bool secondaryFireHold;

		void Awake()
		{
			firstPersonObjectsLayer = LayerMask.NameToLayer("First Person Objects");

			if (combatant == null)
				combatant = GetComponent<Combatant>();

			combatant.OnWeaponSwitch += OnWeaponSwitch;

			playerInput = GetComponent<PlayerInput>();
			playerInput.actions["PrimaryFire"].performed += OnPrimaryFire;
			playerInput.actions["PrimaryFire"].canceled += OnPrimaryFireCancel;
			playerInput.actions["SecondaryFire"].performed += OnSecondaryFire;
			playerInput.actions["SecondaryFire"].canceled += OnSecondaryFireCancel;
			playerInput.actions["ItemSelect"].performed += OnItemSelect;
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
			var number = (int)context.ReadValue<float>();
			if (weapons.Length >= number + 1)
				combatant.EquipWeapon(weapons[number]);
		}

		void OnWeaponSwitch(Weapon weapon, Weapon oldWeapon)
		{
			combatant.equippedWeapon.gameObject.SetLayerRecursively(firstPersonObjectsLayer);
		}

		void Start()
		{
			if (combatant.equippedWeapon == null)
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
			playerInput.actions["PrimaryFire"].performed -= OnPrimaryFire;
			playerInput.actions["PrimaryFire"].canceled -= OnPrimaryFireCancel;
			playerInput.actions["SecondaryFire"].performed -= OnSecondaryFire;
			playerInput.actions["SecondaryFire"].canceled -= OnSecondaryFireCancel;
			playerInput.actions["ItemSelect"].performed -= OnItemSelect;
		}

		[ContextMenu("Equip Weapon")]
		public void EquipFirstWeaponInList()
		{
			combatant.EquipWeapon(weapons[0], false);
			combatant.equippedWeapon.gameObject.SetLayerRecursively(LayerMask.NameToLayer("First Person Objects"));
			Debug.Log($"Equipped: {combatant.equippedWeapon}");
		}

		[ContextMenu("Remove Weapon")]
		public void RemoveWeapon()
		{
			Debug.Log($"Removed: {combatant.equippedWeapon}");
			combatant.ClearWeapons();
		}
	}
}
