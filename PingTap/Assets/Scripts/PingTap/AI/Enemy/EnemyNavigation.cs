using CombatSystem.Combat.Damage;
using UnityEngine;

namespace Fralle.AI
{
	public class EnemyNavigation : MonoBehaviour
	{
		AiController aiController;
		AiNavigation aiNavigation;
		Enemy enemy;

		void Awake()
		{
			aiController = GetComponent<AiController>();
			aiNavigation = GetComponent<AiNavigation>();
			aiNavigation.OnFinalDestination += HandleFinalDestination;
			enemy = GetComponent<Enemy>();
			enemy.OnDeath += HandleDeath;
		}

		void Start()
		{
			if (aiNavigation.HasPurpose)
				aiController.IsMoving = true;
		}

		void HandleFinalDestination()
		{
			aiController.IsMoving = false;
			if (enemy != null)
				enemy.ReachedDestination();
		}

		void HandleDeath(DamageData damageData)
		{
			aiNavigation.Stop();
		}

		void OnDisable()
		{
			enemy.OnDeath -= HandleDeath;
		}
	}
}
