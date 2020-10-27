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

		[HideInInspector] public DamageController damageController;

		[Header("UI")]
		[SerializeField] GameObject infoPrefab;

		[Header("General")]
		public int damageAmount = 1;

		public bool IsDead { get; private set; }
		public Combatant KilledByCombatant { get; private set; }

		void Awake()
		{
			damageController = GetComponent<DamageController>();

			damageController.OnDeath += HandleDeath;
			damageController.OnDamageTaken += HandleDamageTaken;
			MatchManager.OnDefeat += HandleDefeat;

			SetupUI();

			IncrementOnSpawn();
			AllAliveEnemies.Add(this);
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


		void Death(DamageData damageData, bool destroyImmediately = false)
		{
			if (IsDead)
				return;

			DecrementOnDeath();
			AllAliveEnemies.Remove(this);
			IsDead = true;
			KilledByCombatant = damageData?.attacker;

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

		void SetupUI()
		{
			var uiTransform = transform.Find("UI");
			Instantiate(infoPrefab, uiTransform);
		}

		void OnDestroy()
		{
			damageController.OnDeath -= HandleDeath;
			MatchManager.OnDefeat -= HandleDefeat;
		}
	}
}
