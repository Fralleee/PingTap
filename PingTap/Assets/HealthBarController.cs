using System.Collections;
using System.Collections.Generic;
using Fralle;
using UnityEngine;

public class HealthBarController : MonoBehaviour
{
  [SerializeField] HealthBar healthBarPrefab;

  Dictionary<DamageController, HealthBar> healthBars = new Dictionary<DamageController, HealthBar>();

  void Awake()
  {
    DamageController.OnHealthBarAdded += AddHealthBar;
    DamageController.OnHealthBarRemoved += RemoveHealthBar;
  }

  void AddHealthBar(DamageController damageController)
  {
    if (healthBars.ContainsKey(damageController)) return;

    HealthBar healthBar = Instantiate(healthBarPrefab, transform);
    healthBar.Initialize(damageController);
  }

  void RemoveHealthBar(DamageController damageController)
  {
    if (!healthBars.ContainsKey(damageController)) return;

    Destroy(healthBars[damageController].gameObject);
    healthBars.Remove(damageController);
  }
}
