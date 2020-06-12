using System;
using System.Collections;
using UnityEngine;

namespace Fralle.Attack.Addons
{
  public class Ammo : MonoBehaviour
  {
    public event Action<int> OnAmmoChanged = delegate { };

    public int maxAmmo = 30;
    public int currentAmmo;

    [SerializeField] bool infiniteAmmo = false;
    [SerializeField] float reloadSpeed = 0.75f;
    [SerializeField]
    AnimationCurve blendOverLifetime = new AnimationCurve(

      new Keyframe(0.0f, 0.0f, Mathf.Deg2Rad * 0.0f, Mathf.Deg2Rad * 720.0f),
      new Keyframe(0.2f, 1.0f),
      new Keyframe(1.0f, 0.0f));

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
        var agePercent = rotationTime / reloadSpeed;
        rotationTime = Mathf.Clamp(rotationTime + Time.deltaTime, 0, reloadSpeed);
        var animTime = blendOverLifetime.Evaluate(agePercent);
        var spinDelta = -(Mathf.Cos(Mathf.PI * animTime) - 1f) / 2f;
        transform.localRotation = Quaternion.Euler(new Vector3(spinDelta * 360f, 0, 0));
      }
      else if (Input.GetKeyDown(KeyCode.R) && weapon.ActiveWeaponAction == Status.Ready && currentAmmo < maxAmmo) StartCoroutine(ReloadCooldown());
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
      weapon.ChangeWeaponAction(Status.Reloading);
      isReloading = true;
      rotationTime = 0f;
      yield return new WaitForSeconds(reloadSpeed);
      ChangeAmmo(maxAmmo, false);
      weapon.ChangeWeaponAction(Status.Ready);
      isReloading = false;
    }
  }
}
