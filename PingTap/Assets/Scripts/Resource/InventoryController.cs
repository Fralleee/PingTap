using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
  public event Action<int> OnCreditsUpdate = delegate { };

  public int credits;
  public List<InventoryItem> items = new List<InventoryItem>();

  public void Receive(int credits = 0, InventoryItem item = null)
  {
    if (credits > 0)
    {
      this.credits += credits;
      OnCreditsUpdate(this.credits);
    }
    if (item != null) items.Add(item);
  }
}
