using System;
using JetBrains.Annotations;
using UnityEngine;

namespace Fralle
{
  public class DamageController : MonoBehaviour, IDamageable
  {
    public event Action<DamageController, DamageData> OnDeath = delegate { };
    public event Action<float, float, bool> OnHealthChange = delegate { };
    public event Action<DamageData, float, bool> OnDamage = delegate { };

    [SerializeField] float maxHealth = 100f;
    [SerializeField] float currentHealth;
    [SerializeField] int armor;

    [SerializeField] bool immortal;

    bool isDead;

    public float damageMultiplier => 1 - 0.06f * armor / (1 + 0.06f * armor);

    void Start()
    {
      if (currentHealth == 0) currentHealth = maxHealth;
    }

    public void TakeDamage(DamageData damageData)
    {
      if (isDead) return;
      float damage = damageData.damage * damageMultiplier;
      currentHealth = Mathf.Clamp(currentHealth - damage, 0, maxHealth);
      OnHealthChange(currentHealth, maxHealth, true);
      OnDamage(damageData, damage, false);
      if (currentHealth <= 0) Death(damageData);
    }

    public void Death(DamageData damageData)
    {
      if (isDead) return;
      if (immortal)
      {
        currentHealth = maxHealth;
        OnHealthChange(currentHealth, maxHealth, true);
      }
      else
      {
        isDead = true;
        OnDeath(this, damageData);
      }
    }

  }
}