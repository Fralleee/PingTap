using CombatSystem.Combat;
using CombatSystem.Combat.Damage;
using Fralle.Core.Extensions;
using Fralle.Gameplay;
using System;
using UnityEngine;

namespace Fralle.AI
{
  [RequireComponent(typeof(DamageController))]
  public class Enemy : MonoBehaviour
  {
    public static event Action<Enemy> OnEnemyReachedPowerStone = delegate { };
    public static event Action<Enemy> OnAnyEnemyDeath = delegate { };
    public event Action<DamageData> OnDeath = delegate { };

    [HideInInspector] public DamageController damageController;

    [Header("UI")]
    [SerializeField] GameObject infoPrefab;

    [Header("General")]
    public int damageAmount = 1;

    public bool IsDead { get; private set; }
    public Combatant KilledByCombatant { get; private set; }


    void Awake()
    {
      damageController = GetComponent<DamageController>();

      damageController.OnDeath += HandleDeath;
      damageController.OnDamageTaken += HandleDamageTaken;
      MatchManager.OnDefeat += HandleDefeat;

      SetupUI();
    }

    void Update()
    {
      if (Input.GetKeyDown(KeyCode.K)) Death(null, true);
    }

    public void ReachedDestination()
    {
      OnEnemyReachedPowerStone(this);
      Death(null, true);
    }

    void HandleDamageTaken(DamageController damageController, DamageData damageData)
    {

    }

    void HandleDeath(DamageController damageController, DamageData damageData)
    {
      Death(damageData);
    }

    void HandleDefeat()
    {
      Destroy(gameObject);
    }

    void Death(DamageData damageData, bool destroyImmediately = false)
    {
      if (IsDead) return;

      IsDead = true;
      KilledByCombatant = damageData?.attacker;

      DeathEvents(damageData);
      DeathVisuals(destroyImmediately);
    }

    void DeathEvents(DamageData damageData)
    {
      OnAnyEnemyDeath(this);
      OnDeath(damageData);
    }

    void DeathVisuals(bool destroyImmediately)
    {
      if (destroyImmediately)
      {
        Destroy(gameObject);
        return;
      }

      gameObject.SetLayerRecursively(LayerMask.NameToLayer("Corpse"));

      Destroy(gameObject, 3f);
    }

    void SetupUI()
    {
      var uiTransform = transform.Find("UI");
      Instantiate(infoPrefab, uiTransform);
    }

    void OnDestroy()
    {
      damageController.OnDeath -= HandleDeath;
      MatchManager.OnDefeat -= HandleDefeat;
    }
  }
}
