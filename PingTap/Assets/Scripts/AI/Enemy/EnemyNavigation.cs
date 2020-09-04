using CombatSystem.Combat.Damage;
using Fralle.Core.Extensions;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

namespace Fralle.AI
{
  public abstract class EnemyNavigation : MonoBehaviour
  {
    public float currentMovementModifier = 1f;

    Enemy enemy;
    internal NavMeshAgent navMeshAgent;

    float stopTime;
    float movementSpeed;
    readonly Dictionary<string, float> movementModifiers = new Dictionary<string, float>();

    protected void Awake()
    {
      enemy = GetComponent<Enemy>();
      enemy.OnDeath += HandleDeath;

      navMeshAgent = GetComponent<NavMeshAgent>();
      movementSpeed = navMeshAgent.speed;
    }

    internal virtual void Update()
    {
      if (!(stopTime > 0f)) return;

      stopTime = Mathf.Clamp(stopTime - Time.deltaTime, 0, float.MaxValue);
      if (stopTime <= 0) RemoveModifier("DamageTaken");
    }

    internal abstract void SetDestination();

    internal void FinalDestination()
    {
      navMeshAgent.isStopped = false;
      if (enemy != null) enemy.ReachedDestination();
    }

    public void AddModifier(string modifierName, float modifier)
    {
      if (movementModifiers.ContainsKey(modifierName)) movementModifiers[modifierName] = modifier;
      else movementModifiers.Add(modifierName, modifier);
      currentMovementModifier = movementModifiers.OrderBy(x => x.Value).FirstOrDefault().Value;

      SetSpeed(movementSpeed * currentMovementModifier);
    }

    public void RemoveModifier(string modifierName)
    {
      movementModifiers.Remove(modifierName);
      if (movementModifiers.Count > 0)
      {
        var value = movementModifiers.OrderBy(x => x.Value).FirstOrDefault().Value;
        currentMovementModifier = value;
      }
      else currentMovementModifier = 1f;

      SetSpeed(movementSpeed * currentMovementModifier);
    }

    public void SetSpeed(float speed)
    {
      navMeshAgent.speed = speed;
    }

    public void StopMovement(float time)
    {
      AddModifier("DamageTaken", 0);
      stopTime = time;
    }

    void HandleDeath(DamageData damageData)
    {
      if (navMeshAgent) navMeshAgent.enabled = false;
    }

    internal bool PathComplete()
    {
      if (!(Vector3.Distance(navMeshAgent.destination, transform.position) <= navMeshAgent.stoppingDistance)) return false;
      return !navMeshAgent.hasPath || navMeshAgent.velocity.sqrMagnitude.EqualsWithTolerance(0f);
    }

    void OnDisable()
    {
      enemy.OnDeath -= HandleDeath;
    }
  }
}