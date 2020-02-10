using UnityEngine;

public class MouseLook : MonoBehaviour
{
  public float sensitivity = 15F;
  public float minimumY = -60F;
  public float maximumY = 60F;
  public bool invertY = false;

  float rotationY = 0F;
  Rigidbody rigidBody;

  void Start()
  {
    rigidBody = GetComponentInParent<Rigidbody>();

    Cursor.visible = false;
    Cursor.lockState = CursorLockMode.Locked;
  }

  void Update()
  {
    float ySens = sensitivity;
    if (invertY) { ySens *= -1f; }
    rotationY += GetMouseY() * ySens;
    rotationY = Mathf.Clamp(rotationY, minimumY, maximumY);
    transform.localEulerAngles = new Vector3(-rotationY, transform.localEulerAngles.y, 0);
  }

  private void LateUpdate()
  {
    var rotationX = GetMouseX() * sensitivity;
    Debug.Log(GetMouseX() + " : " + Input.GetAxis("Mouse X"));
    if (rigidBody)
    {
      rigidBody.rotation = Quaternion.Euler(rigidBody.rotation.eulerAngles + new Vector3(0, rotationX, 0));
    }
  }

  float GetMouseX() => Input.GetAxis("Mouse X");
  float GetMouseY() => Input.GetAxis("Mouse Y");
}