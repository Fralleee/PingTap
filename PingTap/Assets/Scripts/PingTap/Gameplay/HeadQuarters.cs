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

		public DamageController DamageController;
		public Vector3 EntryCoordinates = Vector3.zero;
		public Vector3 Entry => transform.position + EntryCoordinates;

		void Awake()
		{
			DamageController = GetComponent<DamageController>();
			DamageController.OnDeath += HandleDeath;
			Unit.OnUnitReachedDestination += HandleEnemyReachedPowerStone;
		}

		void HandleDeath(DamageController damageController, DamageData damageData)
		{
			OnDeath(this);
			EventManager.Broadcast(new GameOverEvent());
			Destroy(gameObject);
		}

		void HandleEnemyReachedPowerStone(Unit enemy)
		{
			DamageController.TakeDamage(new DamageData()
			{
				DamageAmount = enemy.DamageAmount
			});
		}

		void OnDestroy()
		{
			DamageController.OnDeath -= HandleDeath;
			Unit.OnUnitReachedDestination -= HandleEnemyReachedPowerStone;
		}
	}
}
