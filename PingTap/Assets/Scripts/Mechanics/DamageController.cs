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
    [SerializeField] bool immortal;

    [HideInInspector] public bool isDead;
    [HideInInspector] public List<DamageEffect> damageEffects = new List<DamageEffect>();

    bool isTouched;
    Armor armor;

    void Start()
    {
      armor = GetComponent<Armor>();

      if (currentHealth == 0) currentHealth = maxHealth;
    }

    void Update()
    {
      foreach (DamageEffect damageEffect in damageEffects)
        damageEffect.Tick(this);

      foreach (DamageEffect damageEffect in damageEffects.Where(x => x.timer > x.time))
        damageEffect.Exit(this);

      damageEffects.RemoveAll(x => x.timer > x.time);
    }

    public void TakeDamage(DamageData damageData)
    {
      if (damageData.damage <= 0)
      {
        FalseHit(damageData);
        return;
      }
      if (!isTouched)
      {
        OnHealthBarAdded(this);
        isTouched = true;
      }
      if (isDead) return;

      currentHealth = Mathf.Clamp(currentHealth - damageData.damage, 0, maxHealth);
      OnHealthChange(currentHealth, maxHealth);
      OnAnyDamage(damageData);

      if (currentHealth <= 0) Death(damageData);
    }

    void ApplyEffects(DamageData damageData, EffectProtection effectProtection = EffectProtection.BlockNone)
    {
      foreach (DamageEffect effect in damageData.effects)
      {
        DamageEffect newEffect = effect.Setup(damageData);
        DamageEffect oldEffect = damageEffects.FirstOrDefault(x => x.name == effect.name);
        newEffect = newEffect.Append(oldEffect);
        newEffect = armor.CalculateEffect(newEffect, effectProtection);
        newEffect.Enter(this);
        damageEffects.Upsert(oldEffect, newEffect);
      }
    }

    void FalseHit(DamageData damageData)
    {
      OnAnyDamage(damageData);
    }

    public void Hit(DamageData damageData)
    {
      if (armor)
      {
        ProtectionResult result = armor.RunProtection(damageData, this);
        result.damageData.damage = armor.CalculateDamage(result.damageData, this);
        TakeDamage(result.damageData.WithHitboxModifier());
        if (result.effectProtection != EffectProtection.BlockAll)
        {
          ApplyEffects(result.damageData, result.effectProtection);
        }
      }
      else 
      {
        TakeDamage(damageData.WithHitboxModifier());
        ApplyEffects(damageData);
      }
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