﻿using Fralle.Core;
using Sirenix.OdinInspector;
using System;
using UnityEngine;

namespace Fralle.PingTap
{
  public class DamageController : MonoBehaviour
  {
    public event Action<DamageController, DamageData> OnDeath = delegate { };
    public event Action<DamageController, DamageData> OnDamageTaken = delegate { };
    public event Action<DamageController, DamageData> OnReceiveAttack = delegate { };
    public event Action<float, float> OnHealthChange = delegate { };

    [HideInInspector] public bool IsDead;
    [HideInInspector] public DamageEffectHandler effectHandler;

    [Header("Stats")]
    public bool Immortal;

    [ProgressBar("Health", "MaxHealth")]
    public float CurrentHealth;
    public float MaxHealth = 200f;

    [Header("Effects")]
    public GameObject impactEffect;

    [Space(10)]
    public Armor Armor;
    DamageDeathHandler deathHandler = new DamageDeathHandler();
    [SerializeField] DamageUIHandler uiHandler;
    [SerializeField] DamageIKHandler ikHandler;
    [SerializeField] DamageAudioHandler audioHandler;
    [SerializeField] DamageGraphicsHandler graphicsHandler;
    [SerializeField] DamageIndicatorHandler indicatorHandler;

    PostProcessController postProcessController;

    void Awake()
    {
      postProcessController = FindObjectOfType<PostProcessController>();

      if (ikHandler.enabled)
        ikHandler.Setup(this);

      effectHandler.Setup(this);
      deathHandler.Setup(this);
      uiHandler.Setup(this);
      audioHandler.Setup(this);
      graphicsHandler.Setup(this);
      indicatorHandler.Setup(this, postProcessController);
    }

    void Start()
    {
      if (CurrentHealth.EqualsWithTolerance(0f))
        CurrentHealth = MaxHealth;
    }

    void Update()
    {
      if (IsDead)
        return;

      effectHandler.DamageEffectsTick();
      uiHandler.Timer();

      if (ikHandler.enabled)
        ikHandler.LerpWeights();

      graphicsHandler.LerpColors();
    }

    public void ReceiveAttack(DamageData damageData)
    {
      if (IsDead)
        return;

      OnReceiveAttack(this, damageData);
      damageData = Armor.Protect(damageData, this);
      TakeDamage(damageData);
      effectHandler.ApplyEffects(damageData);
    }

    public void TakeDamage(DamageData damageData)
    {
      if (IsDead)
        return;

      damageData.Victim = this;

      if (damageData.DamageAmount <= 0)
      {
        damageData.Attacker?.stats.OnSuccessfulAttack(damageData);
        return;
      }

      CurrentHealth -= damageData.DamageAmount;
      OnHealthChange(CurrentHealth, MaxHealth);
      OnDamageTaken(this, damageData);
      if (CurrentHealth <= 0)
      {
        damageData.KillingBlow = true;
        damageData.Gib = CurrentHealth <= -MaxHealth * 0.5f;
        Death(damageData);
      }

      damageData.Attacker?.stats.OnSuccessfulAttack(damageData);
    }

    public void RaycastHit(float? customDelay = null)
    {
      uiHandler.Toggle(true, customDelay);
    }

    void Death(DamageData damageData)
    {
      if (IsDead)
        return;

      if (Immortal)
      {
        CurrentHealth = MaxHealth;
        OnHealthChange(CurrentHealth, MaxHealth);
      }
      else
      {
        IsDead = true;
        OnDeath(this, damageData);
      }
    }

    void OnDestroy()
    {
      if (ikHandler.enabled)
        ikHandler.Clean();

      deathHandler.Clean();
      uiHandler.Clean();
      graphicsHandler.Clean();
      audioHandler.Clean();
      indicatorHandler.Clean();
    }
  }
}
