using Fralle.AbilitySystem;
using UnityEngine.InputSystem;
using static PlayerControls;

public class PlayerAbilityController : AbilityController, IAbilityActions
{
	PlayerControls controls;

	protected override void Awake()
	{
		controls = new PlayerControls();
		controls.Ability.SetCallbacks(this);
		controls.Ability.Enable();

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
