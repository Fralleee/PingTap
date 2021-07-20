using CombatSystem;
using Fralle.PingTap.AI;
using UnityEngine;
using UnityEngine.AI;

namespace Fralle.AI
{
	public class AIAnimator : MonoBehaviour
	{
		[SerializeField] bool useSetSpeed = true;
		[SerializeField] bool dummy;

		AIBrain aiBrain;
		Animator animator;
		NavMeshAgent navMeshAgent;

		int animIsMoving;
		int animVelocity;

		void Awake()
		{
			aiBrain = GetComponentInParent<AIBrain>();
			animator = GetComponent<Animator>();
			navMeshAgent = GetComponentInParent<NavMeshAgent>();
			navMeshAgent = GetComponentInParent<NavMeshAgent>();
			DamageController damageController = GetComponentInParent<DamageController>();
			damageController.OnDeath += HandleDeath;

			animIsMoving = Animator.StringToHash("IsMoving");
			animVelocity = Animator.StringToHash("Velocity");
		}

		void Update()
		{
			if (dummy)
				return;

			animator.SetBool(animIsMoving, navMeshAgent.velocity.magnitude > 0.1f);
			animator.SetFloat(animVelocity, navMeshAgent.velocity.magnitude / aiBrain.runSpeed);
		}

		void OnAnimatorMove()
		{
			if (dummy)
				return;

			if (!useSetSpeed)
				navMeshAgent.speed = (animator.deltaPosition / Time.deltaTime).magnitude;
		}

		void HandleDeath(DamageController damageController, DamageData damageData)
		{
			animator.enabled = false;
			enabled = false;
		}
	}
}
