using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace Fralle.Resource
{
  public class InventoryController : MonoBehaviour
  {
    public event Action<int> OnCreditsUpdate = delegate { };

    public int credits;

    public List<InventoryItem> items = new List<InventoryItem>();

    [SerializeField] float animationTime = 0.25f;

    Volume lootPickupVolume;

    float currentAnimationTime;

    void Awake()
    {
      lootPickupVolume = GameObject.Find("Loot Pickup Volume").GetComponent<Volume>();
    }

    void Update()
    {
      if (currentAnimationTime > 0)
      {
        currentAnimationTime -= Time.deltaTime;
        lootPickupVolume.weight = Mathf.Lerp(0, 1, currentAnimationTime / animationTime);
      }
    }

    public void Receive(int droppedCredits = 0, InventoryItem item = null)
    {
      if (droppedCredits > 0)
      {
        credits += droppedCredits;
        OnCreditsUpdate(credits);
      }

      if (item != null) items.Add(item);

      currentAnimationTime = animationTime;
      lootPickupVolume.weight = 1;
    }

  }
}