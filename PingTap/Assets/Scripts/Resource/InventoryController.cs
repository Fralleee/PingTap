using System;
using System.Collections.Generic;
using UnityEngine;

namespace Fralle.Resource
{
  public class InventoryController : MonoBehaviour
  {
    public event Action<int> OnCreditsUpdate = delegate { };

    public int credits;
    public List<InventoryItem> items = new List<InventoryItem>();

    public void Receive(int droppedCredits = 0, InventoryItem item = null)
    {
      if (droppedCredits > 0)
      {
        this.credits += droppedCredits;
        OnCreditsUpdate(this.credits);
      }

      if (item != null) items.Add(item);
    }

  }
}