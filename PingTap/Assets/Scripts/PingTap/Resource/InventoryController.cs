using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace Fralle.Resource
{
	public class InventoryController : MonoBehaviour
	{
		public event Action<int> OnCreditsUpdate = delegate { };

		public int Credits;

		public List<InventoryItem> Items = new List<InventoryItem>();

		[SerializeField] float animationTime = 0.25f;

		Volume lootPickupVolume;

		float currentAnimationTime;

		void Awake()
		{
			var lootPickupObject = GameObject.Find("Loot Pickup Volume");
			lootPickupVolume = lootPickupObject?.GetComponent<Volume>();
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
				Credits += droppedCredits;
				OnCreditsUpdate(Credits);
			}

			if (item != null)
				Items.Add(item);

			currentAnimationTime = animationTime;
			lootPickupVolume.weight = 1;
		}

	}
}