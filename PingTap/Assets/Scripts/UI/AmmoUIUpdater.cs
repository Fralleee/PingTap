using TMPro;
using UnityEngine;

namespace Fralle
{
  public class AmmoUIUpdater : MonoBehaviour
  {
    [SerializeField] TextMeshProUGUI currentAmmoText;
    [SerializeField] TextMeshProUGUI maxAmmoText;

    AmmoController ammoController;
    UITweener tweener;

    void Start()
    {
      ammoController = GetComponentInParent<AmmoController>();
      tweener = GetComponentInParent<UITweener>();

      currentAmmoText.text = ammoController.currentAmmo.ToString();
      maxAmmoText.text = ammoController.maxAmmo.ToString();

      ammoController.OnAmmoChanged += UpdateCurrentAmmoText;
    }

    void OnDestroy()
    {
      ammoController.OnAmmoChanged -= UpdateCurrentAmmoText;
    }

    void UpdateCurrentAmmoText(object sender, int newAmmo)
    {
      currentAmmoText.text = newAmmo.ToString();

      tweener.HandleTween();
    }

  }
}