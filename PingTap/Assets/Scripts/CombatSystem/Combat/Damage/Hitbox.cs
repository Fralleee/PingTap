using CombatSystem.Enums;
using System;
using UnityEngine;

namespace CombatSystem.Combat.Damage
{
	public class Hitbox : MonoBehaviour
	{
		public HitArea hitArea = HitArea.MAJOR;
		public GameObject activeColliderVisualizer;

		void OnEnable()
		{
			bool showColliders = Convert.ToBoolean(PlayerPrefs.GetInt("showColliders"));
			activeColliderVisualizer.gameObject.SetActive(showColliders);
		}

		public static void ToggleColliders(bool show)
		{
			var hitboxes = FindObjectsOfType<Hitbox>();
			foreach (var hitbox in hitboxes)
			{
				hitbox.activeColliderVisualizer.gameObject.SetActive(show);
			}
		}
	}
}
