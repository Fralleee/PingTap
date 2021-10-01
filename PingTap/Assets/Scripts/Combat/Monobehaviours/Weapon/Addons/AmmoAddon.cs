using Sirenix.OdinInspector;
using System;
using UnityEngine;

namespace Fralle.PingTap
{
  [Serializable]
  public class AmmoAddon
  {
    public event Action<int> OnAmmoChanged = delegate { };

    [ProgressBar("Ammo", "MaxAmmo")]
    [FoldoutGroup("Ammo")] public int CurrentAmmo;
    [FoldoutGroup("Ammo")] public int MaxAmmo = 100;
    [FoldoutGroup("Ammo")] public bool InfiniteAmmo = false;

    Weapon Weapon;
    float reloadStatMultiplier = 1f;

    public float ReloadTime => Weapon.ReloadTime * reloadStatMultiplier;

    public void Setup(Weapon weapon)
    {
      Weapon = weapon;
      CurrentAmmo = MaxAmmo;
      OnAmmoChanged(CurrentAmmo);
    }

    public void ChangeAmmo(int change, bool apply = true)
    {
      CurrentAmmo = apply ? CurrentAmmo + change : change;
      CurrentAmmo = Mathf.Clamp(CurrentAmmo, 0, MaxAmmo);
      OnAmmoChanged(CurrentAmmo);
    }

    public bool HasAmmo(int requiredAmmo = 1)
    {
      if (InfiniteAmmo || CurrentAmmo >= requiredAmmo)
        return true;

      Weapon.Reload();
      return false;
    }

    public void SetMaxAmmo()
    {
      ChangeAmmo(MaxAmmo, false);
    }
  }
}
