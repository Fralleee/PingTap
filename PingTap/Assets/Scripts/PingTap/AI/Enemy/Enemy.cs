using CombatSystem.Combat;
using CombatSystem.Combat.Damage;
using Fralle.Core.Extensions;
using Fralle.Gameplay;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Fralle.AI
{
	[RequireComponent(typeof(DamageController))]
	public class Enemy : MonoBehaviour
	{
		public static event Action<Enemy> OnEnemyReachedPowerStone = delegate { };
		public static event Action<Enemy> OnAnyEnemyDeath = delegate { };
		public event Action<DamageData> OnDeath = delegate { };

		public static int TotalCount;
		public static int AliveCount;
		public static int KillCount;

		public static List<Enemy> AllAliveEnemies = new List<Enemy>();

		[HideInInspector] public DamageController DamageController;

		[Header("General")]
		public int DamageAmount = 1;

		public bool IsDead { get; private set; }
		public Combatant KilledByCombatant { get; private set; }

		void Awake()
		{
			DamageController = GetComponent<DamageController>();

			DamageController.OnDeath += HandleDeath;
			DamageController.OnDamageTaken += HandleDamageTaken;
			MatchManager.OnDefeat += HandleDefeat;

			IncrementOnSpawn();
			AllAliveEnemies.Add(this);

			transform.Find("UI").gameObject.SetActive(true);
		}

		void Update()
		{
			if (Input.GetKeyDown(KeyCode.K))
				Death(null, true);
		}

		[RuntimeInitializeOnLoadMethod]
		static void RunOnLoad()
		{
			TotalCount = 0;
			AliveCount = 0;
			KillCount = 0;
			AllAliveEnemies = new List<Enemy>();
		}

		static void IncrementOnSpawn()
		{
			TotalCount++;
			AliveCount++;
		}

		static void DecrementOnDeath()
		{
			AliveCount--;
			KillCount++;
		}

		public void ReachedDestination()
		{
			OnEnemyReachedPowerStone(this);
			Death(null, true);
		}

		public static void Despawn(int count)
		{
			if (count == 0)
				count = TotalCount;
			Enemy[] enemies = FindObjectsOfType<Enemy>();
			foreach (Enemy enemy in enemies)
			{
				if (count > 0)
				{
					enemy.Death(null, true);
					count--;
				}
			}
		}

		void HandleDamageTaken(DamageController damageController, DamageData damageData)
		{

		}

		void HandleDeath(DamageController damageController, DamageData damageData)
		{
			Death(damageData);
		}

		void HandleDefeat()
		{
			Destroy(gameObject);
		}


		public void Death(DamageData damageData, bool destroyImmediately = false)
		{
			if (IsDead)
				return;

			DecrementOnDeath();
			AllAliveEnemies.Remove(this);
			IsDead = true;
			KilledByCombatant = damageData?.Attacker;

			DeathEvents(damageData);
			DeathVisuals(destroyImmediately);
		}

		void DeathEvents(DamageData damageData)
		{
			OnAnyEnemyDeath(this);
			OnDeath(damageData);
		}

		void DeathVisuals(bool destroyImmediately)
		{
			if (destroyImmediately)
			{
				Destroy(gameObject);
				return;
			}

			gameObject.SetLayerRecursively(LayerMask.NameToLayer("Corpse"));

			Destroy(gameObject, 3f);
		}

		void OnDestroy()
		{
			DamageController.OnDeath -= HandleDeath;
			MatchManager.OnDefeat -= HandleDefeat;
		}
	}
}
