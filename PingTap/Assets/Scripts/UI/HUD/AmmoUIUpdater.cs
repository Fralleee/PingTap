using Fralle.Attack.Addons;
using TMPro;
using UnityEngine;

namespace Fralle.UI.HUD
{
  public class AmmoUiUpdater : MonoBehaviour
  {
    [SerializeField] TextMeshProUGUI currentAmmoText = null;
    [SerializeField] TextMeshProUGUI maxAmmoText = null;

    Ammo ammo;
    UiTweener uiTweener;

    void Start()
    {
      ammo = GetComponentInParent<Ammo>();
      uiTweener = GetComponentInParent<UiTweener>();

      currentAmmoText.text = ammo.currentAmmo.ToString();
      maxAmmoText.text = ammo.maxAmmo.ToString();

      ammo.OnAmmoChanged += UpdateCurrentAmmoText;
    }

    void OnDestroy()
    {
      ammo.OnAmmoChanged -= UpdateCurrentAmmoText;
    }

    void UpdateCurrentAmmoText(int newAmmo)
    {
      currentAmmoText.text = newAmmo.ToString();

      uiTweener.HandleTween();
    }
  }
}