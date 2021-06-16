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
		NavMeshAgent navMeshAgent;
		DamageController damageController;
		AISensor aiSensor;
		AITargetingSystem aiTargetingSystem;

		WanderState wanderState;
		WanderState searchState;
		StartledState startledState;
		ChaseState chaseState;

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
			aiTargetingSystem = aiBrain.GetComponent<AITargetingSystem>();
			damageController = aiBrain.GetComponent<DamageController>();
			navMeshAgent = aiBrain.GetComponent<NavMeshAgent>();

			damageController.OnReceiveAttack += OnReceiveAttack;
		}

		void SetupStates()
		{
			wanderState = new WanderState(aiBrain, aiSensor, navMeshAgent);
			searchState = new WanderState(aiBrain, aiSensor, navMeshAgent, true);
			startledState = new StartledState(aiBrain, aiSensor, navMeshAgent);
			chaseState = new ChaseState(aiBrain, aiTargetingSystem, navMeshAgent);
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
			damageController.OnReceiveAttack -= OnReceiveAttack;
		}

	}
}
