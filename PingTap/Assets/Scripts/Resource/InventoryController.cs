using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static System.Single;

namespace Fralle.Resource
{
  public class InventoryController : MonoBehaviour
  {
    public event Action<int> OnCreditsUpdate = delegate { };

    public int credits;
    public List<InventoryItem> items = new List<InventoryItem>();

    [SerializeField] GameObject creditsPrefab;
    [SerializeField] float stayTime = MaxValue;

    public void Receive(int credits = 0, InventoryItem item = null)
    {
      if (credits > 0)
      {
        this.credits += credits;
        OnCreditsUpdate(this.credits);
      }

      if (item != null) items.Add(item);
    }

    public void Drop()
    {
      var creditsInstance = Instantiate(creditsPrefab, transform.position, Quaternion.identity);
      Destroy(creditsInstance, stayTime);

      foreach (var instance in items.Select(item => Instantiate(item.dropPrefab, transform.position, Quaternion.identity)))
      {
        Destroy(instance, stayTime);
      }
    }
  }
}