using System;
using System.Collections;
using UnityEngine;

namespace Fralle.Attack
{
  public class Ammo : MonoBehaviour
  {
    public event Action<int> OnAmmoChanged = delegate { };

    public int maxAmmo = 30;
    public int currentAmmo;

    [SerializeField] bool infiniteAmmo;
    [SerializeField] float reloadSpeed = 0.75f;

    bool isReloading;
    float rotationTime;
    Weapon weapon;

    void Awake()
    {
      weapon = GetComponent<Weapon>();
    }

    void Start()
    {
      currentAmmo = maxAmmo;
    }

    void Update()
    {
      if (infiniteAmmo) return;
      if (isReloading)
      {
        rotationTime = Mathf.Clamp(rotationTime + Time.deltaTime, 0, reloadSpeed);
        float spinDelta = -(Mathf.Cos(Mathf.PI * (rotationTime / reloadSpeed)) - 1f) / 2f;
        transform.localRotation = Quaternion.Euler(new Vector3(spinDelta * 360f, 0, 0));
      }
      else if (Input.GetKeyDown(KeyCode.R) && weapon.ActiveWeaponAction == WeaponStatus.Ready && currentAmmo < maxAmmo) StartCoroutine(ReloadCooldown());
    }

    public void ChangeAmmo(int change, bool apply = true)
    {
      if (apply) currentAmmo += change;
      else currentAmmo = change;
      currentAmmo = Mathf.Clamp(currentAmmo, 0, maxAmmo);
      OnAmmoChanged(currentAmmo);
    }

    public bool HasAmmo(int requiredAmmo = 1)
    {
      if (infiniteAmmo || currentAmmo >= requiredAmmo) return true;
      StartCoroutine(ReloadCooldown());
      return false;
    }

    IEnumerator ReloadCooldown()
    {
      weapon.ChangeWeaponAction(WeaponStatus.Reloading);
      isReloading = true;
      rotationTime = 0f;
      yield return new WaitForSeconds(reloadSpeed);
      ChangeAmmo(maxAmmo, false);
      weapon.ChangeWeaponAction(WeaponStatus.Ready);
      isReloading = false;
    }
  }
}
