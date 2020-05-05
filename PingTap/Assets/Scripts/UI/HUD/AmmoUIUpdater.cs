using Fralle.Attack.Addons;
using Fralle.Core.Animation;
using TMPro;
using UnityEngine;

namespace Fralle.UI.HUD
{
  public class AmmoUiUpdater : MonoBehaviour
  {
    [SerializeField] TextMeshProUGUI currentAmmoText;
    [SerializeField] TextMeshProUGUI maxAmmoText;

    Ammo ammo;
    UiTweener tweener;

    void Start()
    {
      ammo = GetComponentInParent<Ammo>();
      tweener = GetComponentInParent<UiTweener>();

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

      tweener.HandleTween();
    }
  }
}