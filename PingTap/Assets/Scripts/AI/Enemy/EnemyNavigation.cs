using CombatSystem.Combat.Damage;
using UnityEngine;

namespace Fralle.AI
{
	public class EnemyNavigation : MonoBehaviour
	{
		AIController aiController;
		AINavigation aiNavigation;
		Enemy enemy;

		void Awake()
		{
			aiController = GetComponent<AIController>();
			aiNavigation = GetComponent<AINavigation>();
			aiNavigation.OnFinalDestination += HandleFinalDestination;
			enemy = GetComponent<Enemy>();
			enemy.OnDeath += HandleDeath;
		}

		void Start()
		{
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
