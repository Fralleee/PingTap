using CombatSystem.Combat.Damage;
using Fralle.AI;
using Fralle.Resource;
using UnityEngine;

public class EnemyInventory : MonoBehaviour
{
  [Header("Kill reward")]
  [SerializeField] int minCredits = 1;
  [SerializeField] int maxCredits = 3;

  [Header("Drops")]
  [SerializeField] float dropChance = 0.125f;
  [SerializeField] LootTable lootTable = null;

  Enemy enemy;

  void Awake()
  {
    enemy = GetComponentInParent<Enemy>();
    enemy.OnDeath += HandleDeath;
  }

  void Reward(Enemy enemyP)
  {
    if (!enemyP.KilledByCombatant) return;

    var inventoryController = enemyP.KilledByCombatant.GetComponentInParent<InventoryController>();
    if (inventoryController != null) inventoryController.Receive(Random.Range(minCredits, maxCredits));
  }

  void Drop()
  {
    if (Random.value > dropChance) return;
    if (!lootTable) return;
    lootTable.DropLoot(transform.position);
  }

  void HandleDeath(DamageData damageData)
  {
    Reward(enemy);
    Drop();
  }
}
