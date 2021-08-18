using Fralle;
using Fralle.AbilitySystem;
using UnityEngine.InputSystem;
using static PlayerControls;

public class PlayerAbilityController : AbilityController, IAbilityActions
{
  void Start()
  {
    Player.controls.Ability.SetCallbacks(this);
    Player.controls.Ability.Enable();
  }

  public void OnAttackAbility(InputAction.CallbackContext context)
  {
    if (context.performed && AttackAbility != null && AttackAbility.IsReady)
    {
      AttackAbility.Perform();
    }
  }

  public void OnMovementAbility(InputAction.CallbackContext context)
  {
    if (context.performed && MovementAbility != null && MovementAbility.IsReady)
    {
      MovementAbility.Perform();
    }
  }

  public void OnUltimateAbility(InputAction.CallbackContext context)
  {
    if (context.performed && UltimateAbility != null && UltimateAbility.IsReady)
    {
      UltimateAbility.Perform();
    }
  }
}
