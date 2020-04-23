using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;
using Vector3 = System.Numerics.Vector3;

namespace Fralle
{
  public class DamageController : MonoBehaviour, IDamageable
  {
    public static event Action<DamageController> OnHealthBarAdded = delegate { };
    public static event Action<DamageController> OnHealthBarRemoved = delegate { };
    public static event Action<DamageData> OnAnyDamage = delegate { };

    public event Action<DamageController, DamageData> OnDeath = delegate { };
    public event Action<float, float> OnHealthChange = delegate { };

    [Header("HealthBar")]
    public float yLowestOffset = 2f;
    public float yHighestOffset = 3.5f;

    [Header("Stats")]
    public float maxHealth = 100f;
    public float currentHealth;
    [SerializeField] int armor;
    [SerializeField] bool immortal;
    public bool isDead;
    
    bool isTouched;
    public float damageMultiplier => 1 - 0.06f * armor / (1 + 0.06f * armor);

    public List<DamageEffect> damageEffects = new List<DamageEffect>();

    void Start()
    {
      if (currentHealth == 0) currentHealth = maxHealth;
    }

    void Update()
    {
      foreach (DamageEffect damageEffect in damageEffects)
         damageEffect.Tick(this);

      damageEffects.RemoveAll(x => x.timer > x.time);
    }

    public void TakeDamage(DamageData damageData)
    {
      if (!isTouched)
      {
        OnHealthBarAdded(this);
        isTouched = true;
      }
      if (isDead) return;

      float damage = damageData.damage * damageMultiplier;
      currentHealth = Mathf.Clamp(currentHealth - damage, 0, maxHealth);
      OnHealthChange(currentHealth, maxHealth);
      OnAnyDamage(damageData);

      if (currentHealth <= 0) Death(damageData);
    }

    public void ApplyEffect(DamageEffect effect)
    {
      DamageEffect oldEffect = damageEffects.FirstOrDefault(x => x.name == effect.name);
      damageEffects.Upsert(oldEffect, effect.Append(oldEffect));
    }

    public void Death(DamageData damageData)
    {
      if (isDead) return;
      OnHealthBarRemoved(this);
      if (immortal)
      {
        currentHealth = maxHealth;
        OnHealthChange(currentHealth, maxHealth);
      }
      else
      {
        isDead = true;
        OnDeath(this, damageData);
      }
    }

  }
}