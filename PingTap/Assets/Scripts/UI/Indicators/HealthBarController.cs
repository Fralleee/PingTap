using Fralle.Attack.Offense;
using System.Collections.Generic;
using UnityEngine;

namespace Fralle.UI.Indicators
{
  public class HealthBarController : MonoBehaviour
  {
    [SerializeField] HealthBar healthBarPrefab = null;

    readonly Dictionary<Health, HealthBar> healthBars = new Dictionary<Health, HealthBar>();

    void Awake()
    {
      Health.OnHealthBarAdded += AddHealthBar;
      Health.OnHealthBarRemoved += RemoveHealthBar;
    }

    void AddHealthBar(Health health)
    {
      if (healthBars.ContainsKey(health)) return;

      var healthBar = Instantiate(healthBarPrefab, transform);
      healthBar.Initialize(health);
    }

    void RemoveHealthBar(Health health)
    {
      if (!healthBars.ContainsKey(health)) return;

      Destroy(healthBars[health].gameObject);
      healthBars.Remove(health);
    }

    void OnDestroy()
    {
      Health.OnHealthBarAdded -= AddHealthBar;
      Health.OnHealthBarRemoved -= RemoveHealthBar;
    }
  }
}
