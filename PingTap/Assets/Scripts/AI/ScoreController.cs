﻿using Fralle.PingTap;
using Sirenix.OdinInspector;
using System;
using UnityEngine;

namespace Fralle.AI
{
  [RequireComponent(typeof(DamageController))]
  [SelectionBase]
  public class ScoreController : MonoBehaviour
  {
    public static event Action<ScoreController> OnAnyUnitDeath = delegate { };
    public event Action<DamageData> OnDeath = delegate { };

    [HideInInspector] public DamageController DamageController;
    [HideInInspector] public Combatant Combatant;

    [Header("Configuration")]
    public int startingScoreValue = 1;

    [Header("Values")]
    [ReadOnly] public int scoreValue;

    bool isDead;

    public void ReceiveScore(int score)
    {
      scoreValue += score;
    }

    void Awake()
    {
      DamageController = GetComponent<DamageController>();
      Combatant = GetComponent<Combatant>();

      DamageController.OnDeath += HandleDeath;
    }

    void Start()
    {
      scoreValue = startingScoreValue;
    }

    void HandleDeath(DamageController damageController, DamageData damageData)
    {
      Death(damageData);
    }

    public void Death(DamageData damageData, bool destroyImmediately = false)
    {
      if (isDead)
        return;

      isDead = true;

      if (damageData.Attacker)
      {
        ScoreController attackerScore = damageData.Attacker.GetComponent<ScoreController>();
        if (attackerScore)
          attackerScore.ReceiveScore(scoreValue);
      }

      DeathEvents(damageData);
    }

    void DeathEvents(DamageData damageData)
    {
      OnAnyUnitDeath(this);
      OnDeath(damageData);
    }

    void OnDestroy()
    {
      DamageController.OnDeath -= HandleDeath;
    }
  }
}
