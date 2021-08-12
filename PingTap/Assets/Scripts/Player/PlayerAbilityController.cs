using Fralle.AbilitySystem;
using UnityEngine;
using UnityEngine.InputSystem;
using static PlayerControls;

public class PlayerAbilityController : AbilityController, IAbilityActions
{
	[HideInInspector] public PlayerControls controls;

	static PlayerAbilityController activeController;

	public static void Toggle(bool enabled)
	{
		if (enabled)
			activeController?.controls.Enable();
		else
			activeController?.controls.Disable();
	}

	protected override void Awake()
	{
		controls = new PlayerControls();
		controls.Ability.SetCallbacks(this);
		controls.Ability.Enable();

		activeController = this;
		//Player.controls.Ability

		base.Awake();
	}

	public void OnAttackAbility(InputAction.CallbackContext context)
	{
		if (AttackAbility != null && AttackAbility.IsReady)
			AttackAbility.Perform();
	}

	public void OnMovementAbility(InputAction.CallbackContext context)
	{
		if (MovementAbility != null && MovementAbility.IsReady)
			MovementAbility.Perform();
	}

	public void OnUltimateAbility(InputAction.CallbackContext context)
	{
		if (UltimateAbility != null && UltimateAbility.IsReady)
			UltimateAbility.Perform();
	}
}
