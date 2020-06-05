using Fralle.Core.Enums;
using Fralle.UI.Menu;
using UnityEngine;

namespace Fralle.Player
{
  public class PlayerInputController : MonoBehaviour
  {
    public Vector2 move;
    public Vector2 mouse;
    public Vector2 mouseRaw;

    public bool jumpButtonDown;
    public bool jumpButtonHold;

    public bool dashButtonDown;
    public bool dashButtonHold;
    public bool dashButtonUp;

    public bool crouchButtonHold;

    public bool mouse1ButtonDown;
    public bool mouse1ButtonHold;
    public bool mouse1ButtonUp;

    public bool mouse2ButtonDown;
    public bool mouse2ButtonHold;
    public bool mouse2ButtonUp;

    public float mouseSensitivity = 1f;

    void Awake()
    {
      MainMenu.OnMenuToggle += HandleMenuToggle;
    }

    void Update()
    {
      GatherInputs();
    }

    void GatherInputs()
    {
      move.x = Input.GetAxisRaw("Horizontal");
      move.y = Input.GetAxisRaw("Vertical");

      jumpButtonDown = Input.GetButtonDown("Jump");
      jumpButtonHold = Input.GetButton("Jump");

      dashButtonDown = Input.GetKeyDown(KeyCode.LeftShift);
      dashButtonHold = Input.GetKey(KeyCode.LeftShift);
      dashButtonUp = Input.GetKeyUp(KeyCode.LeftShift);

      crouchButtonHold = Input.GetKey(KeyCode.LeftControl);

      mouse.x += Input.GetAxis("Mouse X") * Time.deltaTime * mouseSensitivity;
      mouse.y -= Input.GetAxis("Mouse Y") * Time.deltaTime * mouseSensitivity;

      mouseRaw.x = Input.GetAxis("Mouse X") * mouseSensitivity;
      mouseRaw.y = Input.GetAxis("Mouse Y") * mouseSensitivity;

      mouse1ButtonDown = Input.GetMouseButtonDown(0);
      mouse1ButtonHold = Input.GetMouseButton(0);
      mouse1ButtonUp = Input.GetMouseButtonUp(0);

      mouse2ButtonDown = Input.GetMouseButtonDown(1);
      mouse2ButtonHold = Input.GetMouseButton(1);
      mouse2ButtonUp = Input.GetMouseButtonUp(1);
    }

    public bool GetMouseButton(MouseButton button, MouseButtonState state)
    {
      switch (button)
      {
        case MouseButton.Left:
          switch (state)
          {
            case MouseButtonState.Up: return mouse1ButtonUp;
            case MouseButtonState.Down: return mouse1ButtonDown;
            case MouseButtonState.Hold: return mouse1ButtonHold;
          }

          break;
        case MouseButton.Right:
          switch (state)
          {
            case MouseButtonState.Up: return mouse2ButtonUp;
            case MouseButtonState.Down: return mouse2ButtonDown;
            case MouseButtonState.Hold: return mouse2ButtonHold;
          }

          break;
      }

      return false;
    }

    void HandleMenuToggle(bool isMenuOpen)
    {
      enabled = !isMenuOpen;
      move = Vector2.zero;
      jumpButtonDown = false;
      jumpButtonHold = false;
      dashButtonDown = false;
      dashButtonHold = false;
      dashButtonUp = false;
      mouse1ButtonDown = false;
      mouse1ButtonHold = false;
      mouse1ButtonUp = false;
      mouse2ButtonDown = false;
      mouse2ButtonHold = false;
      mouse2ButtonUp = false;
    }
  }
}