using TMPro;
using UnityEngine;

namespace Fralle
{
  public class AmmoUIUpdater : MonoBehaviour
  {
    [SerializeField] TextMeshProUGUI currentAmmoText;
    [SerializeField] TextMeshProUGUI maxAmmoText;

    AmmoController ammoController;

    void Start()
    {
      ammoController = GetComponentInParent<AmmoController>();
      Debug.Log($"Ammo is {ammoController.currentAmmo}");

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
    }

  }
}