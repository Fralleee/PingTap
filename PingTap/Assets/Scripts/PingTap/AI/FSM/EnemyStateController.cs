using CombatSystem.Combat;
using CombatSystem.Combat.Damage;
using Fralle.AI;
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

		public DamageController target;
		public LayerMask HostileLayerMask;
		public float AttackRange = 10f;		
		public State currentState;

		[HideInInspector] public NavMeshAgent navMeshAgent;

		void Awake()
		{
			Combatant = GetComponent<Combatant>();
			DamageController = GetComponent<DamageController>();
			Enemy = GetComponent<Enemy>();
			navMeshAgent = GetComponent<NavMeshAgent>();
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
