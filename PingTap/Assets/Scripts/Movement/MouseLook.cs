using Fralle.Core.Attributes;
using Fralle.UI.Menu;
using UnityEngine;

namespace Fralle.Movement
{
  public class MouseLook : MonoBehaviour
  {
    [Header("Mouse Look")] [Readonly] public float currentSensitivity;
    [SerializeField] GameObject orientation;
    public float mouseSensitivity = 50f;
    public float mouseZoomModifier = 0.4f;
    [SerializeField] float mouseLookSmooth = 0f;
    [SerializeField] float affectSmooth = 0.05f;

    [SerializeField] float clampYMax = 90f;
    [SerializeField] float clampYMin = -90f;

    [Header("Debug")] [SerializeField] Vector3 inputMouseLook;
    [SerializeField] float currentRotationX;
    [SerializeField] float currentRotationY;

    Vector2 affectRotation;


    void Awake()
    {
      ConfigureCursor(true);
      currentSensitivity = mouseSensitivity;
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

    void GatherInputs()
    {
      inputMouseLook.x += Input.GetAxis("Mouse X") * currentSensitivity * Time.fixedDeltaTime;
      inputMouseLook.y -= Input.GetAxis("Mouse Y") * currentSensitivity * Time.fixedDeltaTime;
    }

    void Look()
    {
      if (Cursor.visible) return;

      float mouseLookDampX = 0;
      float mouseLookDampY = 0;
      var affectRotationDamp = Vector2.zero;

      inputMouseLook.y = Mathf.Clamp(inputMouseLook.y, clampYMin, clampYMax);

      currentRotationX = Mathf.SmoothDamp(currentRotationX, inputMouseLook.x + affectRotation.x, ref mouseLookDampX,
        mouseLookSmooth);
      currentRotationY = Mathf.SmoothDamp(currentRotationY, inputMouseLook.y + affectRotation.y, ref mouseLookDampY,
        mouseLookSmooth);

      affectRotation = Vector2.SmoothDamp(affectRotation, Vector2.zero, ref affectRotationDamp, affectSmooth);

      var rot = orientation.transform.rotation.eulerAngles;
      orientation.transform.localRotation = Quaternion.Euler(rot.x, currentRotationX, rot.z);
      transform.localRotation = Quaternion.Euler(currentRotationY, currentRotationX, 0);
    }

    void OnApplicationFocus(bool hasFocus)
    {
      ConfigureCursor(hasFocus);
    }

    void OnDisable()
    {
      ConfigureCursor(false);
    }

    void OnEnable()
    {
      if (currentSensitivity != mouseSensitivity)
      {
        mouseSensitivity = PlayerPrefs.GetFloat(Settings.MouseSensitivity.ToString());
        currentSensitivity = mouseSensitivity * mouseZoomModifier;
      }
      else mouseSensitivity = PlayerPrefs.GetFloat(Settings.MouseSensitivity.ToString());

      ConfigureCursor();
    }
  }
}