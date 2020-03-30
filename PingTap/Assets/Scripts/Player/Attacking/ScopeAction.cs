using Fralle;
using UnityEngine;

public class ScopeAction : PlayerAction
{
  [SerializeField] MouseButton scopeButton = MouseButton.Left;
  [SerializeField] Vector3 scopePos = new Vector3(-0.225f, 0.1f);

  [SerializeField] bool interruptScopeOnFire = true;

  [SerializeField] float resetSmooth = 11.75f;
  [SerializeField] float fovSmooth = 0.1f;

  [SerializeField] float defaultFov = 60f;
  [SerializeField] float scopedFov = 50f;

  Weapon weapon;
  Camera playerCamera;

  bool IsScoping => Input.GetMouseButton((int)scopeButton) && (interruptScopeOnFire ?
    weapon.activeWeaponAction == ActiveWeaponAction.READY :
    weapon.activeWeaponAction == ActiveWeaponAction.READY || weapon.activeWeaponAction == ActiveWeaponAction.FIRING);

  void Start()
  {
    weapon = GetComponent<Weapon>();
    playerCamera = weapon.playerCamera.GetComponent<Camera>();
  }

  void Update()
  {
    float fov = IsScoping ? scopedFov : defaultFov;
    playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, fov, fovSmooth * Time.deltaTime);

    transform.localRotation = Quaternion.identity;
    transform.localPosition = Vector3.Lerp(transform.localPosition, IsScoping ? scopePos : Vector3.zero, resetSmooth * Time.deltaTime);
  }
}
