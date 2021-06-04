using CombatSystem.AI;
using CombatSystem.Combat;
using CombatSystem.Combat.Damage;
using CombatSystem.Targeting;
using Fralle.AI;
using Fralle.Core;
using Fralle.Core.AI;
using UnityEngine;
using UnityEngine.AI;

namespace Fralle.PingTap
{
	public class EnemyStateController : MonoBehaviour, IStateController
	{
		[HideInInspector] public Combatant Combatant;
		[HideInInspector] public DamageController DamageController;
		[HideInInspector] public Enemy Enemy;
		[HideInInspector] public TeamController TeamController;
		[HideInInspector] public AITargetingSystem AITargetingSystem;
		[HideInInspector] public AIAttack AIAttack;

		public bool Debug;

		public State currentState;

		[HideInInspector] public NavMeshAgent navMeshAgent;

		void Awake()
		{
			Combatant = GetComponent<Combatant>();
			DamageController = GetComponent<DamageController>();
			Enemy = GetComponent<Enemy>();
			TeamController = GetComponent<TeamController>();
			navMeshAgent = GetComponent<NavMeshAgent>();
			AITargetingSystem = GetComponent<AITargetingSystem>();
			AIAttack = GetComponent<AIAttack>();
		}

		void Start()
		{
			currentState = Instantiate(currentState);
			currentState.EnterState(this);
		}

		void Update()
		{
			currentState.UpdateState(this);
		}

		public void TransitionToState(State nextState)
		{
			if (nextState != currentState)
			{
				currentState.ExitState(this);
				currentState = Instantiate(nextState);
				currentState.EnterState(this);
			}
		}

		void OnDrawGizmos()
		{
			if (currentState != null)
			{
				Gizmos.color = currentState.sceneGizmoColor;
				Gizmos.DrawWireSphere(transform.position, 10f);
			}
		}
	}
}
