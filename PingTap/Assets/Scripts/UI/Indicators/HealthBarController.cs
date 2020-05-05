using System;
using Fralle;
using Fralle.Attack;
using System.Collections.Generic;
using UnityEngine;

public class HealthBarController : MonoBehaviour
{
  [SerializeField] HealthBar healthBarPrefab;

  Dictionary<Health, HealthBar> healthBars = new Dictionary<Health, HealthBar>();

  void Awake()
  {
    Health.OnHealthBarAdded += AddHealthBar;
    Health.OnHealthBarRemoved += RemoveHealthBar;
  }

  void AddHealthBar(Health health)
  {
    if (healthBars.ContainsKey(health)) return;

    HealthBar healthBar = Instantiate(healthBarPrefab, transform);
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
