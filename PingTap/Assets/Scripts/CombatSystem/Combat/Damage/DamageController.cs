using CombatSystem.Combat.Target;
using CombatSystem.Defense;
using CombatSystem.Effect;
using CombatSystem.Interfaces;
using Fralle.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CombatSystem.Combat.Damage
{
  public class DamageController : MonoBehaviour, IDamageable
  {
    public static event Action<DamageData> OnAnyDamage = delegate { };
    public static event Action<DamageController> OnHealthBarAdded = delegate { };
    public static event Action<DamageController> OnHealthBarRemoved = delegate { };

    public event Action<DamageController, DamageData> OnDeath = delegate { };
    public event Action<DamageController, DamageData> OnDamageTaken = delegate { };
    public event Action<float, float> OnHealthChange = delegate { };

    [HideInInspector] public List<DamageEffect> damageEffects = new List<DamageEffect>();
    [HideInInspector] public bool isDead;

    [Header("UI")]
    [SerializeField] GameObject healthbarPrefab;
    [SerializeField] GameObject floatingNumbersPrefab;

    [Header("Stats")]
    public float currentHealth;
    public float maxHealth = 100f;
    public bool immortal;
    public Armor armor;


    bool isTouched;
    TargetController targetController;

    void Awake()
    {
      targetController = GetComponent<TargetController>();
      SetupUI();
    }

    void Start()
    {
      if (currentHealth.EqualsWithTolerance(0f)) currentHealth = maxHealth;
    }

    void Update()
    {
      if (isDead) return;
      for (var i = 0; i < damageEffects.Count; i++)
      {
        damageEffects[i].Tick(this);
        if (!(damageEffects[i].timer > damageEffects[i].time)) continue;
        damageEffects[i].Exit(this);
        damageEffects.RemoveAt(i);
      }
    }

    public void ReceiveAttack(DamageData damageData)
    {
      if (isDead) return;
      if (targetController) targetController.RaycastHit(1.5f);

      damageData = armor.Protect(damageData, this);
      TakeDamage(damageData);
      ApplyEffects(damageData);
    }

    public void TakeDamage(DamageData damageData)
    {
      if (isDead) return;

      damageData.victim = this;

      if (damageData.damageAmount <= 0)
      {
        OnAnyDamage(damageData);
        damageData.attacker?.Stats.OnSuccessfulAttack(damageData);
        return;
      }

      if (!isTouched)
      {
        OnHealthBarAdded(this);
        isTouched = true;
      }

      currentHealth = Mathf.Clamp(currentHealth - damageData.damageAmount, 0, maxHealth);
      OnHealthChange(currentHealth, maxHealth);
      OnAnyDamage(damageData);
      OnDamageTaken(this, damageData);
      if (currentHealth <= 0)
      {
        Death(damageData);
        damageData.killingBlow = true;
      }

      damageData.attacker?.Stats.OnSuccessfulAttack(damageData);
    }

    void ApplyEffects(DamageData damageData)
    {
      foreach (var t in damageData.effects)
      {
        var effect = t;
        var oldEffect = damageEffects.FirstOrDefault(x => x.name == effect.name);
        effect = effect.Append(oldEffect);
        effect.Enter(this);
        damageEffects.Upsert(oldEffect, effect);
      }
    }

    void Death(DamageData damageData)
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

    void SetupUI()
    {
      var uiTransform = transform.Find("UI");
      if (healthbarPrefab) Instantiate(healthbarPrefab, uiTransform);
      if (floatingNumbersPrefab) Instantiate(floatingNumbersPrefab, uiTransform);
    }
  }
}
