using CombatSystem.AI;
using CombatSystem.Combat.Damage;
using CombatSystem.Targeting;
using Fralle.Core.AI;
using Fralle.PingTap.AI;
using UnityEngine;
using UnityEngine.AI;

namespace Fralle.PingTap
{
	[CreateAssetMenu(menuName = "AI/Basic enemy")]
	public class BasicEnemy : AIPersonality
	{
		[SerializeField] float maxSearchStateTime;

		StateMachine<AIState> stateMachine;
		AIBrain aiBrain;
		AIAttack aiAttack;
		NavMeshAgent navMeshAgent;
		DamageController damageController;
		AISensor aiSensor;
		AITargetingSystem aiTargetingSystem;

		WanderState wanderState;
		WanderState searchState;
		StartledState startledState;
		ChaseState chaseState;
		BattleState battleState;

		public override AIPersonality CreateInstance() => Instantiate(this);
		public override void Load(AIBrain aiBrain, StateMachine<AIState> stateMachine)
		{
			this.stateMachine = stateMachine;
			ResolveDependencies(aiBrain);
			SetupStates();
			SetupTransitions();

			this.stateMachine.SetState(wanderState);
		}

		void ResolveDependencies(AIBrain aiBrain)
		{
			this.aiBrain = aiBrain;
			aiSensor = aiBrain.GetComponent<AISensor>();
			aiAttack = aiBrain.GetComponent<AIAttack>();
			aiTargetingSystem = aiBrain.GetComponent<AITargetingSystem>();
			navMeshAgent = aiBrain.GetComponent<NavMeshAgent>();
			damageController = aiBrain.GetComponent<DamageController>();
			damageController.OnReceiveAttack += OnReceiveAttack;
		}

		void SetupStates()
		{
			wanderState = new WanderState(aiBrain, aiSensor, navMeshAgent);
			searchState = new WanderState(aiBrain, aiSensor, navMeshAgent, true);
			startledState = new StartledState(aiBrain, aiSensor, navMeshAgent);
			chaseState = new ChaseState(aiBrain, aiTargetingSystem, navMeshAgent);
			battleState = new BattleState(aiBrain, aiAttack, aiTargetingSystem, navMeshAgent);
		}

		void SetupTransitions()
		{
			stateMachine.AddTransition(startledState, searchState, () => navMeshAgent.remainingDistance < 0.5f);
			stateMachine.AddTransition(searchState, wanderState, () => stateMachine.currentStateTime > maxSearchStateTime);

			stateMachine.AddTransition(wanderState, chaseState, () => aiTargetingSystem.TargetInSight);
			stateMachine.AddTransition(searchState, chaseState, () => aiTargetingSystem.TargetInSight);
			stateMachine.AddTransition(startledState, chaseState, () => aiTargetingSystem.TargetInSight);

			stateMachine.AddTransition(chaseState, wanderState, () => !aiTargetingSystem.HasTarget); // if target died
			stateMachine.AddTransition(chaseState, searchState, () => navMeshAgent.remainingDistance < 0.5f && !aiTargetingSystem.TargetInSight); // if target was lost

			stateMachine.AddTransition(chaseState, battleState, () => navMeshAgent.remainingDistance < aiBrain.attackRange && aiTargetingSystem.TargetInSight); // target in range
			stateMachine.AddTransition(battleState, wanderState, () => !aiTargetingSystem.HasTarget); // target died
			stateMachine.AddTransition(battleState, chaseState, () => !aiTargetingSystem.TargetInSight); // target out of sight
			stateMachine.AddTransition(battleState, chaseState, () => navMeshAgent.remainingDistance > aiBrain.attackRange); // target out of range
		}

		void OnReceiveAttack(DamageController damageController, DamageData damageData)
		{
			if (stateMachine.CurrentState.identifier == AIState.Wandering || stateMachine.CurrentState.identifier == AIState.Searching)
			{
				startledState.position = damageData.Attacker.AimTransform.position;
				stateMachine.SetState(startledState);
			}
		}

		void OnDestroy()
		{
			if (damageController != null)
				damageController.OnReceiveAttack -= OnReceiveAttack;
		}

	}
}
