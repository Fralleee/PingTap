using Fralle.AI;
using Fralle.Resource;
using System;
using Random = UnityEngine.Random;

[Serializable]
public class DropResourceAction
{
  public int minCredits = 1;
  public int maxCredits = 3;

  public void Drop(Enemy enemy)
  {
    if (!enemy.KilledByPlayer) return;

    var inventoryController = enemy.KilledByPlayer.GetComponentInParent<InventoryController>();
    if (inventoryController != null) inventoryController.Receive(Random.Range(minCredits, maxCredits));
  }
}
