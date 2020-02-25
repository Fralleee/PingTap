using UnityEngine;

public class MouseLook : MonoBehaviour
{
  [Header("Mouse Look")]
  [SerializeField] GameObject orientation;
  [SerializeField] float mouseSensitivity = 50f;
  [SerializeField] float mouseLookSmooth = 0f;

  [SerializeField] float clampYMax = 90f;
  [SerializeField] float clampYMin = -90f;

  [Header("Debug")]
  [SerializeField] Vector3 inputMouseLook;
  [SerializeField] float currentRotationX;
  [SerializeField] float currentRotationY;

  float mouseLookDampX;
  float mouseLookDampY;

  void Awake()
  {
    Cursor.visible = false;
    Cursor.lockState = CursorLockMode.Locked;
  }

  void Update()
  {
    GatherInputs();
  }

  void LateUpdate()
  {
    Look();
  }

  void GatherInputs()
  {
    inputMouseLook.x += Input.GetAxis("Mouse X") * mouseSensitivity * Time.fixedDeltaTime;
    inputMouseLook.y -= Input.GetAxis("Mouse Y") * mouseSensitivity * Time.fixedDeltaTime;
  }

  void Look()
  {
    inputMouseLook.y = Mathf.Clamp(inputMouseLook.y, clampYMin, clampYMax);

    currentRotationX = Mathf.SmoothDamp(currentRotationX, inputMouseLook.x, ref mouseLookDampX, mouseLookSmooth);
    currentRotationY = Mathf.SmoothDamp(currentRotationY, inputMouseLook.y, ref mouseLookDampY, mouseLookSmooth);

    Vector3 rot = orientation.transform.rotation.eulerAngles;
    orientation.transform.localRotation = Quaternion.Euler(rot.x, currentRotationX, rot.z);
    transform.localRotation = Quaternion.Euler(currentRotationY, currentRotationX, 0);
  }


}
