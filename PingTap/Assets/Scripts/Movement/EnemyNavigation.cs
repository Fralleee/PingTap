using Fralle.AI;
using Fralle.Attack.Offense;
using Pathfinding;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Fralle.Movement
{
  public class EnemyNavigation : MonoBehaviour
  {
    public WaypointSchema wayPointSchema;
    [SerializeField] float minStoppingDistance = 4f;
    [SerializeField] float maxStoppingDistance = 6f;
    public float currentMovementModifier = 1f;

    Enemy enemy;
    AIPath aiPath;

    Vector3 nextPosition;
    int waypointIndex;
    readonly Dictionary<string, float> movementModifiers = new Dictionary<string, float>();

    void Awake()
    {
      enemy = GetComponent<Enemy>();
      enemy.OnDeath += HandleDeath;

      aiPath = GetComponent<AIPath>();
      aiPath.endReachedDistance = Random.Range(minStoppingDistance, maxStoppingDistance);
    }

    void Start()
    {
      if (!wayPointSchema) return;
      SetDestination();

    }

    void Update()
    {
      if (aiPath.reachedDestination) SetNextDestination();
    }

    void SetDestination()
    {
      nextPosition = wayPointSchema.waypoints[waypointIndex];
      aiPath.destination = nextPosition;
    }

    void SetNextDestination()
    {
      if (!aiPath.enabled) return;
      waypointIndex++;
      if (waypointIndex > wayPointSchema.waypoints.Count - 1) FinalDestination();
      else SetDestination();
    }

    void FinalDestination()
    {
      aiPath.canMove = false;
      if (enemy != null) enemy.ReachedDestination();
    }

    public void AddModifier(string name, float modifier)
    {
      if (movementModifiers.ContainsKey(name)) movementModifiers[name] = modifier;
      else movementModifiers.Add(name, modifier);
      currentMovementModifier = movementModifiers.OrderBy(x => x.Value).FirstOrDefault().Value;
      enemy.enemyAnimator.SetModifier(currentMovementModifier);
    }

    public void RemoveModifier(string name)
    {
      movementModifiers.Remove(name);
      if (movementModifiers.Count > 0)
      {
        float value = movementModifiers.OrderBy(x => x.Value).FirstOrDefault().Value;
        currentMovementModifier = value;
      }
      else currentMovementModifier = 1f;

      enemy.enemyAnimator.SetModifier(currentMovementModifier);
    }

    public void SetSpeed(float speed)
    {
      aiPath.maxSpeed = speed;
    }

    void HandleDeath(Damage damage)
    {
      if (aiPath) aiPath.enabled = false;
    }

    void OnDisable()
    {
      enemy.OnDeath -= HandleDeath;
    }
  }
}