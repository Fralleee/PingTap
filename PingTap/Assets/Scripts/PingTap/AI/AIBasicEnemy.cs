using CombatSystem.Combat.Damage;
using CombatSystem.Targeting;
using Fralle.Core.Attributes;
using Fralle.Core.HFSM;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

namespace Fralle.PingTap.AI
{
	public partial class AIBasicEnemy : MonoBehaviour
	{
		[Readonly] public AIState currentState;

		[Header("Configuration")]
		[SerializeField] bool debugTransitions;
		[SerializeField] AIState startState;
		[SerializeField] float maxSearchStateTime = 4f;

		[Header("Gizmo meshes")]
		[SerializeField] Mesh wanderingMesh;
		[SerializeField] Mesh searchingMesh;
		[SerializeField] Mesh startledMesh;
		[SerializeField] Mesh chasingMesh;
		[SerializeField] Mesh battlingMesh;

		StateMachine<AIState> stateMachine;
		NavMeshAgent navMeshAgent;
		DamageController damageController;
		AISensor aiSensor;
		AITargetingSystem aiTargetingSystem;
		readonly HashSet<IState<AIState>> states = new HashSet<IState<AIState>>();

		[HideInInspector] public Vector3 attackedFrom;

		void Awake()
		{
			navMeshAgent = GetComponent<NavMeshAgent>();
			damageController = GetComponent<DamageController>();
			aiSensor = GetComponent<AISensor>();
			aiTargetingSystem = GetComponent<AITargetingSystem>();

			damageController.OnReceiveAttack += OnReceiveAttack;
		}

		void Start()
		{
			stateMachine = new StateMachine<AIState>();
			stateMachine.OnTransition += OnTransition;

			var wanderState = new WanderState(aiSensor, navMeshAgent);
			var searchState = new WanderState(aiSensor, navMeshAgent, true);
			var startledState = new StartledState(aiSensor, navMeshAgent);
			var chaseState = new ChaseState(aiTargetingSystem, navMeshAgent);

			states.Add(wanderState);
			states.Add(searchState);
			states.Add(startledState);
			states.Add(chaseState);
			//states.Add(battleState);


			// Transitions
			At(startledState, searchState, () => navMeshAgent.remainingDistance < 0.5f);
			At(searchState, wanderState, () => stateMachine.currentStateTime > maxSearchStateTime);

			At(wanderState, chaseState, () => aiTargetingSystem.TargetInSight);
			At(searchState, chaseState, () => aiTargetingSystem.TargetInSight);
			At(startledState, chaseState, () => aiTargetingSystem.TargetInSight);

			At(chaseState, wanderState, () => !aiTargetingSystem.HasTarget); // if target died
			At(chaseState, searchState, () => navMeshAgent.remainingDistance < 0.5f && !aiTargetingSystem.TargetInSight); // if target was lost

			stateMachine.SetState(wanderState);
		}

		void Update()
		{
			stateMachine.OnLogic();
		}

		void OnReceiveAttack(DamageController damageController, DamageData damageData)
		{
			if (currentState == AIState.Wandering || currentState == AIState.Searching)
			{
				StartledState startledState = (StartledState)states.FirstOrDefault(x => x.identifier == AIState.Startled);
				startledState.position = damageData.Attacker.AimTransform.position;
				stateMachine.SetState(startledState);
			}
		}

		void OnDestroy()
		{
			damageController.OnReceiveAttack -= OnReceiveAttack;
		}

		void OnTransition(IState<AIState> newState)
		{
			if (debugTransitions)
				Debug.Log($"Transitioned from {currentState} to {newState.identifier}");

			currentState = newState.identifier;
		}

		void At(IState<AIState> to, IState<AIState> from, Func<bool> condition) => stateMachine.AddTransition(to, from, condition);

		void OnDrawGizmos()
		{
			Gizmos.color = currentState.Color();
			switch (currentState)
			{
				case AIState.Wandering:
					Gizmos.DrawMesh(wanderingMesh, transform.position + Vector3.up * 2.5f, transform.rotation);
					break;
				case AIState.Searching:
					Gizmos.DrawMesh(searchingMesh, transform.position + Vector3.up * 2.5f, transform.rotation);
					break;
				case AIState.Startled:
					Gizmos.DrawMesh(startledMesh, transform.position + Vector3.up * 2.5f, transform.rotation);
					break;
				case AIState.Chasing:
					Gizmos.DrawMesh(chasingMesh, transform.position + Vector3.up * 2.5f, transform.rotation);
					break;
				case AIState.Battling:
					Gizmos.DrawMesh(battlingMesh, transform.position + Vector3.up * 2.5f, transform.rotation);
					break;
			}
		}
	}
}
