using CombatSystem.Combat.Damage;
using System.Collections.Generic;
using UnityEngine;

namespace Fralle.UI.Indicators
{
  public class HealthBarController : MonoBehaviour
  {
    [SerializeField] HealthBar healthBarPrefab = null;

    readonly Dictionary<DamageController, HealthBar> healthBars = new Dictionary<DamageController, HealthBar>();

    void Awake()
    {
      DamageController.OnHealthBarAdded += AddHealthBar;
      DamageController.OnHealthBarRemoved += RemoveHealthBar;
    }

    void AddHealthBar(DamageController damageController)
    {
      if (healthBars.ContainsKey(damageController)) return;

      var healthBar = Instantiate(healthBarPrefab, transform);
      healthBar.Initialize(damageController);
    }

    void RemoveHealthBar(DamageController damageController)
    {
      if (!healthBars.ContainsKey(damageController)) return;

      Destroy(healthBars[damageController].gameObject);
      healthBars.Remove(damageController);
    }

    void OnDestroy()
    {
      DamageController.OnHealthBarAdded -= AddHealthBar;
      DamageController.OnHealthBarRemoved -= RemoveHealthBar;
    }
  }
}
