using CombatSystem.Addons;
using TMPro;
using UnityEngine;

namespace Fralle.UI.HUD
{
  public class AmmoUiUpdater : MonoBehaviour
  {
    [SerializeField] TextMeshProUGUI currentAmmoText = null;
    [SerializeField] TextMeshProUGUI maxAmmoText = null;

    AmmoAddon ammoAddon;
    UiTweener uiTweener;

    void Start()
    {
      ammoAddon = GetComponentInParent<AmmoAddon>();
      uiTweener = GetComponentInParent<UiTweener>();

      currentAmmoText.text = ammoAddon.currentAmmo.ToString();
      maxAmmoText.text = ammoAddon.maxAmmo.ToString();

      ammoAddon.OnAmmoChanged += UpdateCurrentAmmoAddonText;
    }

    void OnDestroy()
    {
      ammoAddon.OnAmmoChanged -= UpdateCurrentAmmoAddonText;
    }

    void UpdateCurrentAmmoAddonText(int newAmmo)
    {
      currentAmmoText.text = newAmmo.ToString();

      uiTweener.HandleTween();
    }
  }
}