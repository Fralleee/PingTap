using UnityEngine;
using UnityEngine.UI;

namespace CombatSystem.Combat.Damage.UI
{
  public class Healthbar : MonoBehaviour
  {
    [SerializeField] Image foregroundImage = null;

    DamageController damageController;

    void Awake()
    {
      damageController = GetComponentInParent<DamageController>();
      damageController.OnHealthChange += HandleDamageControllerChange;
    }

    void HandleDamageControllerChange(float currentHealth, float maxHealth)
    {
      var percentage = Mathf.Clamp01(currentHealth / maxHealth);
      foregroundImage.fillAmount = percentage;
    }

    void OnDestroy()
    {
      damageController.OnHealthChange -= HandleDamageControllerChange;
    }
  }
}