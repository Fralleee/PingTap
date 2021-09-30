using Sirenix.OdinInspector;
using System;
using System.Collections;
using UnityEngine;

namespace Fralle.PingTap
{
  public class AmmoAddon : MonoBehaviour
  {
    public event Action<int> OnAmmoChanged = delegate { };

    public int MaxAmmo = 30;
    public int CurrentAmmo;

    [SerializeField] bool infiniteAmmo = false;
    [SerializeField] float reloadSpeed = 0.75f;

    [Header("Debug")]
    [ReadOnly] public float ReloadPercentage;

    Weapon weapon;
    float reloadStatMultiplier = 1f;

    public float ReloadTime => reloadSpeed * reloadStatMultiplier;

    void Awake()
    {
      weapon = GetComponent<Weapon>();
    }

    void Start()
    {
      CurrentAmmo = MaxAmmo;
      OnAmmoChanged(CurrentAmmo);
    }

    void Update()
    {
      if (infiniteAmmo)
        return;

      if (Input.GetKeyDown(KeyCode.R) && weapon.ActiveWeaponAction == Status.Ready && CurrentAmmo < MaxAmmo)
        StartCoroutine(ReloadCooldown());
    }

    public void ChangeAmmo(int change, bool apply = true)
    {
      if (apply)
        CurrentAmmo += change;
      else
        CurrentAmmo = change;
      CurrentAmmo = Mathf.Clamp(CurrentAmmo, 0, MaxAmmo);
      OnAmmoChanged(CurrentAmmo);
    }

    public bool HasAmmo(int requiredAmmo = 1)
    {
      if (infiniteAmmo || CurrentAmmo >= requiredAmmo)
        return true;
      StartCoroutine(ReloadCooldown());
      return false;
    }

    IEnumerator ReloadCooldown()
    {
      weapon.ChangeWeaponAction(Status.Reloading);
      yield return new WaitForSeconds(ReloadTime);
      ChangeAmmo(MaxAmmo, false);
      weapon.ChangeWeaponAction(Status.Ready);
    }
  }
}
