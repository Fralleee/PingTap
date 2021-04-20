using CombatSystem.Combat.Damage;
using UnityEngine;
using UnityEngine.AI;

namespace Fralle.AI
{
	public class AiAnimator : MonoBehaviour
	{
		[SerializeField] bool useSetSpeed;

		Animator animator;
		AiController aiController;
		NavMeshAgent navMeshAgent;

		void Awake()
		{
			animator = GetComponent<Animator>();
			aiController = GetComponentInParent<AiController>();
			navMeshAgent = GetComponentInParent<NavMeshAgent>();
			var damageController = GetComponentInParent<DamageController>();
			damageController.OnDeath += HandleDeath;
		}

		void Update()
		{
			animator.SetBool("IsMoving", aiController.IsMoving);
		}

		void OnAnimatorMove()
		{
			if (!useSetSpeed)
				navMeshAgent.speed = (animator.deltaPosition / Time.deltaTime).magnitude;
		}

		void HandleDeath(DamageController damageController, DamageData damageData)
		{
			animator.enabled = false;
			this.enabled = false;
		}
	}
}
