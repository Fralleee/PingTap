using CombatSystem.Combat.Damage;
using Fralle.AI;
using System;
using UnityEngine;

namespace Fralle.Gameplay
{
	[RequireComponent(typeof(DamageController))]
	public class HeadQuarters : MonoBehaviour
	{
		public event Action<HeadQuarters> OnDeath = delegate { };

		public DamageController damageController;
		public Vector3 entryCoordinates = Vector3.zero;
		public Vector3 Entry => transform.position + entryCoordinates;

		void Awake()
		{
			damageController = GetComponent<DamageController>();
			damageController.OnDeath += HandleDeath;
			Enemy.OnEnemyReachedPowerStone += HandleEnemyReachedPowerStone;
		}

		void HandleDeath(DamageController damageController, DamageData damageData)
		{
			OnDeath(this);
			EventManager.Broadcast(new GameOverEvent(false));
			Destroy(gameObject);
		}

		void HandleEnemyReachedPowerStone(Enemy enemy)
		{
			damageController.TakeDamage(new DamageData()
			{
				damageAmount = enemy.damageAmount
			});
		}

		void OnDestroy()
		{
			damageController.OnDeath -= HandleDeath;
			Enemy.OnEnemyReachedPowerStone -= HandleEnemyReachedPowerStone;
		}
	}
}
