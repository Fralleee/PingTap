using TMPro;
using UnityEngine;

namespace Fralle
{
  public class FloatingNumbers : MonoBehaviour
  {
    [SerializeField] TextMeshProUGUI numberPrefab;

    DamageController damageController;

    void Awake()
    {
      damageController = GetComponentInParent<DamageController>();
      damageController.OnDamage += HandleDamage;
    }

    void HandleDamage(DamageData damageData, float damage, bool criticalHit)
    {
      var instance = Instantiate(numberPrefab, transform);
      instance.text = Mathf.Round(damage).ToString();
      Destroy(instance, 2f);
    }

    void OnDestroy()
    {
      damageController.OnDamage += HandleDamage;
    }

    void LateUpdate()
    {
      transform.LookAt(Camera.main.transform);
      transform.Rotate(0, 180, 0);
    }

  }
}