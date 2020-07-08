using UnityEngine;

namespace Fralle.FpsController
{
  public class CameraController : MonoBehaviour
  {
    [SerializeField] GameObject orientation = null;
    [SerializeField] Transform cameraRig = null;
    [SerializeField] float smoothTime = 0.05f;
    [SerializeField] float clampYMax = 90f;
    [SerializeField] float clampYMin = -90f;

    InputController input;

    Vector2 affectRotation = Vector2.zero;

    float currentRotationX;
    float currentRotationY;
    float mouseLookDampX = 0;
    float mouseLookDampY = 0;

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

    void Awake()
    {
      input = GetComponent<InputController>();
      ConfigureCursor(true);
    }

    void LateUpdate()
    {
      Look();
    }

    void Look()
    {
      var mouseY = Mathf.Clamp(input.Mouse.y, clampYMin, clampYMax);

      currentRotationX = Mathf.SmoothDamp(currentRotationX, input.Mouse.x + affectRotation.x * Time.deltaTime, ref mouseLookDampX, smoothTime);
      currentRotationY = Mathf.SmoothDamp(currentRotationY, mouseY + affectRotation.y * Time.deltaTime, ref mouseLookDampY, smoothTime);

      affectRotation = Vector2.SmoothDamp(affectRotation, Vector2.zero, ref affectRotation, 0);

      var rot = orientation.transform.rotation.eulerAngles;
      orientation.transform.localRotation = Quaternion.Euler(rot.x, currentRotationX, rot.z);
      cameraRig.localRotation = Quaternion.Euler(currentRotationY, currentRotationX, 0);
    }
  }
}