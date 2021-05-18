using CombatSystem.Enums;
using System;
using UnityEngine;

namespace CombatSystem.Combat.Damage
{
	public class Hitbox : MonoBehaviour
	{
		public HitArea HitArea = HitArea.Major;
		public GameObject ActiveColliderVisualizer;

		void OnEnable()
		{
			bool showColliders = Convert.ToBoolean(PlayerPrefs.GetInt("showColliders"));
			ActiveColliderVisualizer.gameObject.SetActive(showColliders);
		}

		public static void ToggleColliders(bool show)
		{
			Hitbox[] hitboxes = FindObjectsOfType<Hitbox>();
			foreach (var hitbox in hitboxes)
			{
				hitbox.ActiveColliderVisualizer.gameObject.SetActive(show);
			}
		}
	}
}
