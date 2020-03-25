using UnityEngine;

public class MouseLook : MonoBehaviour
{
  [Header("Mouse Look")]
  [SerializeField] GameObject orientation;
  [SerializeField] float mouseSensitivity = 50f;
  [SerializeField] float mouseLookSmooth = 0f;
  [SerializeField] float affectSmooth = 0.05f;

  [SerializeField] float clampYMax = 90f;
  [SerializeField] float clampYMin = -90f;

  [Header("Debug")]
  [SerializeField] Vector3 inputMouseLook;
  [SerializeField] float currentRotationX;
  [SerializeField] float currentRotationY;

  Vector2 affectRotation;

  void Awake()
  {
    ConfigureCursor(true);
  }

  void Update()
  {
    GatherInputs();
  }

  void LateUpdate()
  {
    Look();
  }

  public void AddRotation(Vector2 rotation)
  {
    affectRotation = rotation;
  }

  void ConfigureCursor(bool doLock = true)
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

  void GatherInputs()
  {
    inputMouseLook.x += Input.GetAxis("Mouse X") * mouseSensitivity * Time.fixedDeltaTime;
    inputMouseLook.y -= Input.GetAxis("Mouse Y") * mouseSensitivity * Time.fixedDeltaTime;
  }

  void Look()
  {
    if (Cursor.visible) return;

    float mouseLookDampX = 0;
    float mouseLookDampY = 0;
    Vector2 affectRotationDamp = Vector2.zero;

    inputMouseLook.y = Mathf.Clamp(inputMouseLook.y, clampYMin, clampYMax);

    currentRotationX = Mathf.SmoothDamp(currentRotationX, inputMouseLook.x + affectRotation.x, ref mouseLookDampX, mouseLookSmooth);
    currentRotationY = Mathf.SmoothDamp(currentRotationY, inputMouseLook.y + affectRotation.y, ref mouseLookDampY, mouseLookSmooth);

    affectRotation = Vector2.SmoothDamp(affectRotation, Vector2.zero, ref affectRotationDamp, affectSmooth);

    Vector3 rot = orientation.transform.rotation.eulerAngles;
    orientation.transform.localRotation = Quaternion.Euler(rot.x, currentRotationX, rot.z);
    transform.localRotation = Quaternion.Euler(currentRotationY, currentRotationX, 0);
  }

  void OnApplicationFocus(bool hasFocus)
  {
    ConfigureCursor(hasFocus);
  }

}
