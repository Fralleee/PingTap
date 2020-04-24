using System.Collections;
using System.Collections.Generic;
using Fralle;
using UnityEngine;

public class DropResource : MonoBehaviour
{
  public int credits;
  
  void Awake()
  {
    GetComponent<DamageController>().OnDeath += HandleDeath;
  }

  void HandleDeath(DamageController damageController, DamageData damageData)
  {
    if (!damageData.player) return;

    var inventory = damageData.player.GetComponentInParent<InventoryController>();
    if (inventory != null) inventory.Receive(credits);
  }

  void OnDestroy()
  {
    GetComponent<DamageController>().OnDeath -= HandleDeath;
  }
}
