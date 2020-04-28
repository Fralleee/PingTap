using Fralle.Attack;
using UnityEngine;

public class DropResource : MonoBehaviour
{
  public int credits;

  void Awake()
  {
    GetComponent<Health>().OnDeath += HandleDeath;
  }

  void HandleDeath(Health health, Damage damage)
  {
    if (!damage.player) return;

    var inventory = damage.player.GetComponentInParent<InventoryController>();
    if (inventory != null) inventory.Receive(credits);
  }

  void OnDestroy()
  {
    GetComponent<Health>().OnDeath -= HandleDeath;
  }
}
