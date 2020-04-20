using System.Globalization;
using TMPro;
using UnityEngine;

namespace Fralle
{
  public class FloatingNumbers : MonoBehaviour
  {
    [SerializeField] TextMeshProUGUI numberPrefab;

    DamageController damageController;
    new Camera camera;
    new Renderer renderer;
    Vector3 lastPosition = Vector3.zero;

    public void Initialize(DamageController damageController)
    {
      this.damageController = damageController;
      damageController.OnDamage += HandleDamage;

      camera = Camera.main;

      renderer = damageController.gameObject.GetComponentInChildren<Renderer>();
    }

    void HandleDamage(DamageData damageData, float damage, bool criticalHit)
    {
      TextMeshProUGUI instance = Instantiate(numberPrefab, transform);
      instance.text = Mathf.Round(damage).ToString(CultureInfo.InvariantCulture);
      Destroy(instance, 2f);
    }

    void OnDestroy()
    {
      damageController.OnDamage -= HandleDamage;
    }

    void LateUpdate()
    {
      bool isVisible = renderer && renderer.isVisible;
      bool notDead = damageController != null && !damageController.isDead;
      if (isVisible && notDead) lastPosition = damageController.transform.position;

      transform.position = camera.WorldToScreenPoint(lastPosition + Vector3.up * 2f);
    }

  }
}