using UnityEngine;

public class WeaponManager : MonoBehaviour
{
  [SerializeField] Weapon primaryWeapon;

  [SerializeField] float swaySize = 0.004f;
  [SerializeField] float swaySmooth = 25f;

  [SerializeField] float defaultFov = 60f;
  [SerializeField] float scopedFov = 50f;
  [SerializeField] float fovSmooth = 0.1f;

  [SerializeField] new Rigidbody rigidbody;
  [SerializeField] Transform weaponHolder;
  [SerializeField] Transform playerCamera;
  [SerializeField] Transform swayHolder;
  [SerializeField] Camera[] playerCams;

  bool hasPrimaryWeapon;

  float oldRBVelocityY;
  float bounceBackVelocityY;
  float bounceBackThreshold = 0.1f;

  void Start()
  {
    primaryWeapon = Instantiate(primaryWeapon);
    hasPrimaryWeapon = primaryWeapon != null;
    primaryWeapon?.Equip(weaponHolder, playerCamera);
  }

  void Update()
  {
    if (!hasPrimaryWeapon) return;

    foreach (var cam in playerCams)
    {
      var fov = primaryWeapon.Scoping ? scopedFov : defaultFov;
      cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, fov, fovSmooth * Time.deltaTime);
    }

    float velocityY = rigidbody.velocity.y;
    if (oldRBVelocityY != 0 && velocityY == 0) bounceBackVelocityY = 5f;
    if (bounceBackVelocityY > bounceBackThreshold && velocityY == 0)
    {
      velocityY = bounceBackVelocityY;
      Mathf.SmoothDamp(bounceBackVelocityY, 0, ref bounceBackVelocityY, 1f);
    }

    oldRBVelocityY = rigidbody.velocity.y;
    Vector2 delta = -new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y") + velocityY);

    if (!Cursor.visible)
    {
      swayHolder.localPosition = Vector3.Lerp(swayHolder.localPosition, Vector3.zero, swaySmooth * Time.deltaTime);
      swayHolder.localPosition += (Vector3)delta * swaySize;
    }
  }
}
