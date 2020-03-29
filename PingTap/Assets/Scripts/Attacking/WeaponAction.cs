using Fralle;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Weapon))]
public abstract class WeaponAction : MonoBehaviour
{
  [Header("Shooting")]
  [SerializeField] internal MouseButton fireInput = MouseButton.Left;
  [SerializeField] internal int shotsPerSecond = 20;
  [SerializeField] internal bool tapable = false;

  internal Weapon weapon;

  bool HasAmmo => weapon.ammoController && weapon.ammoController.HasAmmo();

  internal virtual void Awake()
  {
    weapon = GetComponent<Weapon>();
  }

  internal virtual void Update()
  {
    bool shootWeapon = (tapable ? Input.GetMouseButtonDown((int)fireInput) : Input.GetMouseButton((int)fireInput)) && !weapon.performingAction;
    if (!shootWeapon) return;

    if(HasAmmo) weapon.ammoController.ChangeAmmo(-1);
    Fire();
    if (weapon.recoilController) weapon.recoilController.AddRecoil();
    if (HasAmmo) StartCoroutine(ShootingCooldown());
  }

  internal IEnumerator ShootingCooldown()
  {
    weapon.performingAction = true;
    yield return new WaitForSeconds(1f / shotsPerSecond);
    weapon.performingAction = false;
  }

  public abstract void Fire();
}
