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

		void Awake()
		{
			firstPersonObjectsLayer = LayerMask.NameToLayer("First Person Objects");

			if (combatant == null)
				combatant = GetComponent<Combatant>();

			combatant.OnWeaponSwitch += Combatant_OnWeaponSwitch;

			playerInput = GetComponent<PlayerInput>();
		}

		void Combatant_OnWeaponSwitch(Weapon obj)
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
			SwapWeapon();
			FireInput();
		}

		void SwapWeapon()
		{
			//for (var i = 1; i <= weapons.Length; i++)
			//	if (inputController.GetKeyDown("" + i))
			//		combatant.EquipWeapon(weapons[i - 1]);
		}

		void FireInput()
		{
			//if (inputController.Mouse1ButtonDown)
			//	comba tant.PrimaryAction(true);
			//else if (inputController.Mouse1ButtonHold)
			//	combatant.PrimaryAction();
			//else if (inputController.Mouse2ButtonDown)
			//	combatant.SecondaryAction(true);
			//else if (inputController.Mouse2ButtonHold)
			//	combatant.SecondaryAction();
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
