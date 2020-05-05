using Fralle.Attack.Defense;
using Fralle.Attack.Effect;
using Fralle.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Fralle.Attack.Offense
{
  public class Health : MonoBehaviour, IDamageable
  {
    public static event Action<Damage> OnAnyDamage = delegate { };
    public static event Action<Health> OnHealthBarAdded = delegate { };
    public static event Action<Health> OnHealthBarRemoved = delegate { };
    public event Action<Health, Damage> OnDeath = delegate { };
    public event Action<float, float> OnHealthChange = delegate { };

    [HideInInspector] public List<DamageEffect> damageEffects = new List<DamageEffect>();
    [HideInInspector] public bool isDead;

    [Header("Stats")]
    public float currentHealth;
    public float maxHealth = 100f;
    [SerializeField] bool immortal;

    Armor armor;
    Animator animator;
    bool isTouched;

    void Start()
    {
      armor = GetComponent<Armor>();
      if (currentHealth == 0) currentHealth = maxHealth;

      animator = GetComponent<Animator>();
    }

    void Update()
    {
      for (var i = 0; i < damageEffects.Count; i++)
      {
        damageEffects[i].Tick(this);
        if (!(damageEffects[i].timer > damageEffects[i].time)) continue;
        damageEffects[i].Exit(this);
        damageEffects.RemoveAt(i);
      }
    }

    public void ReceiveAttack(Damage damage)
    {
      if (armor) damage = armor.Protect(damage, this);
      TakeDamage(damage.WithHitboxModifier());
      ApplyEffects(damage);
    }

    public void TakeDamage(Damage damage)
    {
      if (isDead) return;

      if (damage.damageAmount <= 0)
      {
        OnAnyDamage(damage);
        return;
      }

      if (!isTouched)
      {
        OnHealthBarAdded(this);
        isTouched = true;
      }

      currentHealth = Mathf.Clamp(currentHealth - damage.damageAmount, 0, maxHealth);
      OnHealthChange(currentHealth, maxHealth);
      OnAnyDamage(damage);
      if (currentHealth <= 0) Death(damage);
    }

    void ApplyEffects(Damage damage)
    {
      foreach (var t in damage.effects)
      {
        var effect = t;
        var oldEffect = damageEffects.FirstOrDefault(x => x.name == effect.name);
        effect = effect.Append(oldEffect);
        effect.Enter(this);
        damageEffects.Upsert(oldEffect, effect);
      }
    }

    void Death(Damage damage)
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
        OnDeath(this, damage);
        if (animator) animator.enabled = false;
      }
    }
  }
}
