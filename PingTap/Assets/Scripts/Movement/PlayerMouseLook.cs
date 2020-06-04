using Fralle.Player;
using Fralle.UI.Menu;
using UnityEngine;

namespace Fralle.Movement
{
  public class PlayerMouseLook : MonoBehaviour
  {
    [SerializeField] GameObject orientation;
    [SerializeField] Transform cameraRig;

    public float mouseSensitivity = 50f;
    public float mouseZoomModifier = 0.4f;

    [SerializeField] float mouseLookSmooth = 0f;
    [SerializeField] float affectSmooth = 0.05f;

    [SerializeField] float clampYMax = 90f;
    [SerializeField] float clampYMin = -90f;


    PlayerInputController input;

    Vector2 affectRotation;
    float currentRotationX;
    float currentRotationY;

    void Awake()
    {
      input = GetComponent<PlayerInputController>();

      ConfigureCursor(true);
      SetMouseSensitivity();

      MainMenu.OnMenuToggle += HandleMenuToggle;
    }

    void LateUpdate()
    {
      Look();
    }

    public void AddRotation(Vector2 rotation)
    {
      affectRotation = rotation;
    }

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
        Cursor.lockState = CursorLockMode.None;
      }
    }

    void Look()
    {
      if (Cursor.visible) return;

      float mouseLookDampX = 0;
      float mouseLookDampY = 0;
      var affectRotationDamp = Vector2.zero;

      input.mouse.y = Mathf.Clamp(input.mouse.y, clampYMin, clampYMax);

      currentRotationX = Mathf.SmoothDamp(currentRotationX, input.mouse.x + affectRotation.x * Time.deltaTime, ref mouseLookDampX, mouseLookSmooth);
      currentRotationY = Mathf.SmoothDamp(currentRotationY, input.mouse.y + affectRotation.y * Time.deltaTime, ref mouseLookDampY, mouseLookSmooth);

      affectRotation = Vector2.SmoothDamp(affectRotation, Vector2.zero, ref affectRotationDamp, affectSmooth);

      var rot = orientation.transform.rotation.eulerAngles;
      orientation.transform.localRotation = Quaternion.Euler(rot.x, currentRotationX, rot.z);
      cameraRig.localRotation = Quaternion.Euler(currentRotationY, currentRotationX, 0);
    }

    void SetMouseSensitivity()
    {
      mouseSensitivity = PlayerPrefs.GetFloat(Settings.MouseSensitivity.ToString());
      mouseSensitivity = Mathf.Clamp(mouseSensitivity, 1, mouseSensitivity);
      input.mouseSensitivity = mouseSensitivity;
    }

    void OnEnable()
    {
      SetMouseSensitivity();
    }

    void HandleMenuToggle(bool isMenuOpen)
    {
      enabled = !isMenuOpen;
      ConfigureCursor(!isMenuOpen);
    }
  }
}