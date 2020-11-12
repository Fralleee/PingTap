using CombatSystem.Combat.Damage;
using Fralle.Core.Attributes;
using System;
using UnityEngine;

namespace Fralle.Gameplay
{
	public class SettingsManager : MonoBehaviour
	{
		[Space(10)]
		[Readonly] public int enemiesSpawned;
		[Readonly] public int enemiesKilled;
		[Readonly] public int totalEnemies;
		[Readonly] public float prepareTimer;
		[Readonly] public float totalTimer;
		[Readonly] public float waveTimer;

		[SerializeField] float prepareTime = 5f;
		[SerializeField] bool showColliders = true;

		void Awake()
		{
			PlayerPrefs.SetInt("showColliders", Convert.ToInt32(showColliders));
			Hitbox.ToggleColliders(showColliders);
		}

		void Update()
		{
			totalTimer += Time.deltaTime;
		}

		public void ResetPreparationTimer()
		{
			prepareTime = prepareTimer;
		}

	}
}
