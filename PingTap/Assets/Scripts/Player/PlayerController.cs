using Fralle.FpsController;
using UnityEngine;
using UnityEngine.InputSystem;
using static PlayerControls;

namespace Fralle.PingTap
{
  public class PlayerController : RigidbodyController, IMovementActions
  {
    public static void ConfigureCursor(bool doLock = true)
    {
      if (doLock)
      {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
      }
      else
      {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
      }
    }

    protected override void Awake()
    {
      ConfigureCursor();

      base.Awake();
    }

    void Start()
    {
      Player.controls.Movement.SetCallbacks(this);
      Player.controls.Movement.Enable();
    }

    public void OnMovement(InputAction.CallbackContext context)
    {
      Movement = context.ReadValue<Vector2>();

      if (Animator)
      {
        Animator.SetFloat(AnimHorizontal, Movement.x);
        Animator.SetFloat(AnimVertical, Movement.y);
      }
    }

    public void OnLook(InputAction.CallbackContext context)
    {
      MouseLook = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
      JumpButton = context.ReadValueAsButton();
    }

    public void OnCrouch(InputAction.CallbackContext context)
    {
      CrouchButton = context.ReadValueAsButton();
    }
  }
}
