using System;
using UnityEngine;

namespace Fralle
{
  public class DamageController : MonoBehaviour, IDamageable
  {
    public event Action OnDeath = delegate { };
    public event Action<float, float, bool> OnHealthChange = delegate { };
    public event Action<float, bool> OnDamage = delegate { };

    [SerializeField] float maxHealth = 100f;
    [SerializeField] float currentHealth;
    [SerializeField] int armor;

    [SerializeField] bool immortal;

    bool isDead;

    public float damageMultiplier { get { return 1 - 0.06f * armor / (1 + 0.06f * armor); } }

    void Start()
    {
      if (currentHealth == 0) currentHealth = maxHealth;
    }

    public void TakeDamage(float rawDamage)
    {
      Debug.Log($"DamageController TakeDamage Rawdamage: {rawDamage}");
      if (isDead) return;
      float damage = rawDamage * damageMultiplier;
      currentHealth = Mathf.Clamp(currentHealth - damage, 0, maxHealth);
      OnHealthChange(currentHealth, maxHealth, true);
      OnDamage(damage, false);
      if (currentHealth <= 0) Death();
    }

    public void Death()
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
        OnDeath();
      }
    }

  }
}