using Fralle.AI;
using Fralle.Attack.Offense;
using Pathfinding;
using UnityEngine;

public class EnemyBody : MonoBehaviour
{
  [Header("Graphics")]
  [SerializeField] GameObject model;
  [SerializeField] GameObject ragdoll;

  new Rigidbody rigidbody;
  Enemy enemy;
  AIPath aiPath;
  CapsuleCollider capsuleCollider;

  void Awake()
  {
    enemy = GetComponent<Enemy>();
    aiPath = GetComponent<AIPath>();

    rigidbody = GetComponent<Rigidbody>();
    rigidbody.isKinematic = true;

    capsuleCollider = GetComponent<CapsuleCollider>();
    capsuleCollider.enabled = false;

    if (ragdoll) ragdoll.SetActive(false);

    enemy.OnDeath += HandleDeath;
  }

  void FixedUpdate()
  {
    if (enemy.IsDead) return;
    if (!rigidbody.isKinematic && rigidbody.velocity.magnitude < 0.5f) GroundControl();
  }

  public void AddForce(Vector3 force, ForceMode forceMode = ForceMode.Force)
  {
    if (enemy.IsDead) return;
    rigidbody.isKinematic = false;
    aiPath.enabled = false;
    rigidbody.AddForce(force, forceMode);

  }
  public void AddExplosionForce(float explosionForce, Vector3 explosionPosition, float explosionRadius, float upwardsModifier)
  {
    if (enemy.IsDead) return;
    rigidbody.isKinematic = false;
    aiPath.enabled = false;
    rigidbody.AddExplosionForce(explosionForce, explosionPosition, explosionRadius, upwardsModifier);
  }

  void GroundControl()
  {
    if (!Physics.Raycast(transform.position, Vector3.down, capsuleCollider.height + 0.1f)) return;

    capsuleCollider.enabled = false;
    rigidbody.isKinematic = true;
    aiPath.enabled = true;

  }

  void HandleDeath(Damage damage)
  {
    capsuleCollider.enabled = false;
    rigidbody.isKinematic = true;

    if (ragdoll && model)
    {
      model.SetActive(false);
      ragdoll.SetActive(true);
    }

    if (damage == null) return;
    foreach (var rb in ragdoll.GetComponentsInChildren<Rigidbody>())
    {
      rb.AddExplosionForce(Random.Range(4000f, 6000f), damage.position, 5f);
    }

  }

  void OnDisable()
  {
    enemy.OnDeath -= HandleDeath;
  }
}
