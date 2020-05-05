using Fralle.Attack;
using TMPro;
using UnityEngine;

namespace Fralle
{
  public class AmmoUIUpdater : MonoBehaviour
  {
    [SerializeField] TextMeshProUGUI currentAmmoText;
    [SerializeField] TextMeshProUGUI maxAmmoText;

    Ammo ammo;
    UITweener tweener;

    void Start()
    {
      ammo = GetComponentInParent<Ammo>();
      tweener = GetComponentInParent<UITweener>();

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